// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using AuthorIntrusion.Common;
using MfGames.HierarchicalPaths;

namespace AuthorIntrusion.Plugins.ImmediateCorrection
{
	/// <summary>
	/// A configuration settings for an immediate correction controller.
	/// </summary>
	[XmlRoot("immediate-correction-settings",
		Namespace = XmlConstants.ProjectNamespace)]
	public class ImmediateCorrectionSettings: IXmlSerializable
	{
		#region Properties

		public static HierarchicalPath SettingsPath { get; private set; }
		public List<RegisteredSubstitution> Substitutions { get; set; }

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
				if (reader.NodeType == XmlNodeType.Element
					&& reader.LocalName == "substitution")
				{
					// Pull out the settings.
					string search = reader["search"];
					string replacement = reader["replace"];
					var options =
						(SubstitutionOptions)
							Enum.Parse(typeof (SubstitutionOptions), reader["options"]);

					// Add in the substitution.
					var substitution = new RegisteredSubstitution(search, replacement, options);
					Substitutions.Add(substitution);
				}
			}
		}

		public void WriteXml(XmlWriter writer)
		{
			// Write out a version field.
			writer.WriteElementString("version", "1");

			// Write out all the substitutions.
			foreach (RegisteredSubstitution substitution in Substitutions)
			{
				writer.WriteStartElement("substitution", XmlConstants.ProjectNamespace);
				writer.WriteAttributeString("search", substitution.Search);
				writer.WriteAttributeString("replace", substitution.Replacement);
				writer.WriteAttributeString("options", substitution.Options.ToString());
				writer.WriteEndElement();
			}
		}

		#endregion

		#region Constructors

		static ImmediateCorrectionSettings()
		{
			SettingsPath = new HierarchicalPath("/Plugins/Immediate Correction");
		}

		public ImmediateCorrectionSettings()
		{
			Substitutions = new List<RegisteredSubstitution>();
		}

		#endregion
	}
}
