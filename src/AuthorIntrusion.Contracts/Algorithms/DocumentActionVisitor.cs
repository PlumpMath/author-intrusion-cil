using System;

using AuthorIntrusion.Contracts.Structures;

namespace AuthorIntrusion.Contracts
{
	public class DocumentActionVisitor : DocumentVisitor
	{
		#region Constructors

		public DocumentActionVisitor()
		{
		}

		#endregion

		#region Actions

		public Func<Document, bool> BeginDocument { get; set; }

		public Action<Document> EndDocument { get; set; }

		public Func<Structure, bool> BeginStructure { get; set; }

		public Action<Structure> EndStructure { get; set; }

		public Func<Section, bool> BeginSection { get; set; }

		public Action<Section> EndSection { get; set; }

		public Func<Paragraph, bool> BeginParagraph { get; set; }

		public Action<Paragraph> EndParagraph { get; set; }

		#endregion

		#region Events

		public override bool OnBeginParagraph (Paragraph paragraph)
		{
			if (BeginParagraph != null)
			{
				return BeginParagraph(paragraph);
			}

			return base.OnBeginParagraph(paragraph);
		}

		public override bool OnBeginDocument (Document document)
		{
			if (BeginDocument != null)
			{
				return BeginDocument(document);
			}

			return base.OnBeginDocument(document);
		}

		public override void OnEndDocument (Document document)
		{
			if (EndDocument != null)
			{
				EndDocument(document);
			}
		}

		public override bool OnBeginStructure (Structure structure)
		{
			if (BeginStructure != null)
			{
				return BeginStructure(structure);
			}

			return base.OnBeginStructure(structure);
		}

		public override void OnEndStructure (Structure structure)
		{
			if (EndStructure != null)
			{
				EndStructure(structure);
			}
		}

		public override bool OnBeginSection(Section section)
		{
			if (BeginSection != null)
			{
				return BeginSection(section);
			}

			return base.OnBeginSection(section);
		}

		public override void OnEndSection(Section section)
		{
			if (EndSection != null)
			{
				EndSection(section);
			}
		}

		#endregion
	}
}

