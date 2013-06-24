// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common;
using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Commands;
using AuthorIntrusion.Common.Plugins;
using MfGames.Commands;

namespace AuthorIntrusion.Plugins.ImmediateBlockTypes
{
	public class ImmediateBlockTypesProjectPlugin: IImmediateEditorProjectPlugin
	{
		#region Properties

		public string Key
		{
			get { return "Immediate Block Types"; }
		}

		public Project Project { get; set; }

		public ImmediateBlockTypesSettings Settings
		{
			get
			{
				var settings =
					Project.Settings.Get<ImmediateBlockTypesSettings>(
						ImmediateBlockTypesSettings.SettingsPath);
				return settings;
			}
		}

		#endregion

		public ImmediateBlockTypesProjectPlugin(Project project)
		{
			Project = project;
		}

		#region Methods

		public void ProcessImmediateEdits(
			BlockCommandContext context,
			Block block,
			int textIndex)
		{
			// Get the plugin settings from the project.
			ImmediateBlockTypesSettings settings = Settings;

			// Grab the substring from the beginning to the index and compare that
			// in the dictionary.
			string text = block.Text.Substring(0, textIndex);

			if (!settings.Replacements.Contains(text))
			{
				// We want to fail as fast as possible.
				return;
			}

			// If the block type is already set to the same name, skip it.
			string blockTypeName = settings.Replacements[text];
			BlockType blockType = Project.BlockTypes[blockTypeName];

			if (block.BlockType == blockType)
			{
				return;
			}

			// Perform the substitution with a replace operation and a block change
			// operation.
			var replaceCommand =
				new ReplaceTextCommand(
					new BlockPosition(block.BlockKey, 0), textIndex, string.Empty);
			var changeCommand = new ChangeBlockTypeCommand(block.BlockKey, blockType);

			// Create a composite command that binds everything together.
			var compositeCommand = new CompositeCommand<BlockCommandContext>(true, false);

			compositeCommand.Commands.Add(replaceCommand);
			compositeCommand.Commands.Add(changeCommand);

			// Add the command to the deferred execution so the command could
			// be properly handled via the undo/redo management.
			block.Project.Commands.DeferDo(compositeCommand);
		}

		#endregion
	}
}
