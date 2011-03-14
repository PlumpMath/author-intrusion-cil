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

		public void Visit(Document document)
		{
			// Check for null values.
			if (document == null)
			{
				throw new ArgumentNullException("document");
			}

			// Start by calling the BeingDocument call.
			OnBeginDocument(document);

			// Loop through the structure of the document.
			Visit(document.Structure);

			// Finish up with the end document.
			OnEndDocument(document);
		}

		protected void Visit(Structure structure)
		{
			OnBeginStructure(structure);

			if (structure is Section)
			{
				Section section = (Section) structure;

				foreach (var childStructure in section.Structures)
				{
					Visit(childStructure);
				}
			}

			OnEndStructure(structure);
		}

		#endregion

		#region Events

		public virtual void OnBeginDocument(Document document)
		{
		}

		public virtual void OnEndDocument(Document document)
		{
		}

		public virtual void OnBeginStructure(Structure structure)
		{
		}

		public virtual void OnEndStructure(Structure structure)
		{
		}
		
		#endregion
	}
}