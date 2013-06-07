// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using MfGames.Extensions.System.Xml;

namespace AuthorIntrusion.Common.Persistence
{
	/// <summary>
	/// Defines the serializable settings used to control the filesystem persistence
	/// project plugin.
	/// 
	/// All of the string variables in here can have project macros in them, which are
	/// expanded at the point of saving.
	/// </summary>
	[XmlRoot("filesystem-persistence-settings",
		Namespace = XmlConstants.ProjectNamespace)]
	public class FilesystemPersistenceSettings: IXmlSerializable
	{
		#region Properties

		/// <summary>
		/// Gets or sets the external settings directory. This is the directory
		/// where non-project settings are stored. Macros inside the variable will
		/// be expanded at the point of saving. If this is blank or null, then the
		/// project settings will be stored in the project file.
		/// </summary>
		public string ExternalSettingsDirectory { get; set; }

		/// <summary>
		/// Gets or sets the external blocks directory. This is the directory where
		/// external blocks (e.g., chapters that are extracted from the main file) are
		/// stored.
		/// 
		/// If this is null or empty, then external files are not allowed. This will
		/// have macros expanded.
		/// </summary>
		public string ProjectBlocksDirectory { get; set; }

		/// <summary>
		/// Gets or sets the project filename. This will have the macros expanded.
		/// If it is blank or null, then it is considered an error condition.
		/// </summary>
		public string ProjectFilename { get; set; }

		/// <summary>
		/// Gets or sets the project settings directory. This is expanded with
		/// macros to determine where the project settings will be stored. If
		/// this is blank, then the project settings will be stored in the project
		/// file.
		/// </summary>
		public string ProjectSettingsDirectory { get; set; }

		#endregion

		#region Methods

		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			throw new NotImplementedException();
		}

		public void WriteXml(XmlWriter writer)
		{
			// Always start with the version field, because that will control how
			// we read it back in.
			writer.WriteElementString("version", "1");

			// Write out the various properties.
			writer.WriteNonNullElementString("project-filename", ProjectFilename);
			writer.WriteNonNullElementString(
				"project-blocks-directory", ProjectBlocksDirectory);
			writer.WriteNonNullElementString(
				"project-settings-directory", ProjectSettingsDirectory);
			writer.WriteNonNullElementString(
				"external-settings-directory", ExternalSettingsDirectory);
		}

		#endregion
	}
}
