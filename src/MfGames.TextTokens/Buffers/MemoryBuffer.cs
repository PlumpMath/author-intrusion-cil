// <copyright file="MemoryBuffer.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;

using MfGames.TextTokens.Commands;
using MfGames.TextTokens.Events;
using MfGames.TextTokens.Lines;
using MfGames.TextTokens.Texts;
using MfGames.TextTokens.Tokens;

namespace MfGames.TextTokens.Buffers
{
	/// <summary>
	/// Implements an IBuffer that is based by an in-memory array of line objects.
	/// </summary>
	public class MemoryBuffer : IBuffer
	{
		#region Fields

		/// <summary>
		/// The changed lines since the last operation.
		/// </summary>
		private readonly HashSet<ILine> changedLines;

		/// <summary>
		/// The lines contained with the buffer.
		/// </summary>
		private readonly List<Line> lines;

		/// <summary>
		/// Contains the token splitter associated with the buffer.
		/// </summary>
		private ITokenSplitter tokenSplitter;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="MemoryBuffer"/> class.
		/// </summary>
		public MemoryBuffer()
		{
			lines = new List<Line>();
			changedLines = new HashSet<ILine>();
			TokenSplitter = new DefaultTokenSplitter();
			UndoCommands = new Stack<BufferCommand>();
			RedoCommands = new Stack<BufferCommand>();
		}

		#endregion

		#region Public Events

		/// <summary>
		/// Occurs when lines are deleted from the buffer.
		/// </summary>
		public event EventHandler<LineIndexLinesDeletedEventArgs> LinesDeleted;

		/// <summary>
		/// Occurs when lines are inserted into the buffer.
		/// </summary>
		public event EventHandler<LineIndexLinesInsertedEventArgs> LinesInserted;

		/// <summary>
		/// Occurs when the selection should be replaced because of a command.
		/// </summary>
		public event EventHandler<ReplaceSelectionEventArgs> ReplaceSelection;

		/// <summary>
		/// Occurs when the selection should be restored, typically after
		/// undoing a command.
		/// </summary>
		public event EventHandler<RestoreSelectionEventArgs> RestoreSelection;

		/// <summary>
		/// Occurs when a token is replaced by zero or more tokens.
		/// </summary>
		public event EventHandler<LineIndexTokenIndexTokensReplacedEventArgs>
			TokensReplaced;

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the lines contained within the buffer.
		/// </summary>
		/// <value>
		/// The lines.
		/// </value>
		public IReadOnlyList<ILine> Lines { get { return lines.AsReadOnly(); } }

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the token parser for this buffer.
		/// </summary>
		/// <value>
		/// The token parser.
		/// </value>
		/// <exception cref="System.ArgumentNullException">value;Cannot assign a null TokenSplitter to the buffer.</exception>
		protected ITokenSplitter TokenSplitter
		{
			get { return tokenSplitter; }

			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(
						"value",
						"Cannot assign a null TokenSplitter to the buffer.");
				}

