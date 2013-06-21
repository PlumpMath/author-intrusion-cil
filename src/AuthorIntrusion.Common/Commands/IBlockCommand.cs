// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using MfGames.Commands;

namespace AuthorIntrusion.Common.Commands
{
	/// <summary>
	/// Defines the signature for an operation that changes the internal block
	/// structure for the project. Examples of these commands would be inserting or
	/// deleting text, changing block types, or the individual results of auto-correct.
	/// </summary>
	public interface IBlockCommand:ICommand<BlockCommandContext>
	{
	}
}
