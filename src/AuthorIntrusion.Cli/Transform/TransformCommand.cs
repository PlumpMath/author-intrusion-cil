// <copyright file="TransformCommand.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System;
using System.Collections.Generic;
using System.Reflection;

using AuthorIntrusion.IO;

namespace AuthorIntrusion.Cli.Transform
{
	/// <summary>
	/// Implements the transform command for the command line.
	/// </summary>
	public class TransformCommand
	{
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="TransformCommand"/> class.
		/// </summary>
		/// <param name="persistenceFactoryManager">
		/// The persistence factory manager.
		/// </param>
		public TransformCommand(
			PersistenceFactoryManager persistenceFactoryManager)
		{
			PersistenceFactoryManager = persistenceFactoryManager;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the persistence factory manager.
		/// </summary>
		/// <value>
		/// The persistence factory manager.
		/// </value>
		public PersistenceFactoryManager PersistenceFactoryManager { get; private set;
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Executes the command.
		/// </summary>
		/// <param name="options">
		/// The options.
		/// </param>
		public void Run(TransformOptions options)
		{
			// Get the persistence objects for both input and output.
			var project = new Project();
			IPersistence inputPersistence =
				PersistenceFactoryManager.CreatePersistence(
					options.InputUri);
			IPersistence outputPersistence =
				PersistenceFactoryManager.CreatePersistence(
					options.OutputUri);

			// Figure out the formats for both input and output.
			IFileBufferFormat inputFormat = inputPersistence.ProjectFormat;
			IFileBufferFormat outputFormat = outputPersistence.ProjectFormat;

			SetFormatSettings(
				inputFormat,
				null,
				null);
			SetFormatSettings(
				outputFormat,
				null,
				options.OutputOptions);

			// Load the project into memory.
			var inputContext = new BufferLoadContext(
				project,
				inputPersistence);
			inputFormat.LoadProject(inputContext);

			// Write out the project from memory.
			var outputContext = new BufferStoreContext(
				project,
				outputPersistence);
			outputFormat.StoreProject(outputContext);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Sets the format settings from options given on the command line.
		/// </summary>
		/// <param name="format">
		/// The format to update.
		/// </param>
		/// <param name="profile">
		/// The name of the profile to load.
		/// </param>
		/// <param name="options">
		/// The list of additional options to set.
		/// </param>
		private void SetFormatSettings(
			IFileBufferFormat format,
			string profile,
			IEnumerable<string> options)
		{
			// If we have a profile, then use that first.
			if (!string.IsNullOrWhiteSpace(profile))
			{
				format.LoadProfile(profile);
			}

			// If we have additional settings, we need to pull out the settings object
			// and set each one.
			if (options != null)
			{
				// Pull out the settings object.
				IBufferFormatSettings settings = format.Settings;

				if (settings == null)
				{
					return;
				}

				// Loop through the settings and set each one. The format of the
				// options line is "key=value".
				foreach (string option in options)
				{
					// Split the options line.
					string[] parts = option.Split(
						new[] { '=' },
						2);
					string name = parts[0];
					string value = parts[1];

					// Get the public property on the setting.
					PropertyInfo property = settings.GetType()
						.GetProperty(name);
					object propertyValue = Convert.ChangeType(
						value,
						property.PropertyType);
					property.SetValue(
						settings,
						propertyValue);
				}
			}
		}

		#endregion
	}
}
