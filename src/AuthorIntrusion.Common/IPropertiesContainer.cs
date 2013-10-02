// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;

namespace AuthorIntrusion.Common
{
	public interface IPropertiesContainer
	{
		#region Properties

		/// <summary>
		/// Gets the properties associated with the block.
		/// </summary>
		PropertiesDictionary Properties { get; }

		#endregion
	}
}
