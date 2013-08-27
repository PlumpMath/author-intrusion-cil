using AuthorIntrusion.Common.Blocks;

namespace AuthorIntrusion.Common
{
	public interface IPropertiesContainer
	{
		/// <summary>
		/// Gets the properties associated with the block.
		/// </summary>
		PropertiesDictionary Properties { get; }
	}
}