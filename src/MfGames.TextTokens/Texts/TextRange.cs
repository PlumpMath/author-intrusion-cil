﻿// <copyright file="TextRange.cs" company="Moonfire Games">
//     Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// MIT Licensed (http://opensource.org/licenses/MIT)
namespace MfGames.TextTokens.Texts
{
    /// <summary>
    /// Represents a range between two text locations.
    /// </summary>
    public struct TextRange
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TextRange"/> struct.
        /// </summary>
        /// <param name="anchor">
        /// The anchor.
        /// </param>
        /// <param name="cursor">
        /// The cursor.
        /// </param>
        public TextRange(
            TextLocation anchor, 
            TextLocation cursor)
            : this()
        {
            this.Anchor = anchor;
            this.Cursor = cursor;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the anchor location.
        /// </summary>
        /// <value>
        /// The anchor.
        /// </value>
        public TextLocation Anchor { get; private set; }

        /// <summary>
        /// Gets the cursor location.
        /// </summary>
        /// <value>
        /// The cursor.
        /// </value>
        public TextLocation Cursor { get; private set; }

        #endregion
    }
}