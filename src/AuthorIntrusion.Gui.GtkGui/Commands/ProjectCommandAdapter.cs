// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using AuthorIntrusion.Common;
using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Blocks.Locking;
using AuthorIntrusion.Common.Commands;
using MfGames.Commands;
using MfGames.Commands.TextEditing;
using MfGames.GtkExt.TextEditor.Models;
using MfGames.GtkExt.TextEditor.Models.Buffers;

namespace AuthorIntrusion.Gui.GtkGui.Commands
{
	public abstract class ProjectCommandAdapter:ITextEditingCommand<OperationContext>
	{
		#region Properties

		public bool CanUndo
		{
			get { return true; }
		}

		public bool IsTransient
		{
			get { return false; }
		}

		public bool UpdateTextPosition { get; set; }
		public bool UpdateTextSelection { get; set; }

		public IUndoableCommand<BlockCommandContext> Command { get; set; }
		protected Project Project { get; private set; }

		#endregion

		#region Methods

		public virtual void Do(OperationContext context)
		{
			throw new InvalidOperationException();
		}

		public void Redo(OperationContext context)
		{
			throw new InvalidOperationException();
		}

		public virtual void Undo(OperationContext context)
		{
			throw new InvalidOperationException();
		}

		#endregion

		#region Constructors

		protected ProjectCommandAdapter(Project project)
		{
			Project = project;
		}

		#endregion
	}
}
