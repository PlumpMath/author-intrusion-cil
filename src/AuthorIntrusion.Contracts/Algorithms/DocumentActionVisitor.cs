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

		public Action<Document> BeginDocument { get; set; }

		public Action<Document> EndDocument { get; set; }

		public Action<Structure> BeginStructure { get; set; }

		public Action<Structure> EndStructure { get; set; }

		#endregion

		#region Events

		public override void OnBeginDocument (Document document)
		{
			if (BeginDocument != null)
			{
				BeginDocument(document);
			}
		}

		public override void OnEndDocument (Document document)
		{
			if (EndDocument != null)
			{
				EndDocument(document);
			}
		}

		public override void OnBeginStructure (Structure structure)
		{
			if (BeginStructure != null)
			{
				BeginStructure(structure);
			}
		}

		public override void OnEndStructure (Structure structure)
		{
			if (EndStructure != null)
			{
				EndStructure(structure);
			}
		}

		#endregion
	}
}

