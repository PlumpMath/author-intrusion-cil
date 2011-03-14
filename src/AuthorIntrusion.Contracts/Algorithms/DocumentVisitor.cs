using System;
using AuthorIntrusion.Contracts.Structures;
using AuthorIntrusion.Contracts.Contents;

namespace AuthorIntrusion.Contracts
{
	public class DocumentVisitor
	{
		#region Constructors

		public DocumentVisitor()
		{
		}

		#endregion

		#region Visiting

		/// <summary>
		/// Visits the various structural nodes of a document.
		/// </summary>
		public void Visit(Document document)
		{
			// Check for null values.
			if (document == null)
			{
				throw new ArgumentNullException("document");
			}

			// Start by calling the BeingDocument call.
			bool shouldRecurse = OnBeginDocument(document);

			// Loop through the structure of the document.
			if (shouldRecurse)
			{
				Visit(document.Structure);
			}

			// Finish up with the end document.
			OnEndDocument(document);
		}

		protected void Visit(Structure structure)
		{
			// We always call the start structure to determine if we
			// should recurse into the structure.
			bool shouldRecurse = OnBeginStructure(structure);

			// Determine the type of structure we should work with.
			if (structure is Section)
			{
				Section section = (Section) structure;

				Visit(section, shouldRecurse);
			}
			else if (structure is Paragraph)
			{
				Paragraph paragraph = (Paragraph) structure;

				Visit(paragraph, shouldRecurse);
			}

			OnEndStructure(structure);
		}

		private void Visit(Section section, bool shouldRecurse)
		{
			shouldRecurse |= OnBeginSection(section);

			if (shouldRecurse)
			{
				foreach (var structure in section.Structures)
				{
					Visit(structure);
				}
			}

			OnEndSection(section);
		}

		private void Visit(Paragraph paragraph, bool shouldRecurse)
		{
			OnBeginParagraph(paragraph);

			OnEndParagraph(paragraph);
		}

		#endregion

		#region Events

		public virtual bool OnBeginSection(Section section)
		{
			return true;
		}

		public virtual void OnEndSection(Section section)
		{
		}

		public virtual bool OnBeginParagraph(Paragraph paragraph)
		{
			return true;
		}

		public virtual void OnEndParagraph(Paragraph paragraph)
		{
		}

		public virtual bool OnBeginDocument(Document document)
		{
			return true;
		}

		public virtual void OnEndDocument(Document document)
		{
		}

		public virtual bool OnBeginStructure(Structure structure)
		{
			return true;
		}

		public virtual void OnEndStructure(Structure structure)
		{
		}
		
		#endregion
	}
}