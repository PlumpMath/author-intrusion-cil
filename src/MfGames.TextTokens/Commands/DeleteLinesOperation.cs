﻿// <copyright file="DeleteLinesOperation.cs" company="Moonfire Games">
//     Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// MIT Licensed (http://opensource.org/licenses/MIT)
namespace MfGames.TextTokens.Commands
{
    using System.Collections.Generic;
    using System.Linq;

    using MfGames.TextTokens.Buffers;
    using MfGames.TextTokens.Lines;
    using MfGames.TextTokens.Tokens;

    /// <summary>
    /// Defines an operation that deletes one or more lines.
    /// </summary>
    public class DeleteLinesOperation : IBufferOperation
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteLinesOperation"/> class.
        /// </summary>
        /// <param name="lineIndex">
        /// Index of the line.
        /// </param>
        /// <param name="count">
        /// The count.
        /// </param>
        public DeleteLinesOperation(LineIndex lineIndex, int count)
        {
            this.LineIndex = lineIndex;
            this.Count = count;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the number of lines to insert.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count { get; private set; }

        /// <summary>
        /// Gets the index of the line to insert after.
        /// </summary>
        /// <value>
        /// The index of the line.
        /// </value>
        public LineIndex LineIndex { get; private set; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the tokens from the deleted lines.
        /// </summary>
        /// <value>
        /// The deleted tokens.
        /// </value>
        protected List<List<IToken>> DeletedTokens { get; private set; }

        /// <summary>
        /// Gets or sets the delete lines.
        /// </summary>
        /// <value>
        /// The deleted lines.
        /// </value>
        private List<ILine> DeletedLines { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Performs the operation on the buffer.
        /// </summary>
        /// <param name="buffer">
        /// The buffer to execute the operations on.
        /// </param>
        public void Do(IBuffer buffer)
        {
            // Actually delete the lines from the buffer.
            IEnumerable<ILine> lines = buffer.DeleteLines(
                this.LineIndex, this.Count);
            this.DeletedLines = lines.ToList();
        }

        /// <summary>
        /// Reverses the operation on the given buffer.
        /// </summary>
        /// <param name="buffer">
        /// The buffer to manipulate.
        /// </param>
        public void Undo(IBuffer buffer)
        {
            // Get the new lines from the buffer.
            buffer.InsertLines(this.LineIndex, this.DeletedLines);
        }

        #endregion
    }
}