				tokenSplitter = value;
			}
		}

		/// <summary>
		/// Gets or sets the redo commands.
		/// </summary>
		/// <value>
		/// The redo commands.
		/// </value>
		private Stack<BufferCommand> RedoCommands { get; }

		/// <summary>
		/// Gets or sets the undo commands currently on the buffer.
		/// </summary>
		/// <value>
		/// The undo commands.
		/// </value>
		private Stack<BufferCommand> UndoCommands { get; }

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Creates a new token.
		/// </summary>
		/// <param name="newText">
		/// The new text.
		/// </param>
		/// <returns>
		/// A constructed token.
		/// </returns>
		public IToken CreateToken(string newText)
		{
			TokenKey tokenKey = KeyGenerator.Instance.GetNextTokenKey();
			var token = new Token(
				tokenKey,
				newText);
			return token;
		}

		/// <summary>
		/// Constructs a new token that is copied from the old one except
		/// for the given text.
		/// </summary>
		/// <param name="oldToken">
		/// The old token.
		/// </param>
		/// <param name="newText">
		/// The new text.
		/// </param>
		/// <returns>
		/// A new token.
		/// </returns>
		public IToken CreateToken(
			IToken oldToken,
			string newText)
		{
			TokenKey tokenKey = KeyGenerator.Instance.GetNextTokenKey();
			var token = new Token(
				tokenKey,
				newText);
			return token;
		}

		/// <summary>
		/// Deletes lines from the buffer, starting with the given index.
		/// </summary>
		/// <param name="lineIndex">
		/// Index of the line to start deleting.
		/// </param>
		/// <param name="count">
		/// The number of lines to delete.
		/// </param>
		/// <returns>
		/// The lines deleted from the buffer.
		/// </returns>
		public IEnumerable<ILine> DeleteLines(
			LineIndex lineIndex,
			int count)
		{
			// Retrieve the list of lines deleted.
			List<Line> deletedLines = lines.GetRange(
				lineIndex.Index,
				count);

			// Remove the lines from the buffer.
			lines.RemoveRange(
				lineIndex.Index,
				count);

			// Raise the event to indicate we deleted the lines.
			RaiseLinesDeleted(
				lineIndex,
				count);

			// Return the resulting lines.
			return deletedLines;
		}

		/// <summary>
		/// Executes a command on the buffer, running through each operation in turn.
		/// </summary>
		/// <param name="command">
		/// The command.
		/// </param>
		public void Do(BufferCommand command)
		{
			// Performs the operations of the command.
			command.Do(this);

			// Add this command to our undo stack.
			UndoCommands.Push(command);
		}

		/// <summary>
		/// Gets the index of the line.
		/// </summary>
		/// <param name="line">
		/// The line.
		/// </param>
		/// <returns>
		/// A line index of the given line.
		/// </returns>
		public LineIndex GetLineIndex(ILine line)
		{
			// Establish our contracts.
			Contract.Requires(line != null);

			// Find the line's index, wrap it, and return it.
			int index = lines.FindIndex(l => l.LineKey == line.LineKey);
			var lineIndex = new LineIndex(index);
			return lineIndex;
		}

		/// <summary>
		/// Retrieves the token at the given indexes.
		/// </summary>
		/// <param name="lineIndex">
		/// Index of the line.
		/// </param>
		/// <param name="tokenIndex">
		/// Index of the token.
		/// </param>
		/// <returns>
		/// The token at the given indexes.
		/// </returns>
		public IToken GetToken(
			LineIndex lineIndex,
			TokenIndex tokenIndex)
		{
			ILine line = Lines[lineIndex.Index];
			IToken token = line.Tokens[tokenIndex.Index];
			return token;
		}

		/// <summary>
		/// Retrieves operations to normalize the changed lines including rebuilding
		/// tokens.
		/// </summary>
		/// <returns>
		/// An enumeration of buffer operations to normalize the changed lines.
		/// </returns>
		public IEnumerable<IBufferOperation> GetUpdateOperations()
		{
			// Go through each line and process them in turn.
			var operations = new List<IBufferOperation>();

			foreach (Line line in changedLines)
			{
				// See if we can find the line.
				int lineIndex = lines.IndexOf(line);

				if (lineIndex < 0)
				{
					// The line doesn't exist, so skip it.
					continue;
				}

				// Grab the visible text and figure out if we need to retokenize it.
				TokenList<Token> originalTokens = line.Tokens;
				string originalText = originalTokens.GetVisibleText();

				string[] newTokenTexts =
					tokenSplitter.Tokenize(originalText)
						.ToArray();

				// If the tokens don't match the visible ones exactly, we simply replace the
				// entire line.
				bool rebuildLine = originalTokens.Count != newTokenTexts.Count();

				if (!rebuildLine)
				{
					for (var index = 0; index < originalTokens.Count; index++)
					{
						if (originalTokens[index].Text != newTokenTexts[index])
						{
							rebuildLine = true;
							break;
						}
					}
				}

				// If we aren't rebuilding the line, then move to the next line.
				if (!rebuildLine)
				{
					continue;
				}

				// If we are rebuilding the line, then add in a replace tokens operation, which
				// will remove all non-visible tokens. But, we'll be rebuilding the lines
				// anyways, so it should be acceptable.
				IEnumerable<IToken> newTokens =
					newTokenTexts.Select(t => CreateToken(t));

				operations.Add(
					new ReplaceTokenOperation(
						new LineIndex(lineIndex),
						TokenIndex.First,
						originalTokens.Count,
						newTokens));
			}

			// Return the resulting operations.
			return operations;
		}

		/// <summary>
		/// Inserts the lines.
		/// </summary>
		/// <param name="afterLineIndex">
		/// Index of the after line.
		/// </param>
		/// <param name="count">
		/// The count.
		/// </param>
		/// <returns>
		/// An enumerable of the created lines.
		/// </returns>
		public IEnumerable<ILine> InsertLines(
			LineIndex afterLineIndex,
			int count)
		{
			// First populate a list of line keys for the new lines.
			var insertedLines = new Line[count];

			for (var index = 0; index < count; index++)
			{
				LineKey lineKey = KeyGenerator.Instance.GetNextLineKey();
				insertedLines[index] = new Line(lineKey);
			}

			// Insert the lines into the buffer.
			InsertLines(
				afterLineIndex,
				insertedLines);

			// Return the resulting lines.
			return insertedLines;
		}

		/// <summary>
		/// Inserts the lines.
		/// </summary>
		/// <param name="afterLineIndex">
		/// Index of the after line.
		/// </param>
		/// <param name="insertedLines">
		/// The inserted lines.
		/// </param>
		public void InsertLines(
			LineIndex afterLineIndex,
			params Line[] insertedLines)
		{
			// Establish our contracts.
			Contract.Requires(afterLineIndex.Index >= 0);
			Contract.Requires(insertedLines != null);

			// Insert the lines into the buffer.
			InsertLines(
				afterLineIndex,
				(IEnumerable<Line>)insertedLines);

			// Mark that all of these lines changed.
			Array.ForEach(
				insertedLines,
				l => changedLines.Add(l));
		}

		/// <summary>
		/// Inserts the lines into the buffer.
		/// </summary>
		/// <param name="afterLineIndex">
		/// Index of the after line.
		/// </param>
		/// <param name="insertedLines">
		/// The inserted lines.
		/// </param>
		public void InsertLines(
			LineIndex afterLineIndex,
			IEnumerable<ILine> insertedLines)
		{
			// Subscribe to the events of these lines.
			List<Line> lineArray =
				insertedLines.Select(l => (l as Line) ?? new Line(l))
					.ToList();

			lineArray.ForEach(
				l => l.TokensReplaced += OnLineTokensReplaced);

			// Insert the lines into the buffer at the given position.
			lines.InsertRange(
				afterLineIndex.Index,
				lineArray);

			// Raise an event for the inserted lines.
			RaiseLinesInserted(
				afterLineIndex,
				lineArray);
		}

		/// <summary>
		/// Raises the ReplaceSelection event with the given arguments.
		/// </summary>
		/// <param name="newTextRange">
		/// The new text range.
		/// </param>
		/// <returns>
		/// A dictionary of the old selection items.
		/// </returns>
		public Dictionary<object, TextRange> RaiseReplaceSelection(
			TextRange newTextRange)
		{
			// If we don't have any listeners, then return an empty selection.
			EventHandler<ReplaceSelectionEventArgs> listeners =
				ReplaceSelection;

			if (listeners == null)
			{
				return new Dictionary<object, TextRange>();
			}

			// Otherwise, raise the event.
			var args = new ReplaceSelectionEventArgs(newTextRange);

			listeners(
				this,
				args);

			return args.OldTextRanges;
		}

		/// <summary>
		/// Raises the restore selection event.
		/// </summary>
		/// <param name="oldTextRanges">
		/// The old text ranges.
		/// </param>
		public void RaiseRestoreSelection(
			Dictionary<object, TextRange> oldTextRanges)
		{
			// If we have no listeners, then don't do anything.
			EventHandler<RestoreSelectionEventArgs> listeners =
				RestoreSelection;

			if (listeners == null)
			{
				return;
			}

			// Otherwise, raise the event.
			var args = new RestoreSelectionEventArgs(oldTextRanges);

			listeners(
				this,
				args);
		}

		/// <summary>
		/// Re-executes the last undone command (reverses the undo) or do nothing if
		/// there are no re-doable commands.
		/// </summary>
		public void Redo()
		{
			// If there are no undo commands, then do nothing.
			if (RedoCommands.Count == 0)
			{
				return;
			}

			// Grab the last command and reexecute it.
			BufferCommand command = RedoCommands.Pop();
			UndoCommands.Push(command);

			// Perform the action.
			command.Do(this);
		}

		/// <summary>
		/// Replaces the tokens.
		/// </summary>
		/// <param name="lineIndex">
		/// Index of the line.
		/// </param>
		/// <param name="tokenIndex">
		/// Index of the token.
		/// </param>
		/// <param name="count">
		/// The count.
		/// </param>
		/// <param name="newTokens">
		/// The new tokens.
		/// </param>
		/// <returns>
		/// The tokens replaced.
		/// </returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// count;Count cannot be less than zero.
		/// </exception>
		/// <exception cref="System.ArgumentNullException">
		/// newTokens;newTokens cannot be null.
		/// </exception>
		public IEnumerable<IToken> ReplaceTokens(
			LineIndex lineIndex,
			TokenIndex tokenIndex,
			int count,
			IEnumerable<IToken> newTokens)
		{
			// Establish our contracts.
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException(
					"count",
					"Count cannot be less than zero.");
			}

			if (newTokens == null)
			{
				throw new ArgumentNullException(
					"newTokens",
					"newTokens cannot be null.");
			}

			// Get the line and tokens for this request.
			Line line = lines[lineIndex.Index];
			List<Token> oldTokens = line.Tokens.GetRange(
				tokenIndex.Index,
				count);

			// Make sure we have the right type.
			Token[] tokenArray =
				newTokens.Select(t => t as Token ?? new Token(t))
					.ToArray();

			// Figure out if these two tokens are identity.
			string oldText = oldTokens.GetVisibleText();
			string newText = tokenArray.GetVisibleText();
			TokenReplacement replacementType = oldText == newText
				? TokenReplacement.Identity
				: TokenReplacement.Different;

			// Replace the tokens in our collection.
			line.Tokens.RemoveRange(
				tokenIndex.Index,
				count);
			line.Tokens.InsertRange(
				tokenIndex.Index,
				tokenArray);

			// Raise an event about the change.
			RaiseTokensReplaced(
				lineIndex,
				tokenIndex,
				count,
				tokenArray,
				replacementType);
			changedLines.Add(line);

			// Return the replaced tokens.
			return oldTokens;
		}

		/// <summary>
		/// Executes the reverse operation of the last command or do nothing if there
		/// are no undoable commands.
		/// </summary>
		public void Undo()
		{
			// If there are no undo commands, then do nothing.
			if (UndoCommands.Count == 0)
			{
				return;
			}

			// Grab the last command and reexecute it.
			BufferCommand command = UndoCommands.Pop();
			RedoCommands.Push(command);

			// Perform the undo action and update the internal state.
			command.Undo(this);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Raises the LinesInserted event.
		/// </summary>
		/// <param name="afterLineIndex">
		/// Index of the after line.
		/// </param>
		/// <param name="insertedLines">
		/// The inserted lines.
		/// </param>
		protected void RaiseLinesInserted(
			LineIndex afterLineIndex,
			IEnumerable<ILine> insertedLines)
		{
			EventHandler<LineIndexLinesInsertedEventArgs> listener =
				LinesInserted;

			if (listener == null)
			{
				return;
			}

			ReadOnlyCollection<ILine> readOnlyLines =
				new List<ILine>(insertedLines).AsReadOnly();
			var args = new LineIndexLinesInsertedEventArgs(
				afterLineIndex,
				readOnlyLines);

			listener(
				this,
				args);
		}

		/// <summary>
		/// Raises the token replaced event.
		/// </summary>
		/// <param name="lineIndex">
		/// Index of the line.
		/// </param>
		/// <param name="tokenIndex">
		/// Index of the token.
		/// </param>
		/// <param name="count">
		/// The number of tokens to replace.
		/// </param>
		/// <param name="replacementTokens">
		/// The replacement tokens.
		/// </param>
		/// <param name="replacementType">
		/// Type of the replacement.
		/// </param>
		protected void RaiseTokensReplaced(
			LineIndex lineIndex,
			TokenIndex tokenIndex,
			int count,
			IEnumerable<IToken> replacementTokens,
			TokenReplacement replacementType)
		{
			// Make sure we have at least one listener for this event.
			EventHandler<LineIndexTokenIndexTokensReplacedEventArgs> listener =
				TokensReplaced;

			if (listener == null)
			{
				return;
			}

			// Ensure that the collection is wrapped properly.
			ImmutableArray<IToken> tokenArray =
				replacementTokens.ToImmutableArray();

			// Call the event with the property event arguments.
			var args = new LineIndexTokenIndexTokensReplacedEventArgs(
				lineIndex,
				tokenIndex,
				count,
				tokenArray,
				replacementType);

			listener(
				this,
				args);
		}

		/// <summary>
		/// Called when tokens are inserted into line.
		/// </summary>
		/// <param name="sender">
		/// The sender.
		/// </param>
		/// <param name="e">
		/// The <see cref="TokenIndexTokensReplacedEventArgs"/> instance containing the event data.
		/// </param>
		private void OnLineTokensReplaced(
			object sender,
			TokenIndexTokensReplacedEventArgs e)
		{
			// Establish our contracts.
			Contract.Requires(sender is Line);

			// Get our line and its index.
			var line = (Line)sender;
			LineIndex lineIndex = GetLineIndex(line);

			// We need to keep track of the lines that have been updated so we can retokenize
			// them later.
			changedLines.Add(line);

			// Pass the event to our listeners.
			RaiseTokensReplaced(
				lineIndex,
				e.TokenIndex,
				e.Count,
				e.TokensInserted,
				e.ReplacementType);
		}

		/// <summary>
		/// Raises the lines deleted event.
		/// </summary>
		/// <param name="lineIndex">
		/// Index of the line.
		/// </param>
		/// <param name="count">
		/// The count.
		/// </param>
		private void RaiseLinesDeleted(
			LineIndex lineIndex,
			int count)
		{
			// Make sure we have listeners for this event.
			EventHandler<LineIndexLinesDeletedEventArgs> listeners =
				LinesDeleted;

			if (listeners == null)
			{
				return;
			}

			// Construct the event arguments and raise the event.
			var args = new LineIndexLinesDeletedEventArgs(
				lineIndex,
				count);

			listeners(
				this,
				args);
		}

		#endregion
	}
}
