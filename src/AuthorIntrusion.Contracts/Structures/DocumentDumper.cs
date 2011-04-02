#region Copyright and License

// Copyright (c) 2011, Moonfire Games
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#endregion

#region Namespaces

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using AuthorIntrusion.Contracts.Algorithms;

#endregion

namespace AuthorIntrusion.Contracts.Structures
{
	/// <summary>
	/// A visitor class for going through a document and writing it out to
	/// a stream or a writer. This is used for debugging purposes.
	/// </summary>
	public class DocumentDumper : DocumentVisitor
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="DocumentDumper"/> class.
		/// </summary>
		/// <param name="document">The document.</param>
		/// <param name="writer">The writer.</param>
		public DocumentDumper(
			Document document,
			TextWriter writer)
		{
			this.document = document;
			this.writer = writer;
			prefix = String.Empty;
			structurePrefixes = new Dictionary<int, char>();
			structureSuffixes = new Dictionary<int, char>();
			showStructureIndex = true;
		}

		#endregion

		#region Static Calls

		/// <summary>
		/// Dumps the document to Console.Out.
		/// </summary>
		/// <param name="document">The document.</param>
		public static void DumpDocument(Document document)
		{
			DumpDocument(document, Console.Out);
		}

		/// <summary>
		/// Dumps the document to the given writer.
		/// </summary>
		/// <param name="document">The document.</param>
		/// <param name="writer">The writer.</param>
		public static void DumpDocument(
			Document document,
			TextWriter writer)
		{
			var dumper = new DocumentDumper(document, writer);
			dumper.Dump();
		}

		#endregion

		#region Properties

		private readonly Document document;
		private readonly Dictionary<int, char> structurePrefixes;
		private readonly Dictionary<int, char> structureSuffixes;
		private readonly TextWriter writer;
		private bool showStructureIndex;

		/// <summary>
		/// Gets or sets a value indicating whether structure index numbers
		/// should be shown.
		/// </summary>
		/// <value><c>true</c> if [show structure index]; otherwise, <c>false</c>.</value>
		public bool ShowStructureIndex
		{
			get { return showStructureIndex; }
			set { showStructureIndex = value; }
		}

		/// <summary>
		/// Contains the prefix strings for the structure indexes.
		/// </summary>
		/// <value>The structure prefixes.</value>
		public Dictionary<int, char> StructurePrefixes
		{
			get { return structurePrefixes; }
		}

		/// <summary>
		/// Gets the suffix strings for the structure indexes.
		/// </summary>
		/// <value>The structure suffixes.</value>
		public Dictionary<int, char> StructureSuffixes
		{
			get { return structureSuffixes; }
		}

		#endregion

		#region Dumping

		private int index;
		private string prefix;

		/// <summary>
		/// Dumps this instance to the given writer.
		/// </summary>
		public void Dump()
		{
			Visit(document);
		}

		/// <summary>
		/// Called when the visitor enters a structure. This is always called
		/// before OnBeginSection and OnBeginParagraph.
		/// </summary>
		/// <param name="structure">The structure.</param>
		/// <returns>
		/// True if the visitor should continue to recurse.
		/// </returns>
		protected override bool OnBeginStructure(Structure structure)
		{
			// Build up a formatted string.
			var builder = new StringBuilder();

			if (showStructureIndex)
			{
				builder.Append(index.ToString().PadLeft(3));
				builder.Append(" ");
			}

			builder.Append(prefix);

			if (structurePrefixes.ContainsKey(index))
			{
				builder.Append(structurePrefixes[index]);
			}
			else
			{
				builder.Append(" ");
			}

			builder.Append(structure);

			if (structureSuffixes.ContainsKey(index))
			{
				builder.Append(structureSuffixes[index]);
			}
			else
			{
				builder.Append(" ");
			}

			// Write out the constructed string.
			Console.WriteLine(builder.ToString());

			// Increment the prefix.
			prefix += " ";
			index++;

			// Recurse into the inner structures.
			return true;
		}

		/// <summary>
		/// Called when the visitor leaves a structure. This is called after
		/// OnEndSection and OnEndParagraph.
		/// </summary>
		/// <param name="structure">The structure.</param>
		protected override void OnEndStructure(Structure structure)
		{
			// Reduce the prefix by one step.
			prefix = prefix.Substring(1);

			// Call the base implementation.
			base.OnEndStructure(structure);
		}

		#endregion
	}
}