// <copyright file="SystemXmlXmlWriterExtensions.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Xml;

using AuthorIntrusion.Metadata;

namespace AuthorIntrusion.Extensions.System.Xml
{
	/// <summary>
	/// Provides extensions for System.Xml.XmlWriterExtensions.
	/// </summary>
	public static class SystemXmlXmlWriterExtensions
	{
		#region Public Methods and Operators

		/// <summary>
		/// Writes the metadata value string to the given writer.
		/// </summary>
		/// <param name="writer">
		/// The writer.
		/// </param>
		/// <param name="elementName">
		/// Name of the element.
		/// </param>
		/// <param name="project">
		/// The project.
		/// </param>
		/// <param name="metadataKey">
		/// The metadata key.
		/// </param>
		public static void WriteElementString(
			this XmlWriter writer,
			string elementName,
			Project project,
			string metadataKey)
		{
			WriteElementString(
				writer,
				elementName,
				project,
				project.Metadata,
				metadataKey);
		}

		/// <summary>
		/// Writes the metadata value string to the given writer.
		/// </summary>
		/// <param name="writer">
		/// The writer.
		/// </param>
		/// <param name="elementName">
		/// Name of the element.
		/// </param>
		/// <param name="project">
		/// The project.
		/// </param>
		/// <param name="metadata">
		/// The metadata.
		/// </param>
		/// <param name="metadataKey">
		/// The metadata key.
		/// </param>
		public static void WriteElementString(
			this XmlWriter writer,
			string elementName,
			Project project,
			MetadataDictionary metadata,
			string metadataKey)
		{
			MetadataKey key = project.MetadataManager[metadataKey];

			WriteElementString(
				writer,
				elementName,
				project,
				metadata,
				key);
		}

		/// <summary>
		/// Writes the metadata value string to the given writer.
		/// </summary>
		/// <param name="writer">
		/// The writer.
		/// </param>
		/// <param name="elementName">
		/// Name of the element.
		/// </param>
		/// <param name="project">
		/// The project.
		/// </param>
		/// <param name="metadata">
		/// The metadata.
		/// </param>
		/// <param name="metadataKey">
		/// The metadata key.
		/// </param>
		public static void WriteElementString(
			this XmlWriter writer,
			string elementName,
			Project project,
			MetadataDictionary metadata,
			MetadataKey metadataKey)
		{
			string value = metadata.GetOrCreate(metadataKey)
				.Value;

			writer.WriteElementString(
				elementName,
				value);
		}

		#endregion
	}
}
