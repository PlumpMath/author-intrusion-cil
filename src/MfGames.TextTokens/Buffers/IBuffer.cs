// <copyright file="IBuffer.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System;
using System.Collections.Generic;

using MfGames.TextTokens.Commands;
using MfGames.TextTokens.Events;
using MfGames.TextTokens.Lines;
using MfGames.TextTokens.Texts;
using MfGames.TextTokens.Tokens;

namespace MfGames.TextTokens.Buffers
{
	/// <summary>
	/// Defines the public signature of a buffer which consists of zero or more lines which
	/// in turn have zero or more tokens. The buffer is the equivalent of a document in most
	/// text editors but can represent a virtualized document such as seamlessly combining
	/// multiple text documents into one or pulling data from another source.
	/// </summary>
	public interface IBuffer
	{
		#region Public Events

		/// <summary>
		/// Occurs when lines are deleted from the buffer.
		/// </summary>
		event EventHandler<LineIndexLinesDeletedEventArgs> LinesDeleted;

		/// <summary>
		/// Occurs when lines are inserted into the buffer.
		/// </summary>
		event EventHandler<LineIndexLinesInsertedEventArgs> LinesInserted;

		/// <summary>
		/// Occurs when the selection should be replaced because of a command.
		/// </summary>
		event EventHandler<ReplaceSelectionEventArgs> ReplaceSelection;

		/// <summary>
		/// Occurs when the selection should be restored, typically after
		/// undoing a command.
		/// </summary>
		event EventHandler<RestoreSelectionEventArgs> RestoreSelection;

		/// <summary>
		/// Occurs when a token is replaced by zero or more tokens.
		/// </summary>
		event EventHandler<LineIndexTokenIndexTokensReplacedEventArgs> TokensReplaced;

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the lines contained within the buffer.
		/// </summary>
		/// <value>
		/// The lines.
		/// </value>
		IReadOnlyList<ILine> Lines { get; }

		#endregion

		#region Public Methods and Operators

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
		IToken CreateToken(
			IToken oldToken,
			string newText);

		/// <summary>
		/// Creates a new token.
		/// </summary>
		/// <param name="newText">
		/// The new text.
		/// </param>
		/// <returns>
		/// A constructed token.
		/// </returns>
		IToken CreateToken(string newText);

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
		IEnumerable<ILine> DeleteLines(
			LineIndex lineIndex,
			int count);

		/// <summary>
		/// Executes a command on the buffer, running through each operation in turn.
		/// </summary>
		/// <param name="command">
		/// The command.
		/// </param>
		void Do(BufferCommand command);

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
		IToken GetToken(
			LineIndex lineIndex,
			TokenIndex tokenIndex);

		/// <summary>
		/// Retrieves operations to normalize the changed lines including rebuilding
		/// tokens.
		/// </summary>
		/// <returns>
		/// An enumeration of buffer operations to normalize the changed lines.
		/// </returns>
		IEnumerable<IBufferOperation> GetUpdateOperations();

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
		IEnumerable<ILine> InsertLines(
			LineIndex afterLineIndex,
			int count);

		/// <summary>
		/// Inserts lines into the buffer.
		/// </summary>
		/// <param name="afterLineIndex">
		/// Index of the after line.
		/// </param>
		/// <param name="lines">
		/// The lines.
		/// </param>
		void InsertLines(
			LineIndex afterLineIndex,
			IEnumerable<ILine> lines);

		/// <summary>
		/// Raises the ReplaceSelection event with the given arguments.
		/// </summary>
		/// <param name="newTextRange">
		/// The new text range.
		/// </param>
		/// <returns>
		/// A dictionary of the old selection items.
		/// </returns>
		Dictionary<object, TextRange> RaiseReplaceSelection(
			TextRange newTextRange);

		/// <summary>
		/// Raises the restore selection event.
		/// </summary>
		/// <param name="oldTextRanges">
		/// The old text ranges.
		/// </param>
		void RaiseRestoreSelection(Dictionary<object, TextRange> oldTextRanges);

		/// <summary>
		/// Re-executes the last undone command (reverses the undo) or do nothing if
		/// there are no re-doable commands.
		/// </summary>
		void Redo();

		/// <summary>
		/// Replaces the given token with a set of zero or more next tokens.
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
		IEnumerable<IToken> ReplaceTokens(
			LineIndex lineIndex,
			TokenIndex tokenIndex,
			int count,
			IEnumerable<IToken> newTokens);

		/// <summary>
		/// Executes the reverse operation of the last command or do nothing if there
		/// are no undoable commands.
		/// </summary>
		void Undo();

		#endregion
	}
}
