// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using AuthorIntrusion.Common;
using MfGames.HierarchicalPaths;

namespace AuthorIntrusion.Plugins.Spelling.LocalWords
{
	/// <summary>
	/// Contains the serialiable settings for the Local Words plugin.
	/// </summary>
	[XmlRoot("local-words-settings", Namespace = XmlConstants.ProjectNamespace)]
	public class LocalWordsSettings: IXmlSerializable
	{
		#region Properties

		public HashSet<string> CaseInsensitiveDictionary { get; private set; }
		public HashSet<string> CaseSensitiveDictionary { get; private set; }

		#endregion

		#region Methods

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
			while (reader.Read())
			{
				// If we aren't in our namespace, we don't need to bother.
				if (reader.NamespaceURI != XmlConstants.ProjectNamespace)
				{
					continue;
				}

				// If we got to the end of the node, then stop reading.
				if (reader.LocalName == elementName)
				{
					return;
				}

				// Look for a key, if we have it, set that value.
				if (reader.NodeType == XmlNodeType.Element)
				{
					switch (reader.LocalName)
					{
						case "sensitive":
							string sensitive = reader.ReadString();
							CaseSensitiveDictionary.Add(sensitive);
							break;

						case "insensitive":
							string insensitive = reader.ReadString();
							CaseInsensitiveDictionary.Add(insensitive);
							break;
					}
				}
			}
		}

		public void WriteXml(XmlWriter writer)
		{
			// Write out a version field.
			writer.WriteElementString("version", "1");

			// Sort the list of words.
			var sortedInsensitiveWords = new List<string>();
			sortedInsensitiveWords.AddRange(CaseInsensitiveDictionary);
			sortedInsensitiveWords.Sort();

			var sortedSensitiveWords = new List<string>();
			sortedSensitiveWords.AddRange(CaseSensitiveDictionary);
			sortedSensitiveWords.Sort();

			// Write out the records.
			foreach (string word in sortedInsensitiveWords)
			{
				writer.WriteElementString("insensitive", word);
			}

			foreach (string word in sortedSensitiveWords)
			{
				writer.WriteElementString("sensitive", word);
			}
		}

		#endregion

		#region Constructors

		static LocalWordsSettings()
		{
			SettingsPath = new HierarchicalPath("/Plugins/Spelling/Local Words");
		}

		public LocalWordsSettings()
		{
			CaseInsensitiveDictionary = new HashSet<string>();
			CaseSensitiveDictionary = new HashSet<string>();
		}

		#endregion

		#region Fields

		public static readonly HierarchicalPath SettingsPath;

		#endregion
	}
}
