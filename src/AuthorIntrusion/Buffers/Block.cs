// <copyright file="Block.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System;

using Humanizer;

namespace AuthorIntrusion.Buffers
{
	/// <summary>
	/// Represents a single line or link inside the project.
	/// </summary>
	public class Block
	{
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Block"/> class.
		/// </summary>
		public Block()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Block"/> class.
		/// </summary>
		/// <param name="text">
		/// The text.
		/// </param>
		public Block(string text)
			: this()
		{
			Text = text;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the type of the block.
		/// </summary>
		/// <value>
		/// The type of the block.
		/// </value>
		public BlockType BlockType { get; set; }

		/// <summary>
		/// Gets or sets the linked region.
		/// </summary>
		/// <value>
		/// The linked region.
		/// </value>
		public Region LinkedRegion { get; set; }

		/// <summary>
		/// Gets or sets the line text.
		/// </summary>
		/// <value>
		/// The text.
		/// </value>
		public string Text { get; set; }

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			string text;

			switch (BlockType)
			{
				case BlockType.Text:
					text = string.Format(
						"\"{0}\"",
						Text.Truncate(23));
					break;
				case BlockType.Region:
					text = LinkedRegion.Slug;
					break;
				default:
					throw new Exception(
						"Unknown block type: " + BlockType + ".");
			}

			return string.Format(
				"Block({0}, {1})",
				BlockType,
				text);
		}

		#endregion
	}
}
