// <copyright file="UserBufferController.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Linq;

using MfGames.TextTokens.Buffers;
using MfGames.TextTokens.Commands;
using MfGames.TextTokens.Lines;
using MfGames.TextTokens.Texts;
using MfGames.TextTokens.Tokens;

namespace MfGames.TextTokens.Controllers
{
	/// <summary>
	/// A specialized controller which takes input from user-initiated events, such
	/// as ones from an editor view, and maps them into buffer operations. There is 
	/// exactly one buffer for a given controller, but a buffer may have multiple
	/// user controllers and (potentially) processing controllers.
	/// </summary>
	public class UserBufferController
	{
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="UserBufferController"/> class.
		/// </summary>
		/// <param name="buffer">
		/// The buffer.
		/// </param>
		public UserBufferController(IBuffer buffer)
		{
			// Establish our contracts.
			Contract.Requires(buffer != null);

			// Copy the values together.
			Buffer = buffer;
			Selection = new BufferSelection(buffer);
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the buffer associated with the controller.
		/// </summary>
		/// <value>
		/// The buffer.
		/// </value>
		public IBuffer Buffer { get; private set; }

		/// <summary>
		/// Gets the anchor position of the selection.
		/// </summary>
		public TextLocation SelectionAnchor { get { return Selection.Anchor; } }

		/// <summary>
		/// Gets the cursor position of the selection.
		/// </summary>
		public TextLocation SelectionCursor { get { return Selection.Cursor; } }

		#endregion

		#region Properties

		/// <summary>
		/// Gets the selection associated with the controller.
		/// </summary>
		/// <value>
		/// The selection.
		/// </value>
		protected BufferSelection Selection { get; private set; }

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Deletes one character right of the current position.
		/// </summary>
		/// <param name="textCount">
		/// The character count.
		/// </param>
		public void DeleteRight(int textCount)
		{
			// Establish our contracts.
			Contract.Requires(textCount > 0);

			// We'll be gathering up lines to perform the insert.
			var command = new BufferCommand();

			// Allow the selection to add any operations to remove the selection.
			PostSelectionDeleteState state =
				Selection.AddDeleteOperations(command);

			// The number of characters we are deleting may jump to the next token
			// or even the next line. We keep track of how many characters we are
			// deleting and loop through the next tokens until we've deleted everything.
			int remainingCount = textCount;

			// Figure out how much we can delete from the current token. This will use
			// the text index to figure out where to start.
			string newText = state.CursorToken.Text;
			int textOffset = state.Cursor.TextIndex.Index;
			int maxDeletable = newText.Length - textOffset;

			if (maxDeletable > 0)
			{
				// Delete the characters from the first token.
				int deleteCount = Math.Min(
					maxDeletable,
					remainingCount);
				string beforeText = newText.Substring(
					0,
					textOffset);
				string afterText = newText.Substring(textOffset + deleteCount);
				newText = beforeText + afterText;

				// Decrement the number of characters we have removed.
				remainingCount -= deleteCount;
			}

			// Gather up a list with the token remaining on this line.
			ILine cursorLine = Buffer.Lines[state.Cursor.LineIndex.Index];
			int lineTokenCount = cursorLine.Tokens.Count;
			List<IToken> remainingTokens = state.RemainingTokens.ToList();

			// Loop until we run out of characters to delete. For as long as we have
			// characters to delete, we will remove the tokens until we are only removing
			// part of one. Then, the final one will have its remaining text appended
			// to the end of the newText. We start at 1 for this count because of the original
			// token we've already processed.
			var removeTokenCount = 1;
			var lineOffset = 0;

			while (remainingCount > 0)
			{
				// If we get to the end of the list, then we need to pull in the next line.
				if (remainingTokens.Count == 0)
				{
					// Increment the line offset which we'll be pulling the lines. And then
					// figure out the tokens on that line.
					lineOffset++;

					ILine nextLine = Buffer.Lines[lineOffset];
					ImmutableList<IToken> nextLineTokens = nextLine.Tokens;

					if (nextLineTokens.Count == 0)
					{
						throw new InvalidOperationException(
							"Cannot find additional line tokens.");
					}

					// Merge the tokens from the next line into this line using a replace
					// operation.
					command.Add(
						new ReplaceTokenOperation(
							state.Cursor.LineIndex,
							new TokenIndex(lineTokenCount),
							0,
							nextLineTokens));
					lineTokenCount += nextLineTokens.Count;

					// Delete the next line.
					command.Add(
						new DeleteLinesOperation(
							state.Cursor.LineIndex.Add(1),
							1));

					// Add these tokens to the remaining.
					remainingTokens.AddRange(nextLineTokens);

					// The newline is considered one "character".
					remainingCount--;
					continue;
				}

				// Get the next token from the list and add it to the list of tokens to remove.
				IToken nextToken = remainingTokens[0];
				remainingTokens.RemoveAt(0);
				removeTokenCount++;

				// If the remaining deleted characters exceed the length of the token,
				// we'll be removing it entirely.
				if (nextToken.Text.Length <= remainingCount)
				{
					// We are removing this one entirely.
					remainingCount -= nextToken.Text.Length;
					continue;
				}

				// If we got this far, then we are removing a portion of this token
				// and appending it.
				string afterText = nextToken.Text.Substring(remainingCount);

				newText += afterText;
				break;
			}

			// Create the command to replace the tokens with a new one.
			IToken newToken = Buffer.CreateToken(newText);

			var replaceOperation = new ReplaceTokenOperation(
				state.Cursor,
				removeTokenCount,
				newToken);
			command.Add(replaceOperation);

			// Submit the command to the buffer.
			Buffer.Do(command);
		}

		/// <summary>
		/// Inserts text into a given location, choosing the appropriate token to modify
		/// and passing it into the buffer as an undoable command.
		/// </summary>
		/// <param name="textLocation">
		/// The location to insert into the buffer.
		/// </param>
		/// <param name="text">
		/// The text to insert.
		/// </param>
		public void InsertText(
			TextLocation textLocation,
			string text)
		{
			// Establish our contracts.
			Contract.Requires(text != null);

			// Set the cursor and then insert the text.
			SetCursor(textLocation);
			InsertText(text);
		}

		/// <summary>
		/// Inserts text at the cursor position, choosing the appropriate token to modify
		/// and passing it into the buffer as a command with a set of operations.
		/// </summary>
		/// <param name="text">
		/// The text to insert.
		/// </param>
		public void InsertText(string text)
		{
			// Establish our contracts.
			Contract.Requires(text != null);

			// We'll be gathering up lines to perform the insert.
			var command = new BufferCommand();

			// Allow the selection to add any operations to remove the selection.
			PostSelectionDeleteState state =
				Selection.AddDeleteOperations(command);

			// Determine if we are doing a single or multi-line insert.
			text = text.NormalizeNewlines();

			if (text.Contains("\n"))
			{
				// Insert multiple lines of text.
				InsertMultipleLines(
					text,
					state,
					command);
			}
			else
			{
				// Insert a single line text.
				InsertSingleLine(
					text,
					state,
					command);
			}

			// Submit the command to the buffer.
			Buffer.Do(command);
		}

		/// <summary>
		/// Re-executes the last reversed command on the buffer, or do nothing if there are no
		/// commands that can be redone.
		/// </summary>
		public void Redo()
		{
			Buffer.Redo();
		}

		/// <summary>
		/// Extends the selection to the new location.
		/// </summary>
		/// <param name="cursor">
		/// The cursor.
		/// </param>
		public void Select(TextLocation cursor)
		{
			Selection.Select(cursor);
		}

		/// <summary>
		/// Sets the cursor location in the selection.
		/// </summary>
		/// <param name="textLocation">
		/// The text location.
		/// </param>
		public void SetCursor(TextLocation textLocation)
		{
			Selection.SetCursor(textLocation);
		}

		/// <summary>
		/// Reverses the last command on the buffer or nothing if there are no commands
		/// that can be reversed.
		/// </summary>
		public void Undo()
		{
			Buffer.Undo();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Sets up the various operations to insert multiple lines of text into
		/// the buffer.
		/// </summary>
		/// <param name="text">
		/// The text.
		/// </param>
		/// <param name="state">
		/// The state after the selection.
		/// </param>
		/// <param name="command">
		/// The command.
		/// </param>
		private void InsertMultipleLines(
			string text,
			PostSelectionDeleteState state,
			BufferCommand command)
		{
			// Split the text of the old token where we're going to insert the values. We
			// append the first line to the before token and prepend the last line to the
			// after token to get a complete token. We don't have to worry about splitting
			// the tokens since the buffer will handle that.
			string[] lines = text.Split(
				new[] { '\n' },
				StringSplitOptions.None);
			int lastTokenlength = lines[lines.Length - 1].Length;
			string before = state.CursorToken.Text.Substring(
				0,
				state.Cursor.TextIndex.Index) + lines[0];
			string after = lines[lines.Length - 1]
				+ state.CursorToken.Text.Substring(state.Cursor.TextIndex.Index);

			lines[lines.Length - 1] = after;

			// We need to create lines for all but the first one.
			int newLineCount = lines.Length - 1;

			command.Add(
				new InsertLinesOperation(
					state.Cursor.LineIndex.Add(1),
					newLineCount));

			// Move all tokens to the right of the first line into the last one.
			ILine beforeLine = Buffer.Lines[state.Cursor.LineIndex.Index];
			ImmutableList<IToken> movedTokens =
				beforeLine.Tokens.GetRange(
					state.Cursor.TextIndex.Index + 1,
					beforeLine.Tokens.Count - state.Cursor.TextIndex.Index - 1);

			// Remove the tokens from the first line.
			command.Add(
				new ReplaceTokenOperation(
					state.Cursor.LineIndex,
					state.Cursor.TokenIndex.Add(1),
					movedTokens.Count));

			// Add the tokens into the last line.
			command.Add(
				new ReplaceTokenOperation(
					state.Cursor.LineIndex.Add(newLineCount),
					TokenIndex.First,
					0,
					movedTokens));

			// Append the new tokens into each line after the first one.
			for (var index = 1; index < lines.Length; index++)
			{
				// Create a token for this line's text and add it to that line.
				IToken lineToken = Buffer.CreateToken(lines[index]);

				command.Add(
					new ReplaceTokenOperation(
						state.Cursor.LineIndex.Add(index),
						TokenIndex.First,
						0,
						lineToken));
			}

			// Figure out the new text of the string and create a new token with the modified
			// version. This will also copy the attributes of the old token.
			IToken newToken = Buffer.CreateToken(
				state.CursorToken,
				before);

			// Add the text replacement command into the buffer.
			const int SingleTokenReplacement = 1;

			command.Add(
				new ReplaceTokenOperation(
					state.Cursor.LineIndex,
					state.Cursor.TokenIndex,
					SingleTokenReplacement,
					newToken));

			// Update the cursor location.
			var newCursor =
				new TextLocation(
					state.Cursor.LineIndex.Add(newLineCount),
					TokenIndex.First,
					new TextIndex(lastTokenlength));

			command.Add(new ReplaceSelectionOperation(newCursor));
		}

		/// <summary>
		/// Sets up the various operations to insert a single line text into the buffer.
		/// </summary>
		/// <param name="text">
		/// The text.
		/// </param>
		/// <param name="state">
		/// The state.
		/// </param>
		/// <param name="command">
		/// The command.
		/// </param>
		private void InsertSingleLine(
			string text,
			PostSelectionDeleteState state,
			BufferCommand command)
		{
			// Figure out the new text of the string and create a new token with the modified
			// version. This will also copy the attributes of the old token.
			string newText =
				state.CursorToken.Text.Insert(
					state.Cursor.TextIndex.Index,
					text);
			IToken newToken = Buffer.CreateToken(
				state.CursorToken,
				newText);

			// Add the text replacement command into the buffer.
			const int SingleTokenReplacement = 1;

			command.Add(
				new ReplaceTokenOperation(
					state.Cursor.LineIndex,
					state.Cursor.TokenIndex,
					SingleTokenReplacement,
					newToken));

			// Update the cursor location.
			TextLocation newCursor =
				Selection.First.AddTextIndex(text.Length);

			command.Add(new ReplaceSelectionOperation(newCursor));
		}

		#endregion
	}
}
