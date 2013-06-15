// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using AuthorIntrusion.Common;
using C5;
using MfGames.HierarchicalPaths;

namespace AuthorIntrusion.Plugins.ImmediateBlockTypes
{
	/// <summary>
	/// The settings for the immediate block type settings. This controls which
	/// text prefixes will alter the block types.
	/// </summary>
	[XmlRoot("immediate-block-types", Namespace = XmlConstants.ProjectNamespace)]
	public class ImmediateBlockTypesSettings:IXmlSerializable
	{
		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			// We are already at the starting point of this element, so read until the
			// end.
			string elementName = reader.LocalName;

			// Read until we get to the end element.
			while(reader.Read())
			{
				// If we aren't in our namespace, we don't need to bother.
				if(reader.NamespaceURI != XmlConstants.ProjectNamespace)
				{
					continue;
				}

				// If we got to the end of the node, then stop reading.
				if(reader.LocalName == elementName)
				{
					return;
				}

				// Look for a key, if we have it, set that value.
				if(reader.NodeType == XmlNodeType.Element && reader.LocalName=="replacement")
				{
					// Pull out the elements from the attribute string.
					string prefix = reader["prefix"];
					string blockTypeName = reader["block-type"];

					// Insert the replacement into the dictionary.
					Replacements[prefix] = blockTypeName;
				}
			}
		}

		public void WriteXml(XmlWriter writer)
		{
			// Write out a version field.
			writer.WriteElementString("version","1");

			// Sort the list of words.
			var prefixes = new ArrayList<string>();
			prefixes.AddAll(Replacements.Keys);
			prefixes.Sort();

			// Write out the records.
			foreach(string prefix in prefixes)
			{
				writer.WriteStartElement("replacement",XmlConstants.ProjectNamespace);
				writer.WriteAttributeString("prefix", prefix);
				writer.WriteAttributeString("block-type", Replacements[prefix]);
				writer.WriteEndElement();
			}
		}

		public IDictionary<string, string> Replacements { get; private set; }

		public ImmediateBlockTypesSettings()
		{
			Replacements = new HashDictionary<string, string>();
		}

		static ImmediateBlockTypesSettings()
		{
			SettingsPath = new HierarchicalPath("/Plugins/Immediate Block Types");
		}

		public static HierarchicalPath SettingsPath { get; private set; }
	}
}