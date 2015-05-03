// <copyright file="SingletonManager.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Collections.Generic;
using System.Diagnostics.Contracts;

using AuthorIntrusion.Css;
using AuthorIntrusion.Metadata;

namespace AuthorIntrusion
{
	/// <summary>
	/// A basic implementation of a singleton manager which 
	/// </summary>
	public class SingletonManager
	{
		#region Fields

		/// <summary>
		/// Contains the singleton instances of CSS class names for the project.
		/// </summary>
		private readonly Dictionary<string, CssClassKey> cssClassKeys;

		/// <summary>
		/// Contains the singleton instances of metadata keys for the project.
		/// </summary>
		private readonly Dictionary<string, MetadataKey> metadataKeys;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="SingletonManager"/> class.
		/// </summary>
		public SingletonManager()
		{
			cssClassKeys = new Dictionary<string, CssClassKey>();
			metadataKeys = new Dictionary<string, MetadataKey>();
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Gets a singleton class identifier.
		/// </summary>
		/// <param name="className">
		/// Name of the class.
		/// </param>
		/// <returns>
		/// A <c>CssClassKey</c> that represents that class.
		/// </returns>
		public CssClassKey GetCssClassKey(string className)
		{
			// Establish our contracts.
			Contract.Requires(className != null);
			Contract.Requires(className.Length > 0);

			// All the class names are singletones.
			className = string.Intern(className);

			// Look for the key.
			CssClassKey results;

			if (cssClassKeys.TryGetValue(
				className,
				out results))
			{
				return results;
			}

			// We haven't registered this key yet, so register and return it.
			results = new CssClassKey(className);

			cssClassKeys[className] = results;

			return results;
		}

		/// <summary>
		/// Gets a singleton metadata key for a given name.
		/// </summary>
		/// <param name="keyName">
		/// The name of the metadata key to retrieve.
		/// </param>
		/// <returns>
		/// A singleton MetadataKey that represents the given name.
		/// </returns>
		public MetadataKey GetMetadataKey(string keyName)
		{
			// Establish our contracts.
			Contract.Requires(keyName != null);
			Contract.Requires(keyName.Length > 0);

			// All the class names are singletones.
			keyName = string.Intern(keyName);

			// Look for the key.
			MetadataKey results;

			if (metadataKeys.TryGetValue(
				keyName,
				out results))
			{
				return results;
			}

			// We haven't registered this key yet, so register and return it.
			results = new MetadataKey(keyName);

			metadataKeys[keyName] = results;

			return results;
		}

		#endregion
	}
}
