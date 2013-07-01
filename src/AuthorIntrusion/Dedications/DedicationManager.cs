// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using MfGames;

namespace AuthorIntrusion.Dedications
{
	/// <summary>
	/// A manager class that reads the dedications from an embedded resource
	/// and allows for retrieval of the versions.
	/// </summary>
	public class DedicationManager
	{
		#region Properties

		/// <summary>
		/// Contains the dedication for the current assembly version.
		/// </summary>
		public Dedication CurrentDedication
		{
			get
			{
				// Get our assembly version so we can look up the dedication.
				Version assemblyVersion = GetType().Assembly.GetName().Version;
				var extendedVersion = new ExtendedVersion(assemblyVersion.ToString());

				// Go through and find the appropriate version.
				IEnumerable<Dedication> search =
					Dedications.Where(d => d.Version == extendedVersion);

				foreach (Dedication dedication in search)
				{
					return dedication;
				}

				// If we get this far, then we couldn't find it.
				throw new InvalidOperationException(
					"Cannot find a dedication for version " + extendedVersion + ".");
			}
		}

		/// <summary>
		/// Contains the dedications for all the versions of Author Intrusion.
		/// </summary>
		public List<Dedication> Dedications { get; private set; }

		#endregion

		#region Constructors

		public DedicationManager()
		{
			// Get the embedded resource stream.
			Type type = GetType();
			Assembly assembly = type.Assembly;
			Stream stream = assembly.GetManifestResourceStream(type, "Dedication.xml");

			if (stream == null)
			{
				throw new InvalidOperationException(
					"Cannot load Dedication.xml from the assembly to load dedications.");
			}

			// Load the XML from the given stream.
			using (XmlReader reader = XmlReader.Create(stream))
			{
				// Loop through the reader.
				Dedications = new List<Dedication>();
				Dedication dedication = null;

				while (reader.Read())
				{
					// If we have a "dedication", we are either starting or
					// finishing one.
					if (reader.LocalName == "dedication")
					{
						if (reader.NodeType == XmlNodeType.Element)
						{
							dedication = new Dedication();
						}
						else
						{
							Dedications.Add(dedication);
							dedication = null;
						}
					}

					if (reader.NodeType != XmlNodeType.Element
						|| dedication == null)
					{
						continue;
					}

					// For the remaining tags, we just need to pull out the text.
					switch (reader.LocalName)
					{
						case "author":
							string author = reader.ReadString();
							dedication.Author = author;
							break;

						case "version":
							string version = reader.ReadString();
							var assemblyVersion = new ExtendedVersion(version);
							dedication.Version = assemblyVersion;
							break;

						case "dedicator":
							string dedicator = reader.ReadString();
							dedication.Dedicator = dedicator;
							break;

						case "p":
							string p = reader.ReadOuterXml();
							dedication.Html += p;
							break;
					}
				}

				// Finish up the stream.
				reader.Close();
			}
		}

		#endregion
	}
}
