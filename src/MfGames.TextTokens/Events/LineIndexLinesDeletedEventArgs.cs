﻿// <copyright file="LineIndexLinesDeletedEventArgs.cs" company="Moonfire Games">
//     Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// MIT Licensed (http://opensource.org/licenses/MIT)
namespace MfGames.TextTokens.Events
{
    using System.Diagnostics.Contracts;

    using MfGames.TextTokens.Lines;

    /// <summary>
    /// Represents an event where one or more lines are delete from a buffer
    /// starting with the given index.
    /// </summary>
    public class LineIndexLinesDeletedEventArgs : LineIndexEventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LineIndexLinesDeletedEventArgs"/> class.
        /// </summary>
        /// <param name="lineIndex">
        /// Index of the line.
        /// </param>
        /// <param name="count">
        /// The count.
        /// </param>
        public LineIndexLinesDeletedEventArgs(
            LineIndex lineIndex, 
            int count)
            : base(lineIndex)
        {
            Contract.Requires(count > 0);

            this.Count = count;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the number of lines to delete including the given index.
        /// </summary>
        public int Count { get; private set; }

        #endregion
    }
}