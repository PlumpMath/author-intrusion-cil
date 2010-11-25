#region Namespaces

using System;
using System.Collections.Generic;
using System.Reflection;

using AuthorIntrusion.English.Attributes;
using AuthorIntrusion.English.Enumerations;

using MfGames.Extensions.Reflection;

#endregion

namespace AuthorIntrusion.English
{
	/// <summary>
	/// Utility class that identifies parts of speech from the
	/// enumerations.
	/// </summary>
	public static class PartsOfSpeechUtility
	{
		#region Constructors

		/// <summary>
		/// Initializes the <see cref="PartsOfSpeechUtility"/> class.
		/// </summary>
		static PartsOfSpeechUtility()
		{
			// Cache the parts of speech codes.
			TreebankLookup = new Dictionary<string, PartOfSpeech>();

			foreach (PartOfSpeech part in Enum.GetValues(typeof(PartOfSpeech)))
			{
				// Get the treebank code for this field.
				FieldInfo fieldInfo = part.GetType().GetField(part.ToString());
				var attribute = fieldInfo.GetCustomAttribute<TreebankCodeAttribute>();

				if (attribute == null)
				{
					continue;
				}

				// We have an attribute, so add it to the cache.
				TreebankLookup.Add(attribute.TreebankCode, part);
			}

			// Cache the phrase type codes.
			PhraseTypeLookup = new Dictionary<string, PhraseType>();

			foreach (PhraseType phraseType in Enum.GetValues(typeof(PhraseType)))
			{
				// Get the treebank code for this field.
				FieldInfo fieldInfo = phraseType.GetType().GetField(phraseType.ToString());
				var attribute = fieldInfo.GetCustomAttribute<TreebankCodeAttribute>();

				if (attribute == null)
				{
					continue;
				}

				// We have an attribute, so add it to the cache.
				PhraseTypeLookup.Add(attribute.TreebankCode, phraseType);
			}
		}

		#endregion

		#region Caching

		private static readonly Dictionary<string, PhraseType> PhraseTypeLookup;
		private static readonly Dictionary<string, PartOfSpeech> TreebankLookup;

		/// <summary>
		/// Gets the enumeration for the given treebank code.
		/// </summary>
		/// <param name="treebankCode">The treebank code.</param>
		/// <returns></returns>
		public static PartOfSpeech GetPartOfSpeech(string treebankCode)
		{
			return TreebankLookup[treebankCode];
		}

		/// <summary>
		/// Gets the enumeration for the given treebank code.
		/// </summary>
		/// <param name="treebankCode">The treebank code.</param>
		/// <returns></returns>
		public static PhraseType GetPhraseType(string treebankCode)
		{
			return PhraseTypeLookup[treebankCode];
		}

		/// <summary>
		/// Determines whether the given treebank code represents a part of
		/// speech.
		/// </summary>
		/// <param name="treebankCode">The treebank code.</param>
		/// <returns>
		/// 	<c>true</c> if [is part of speech] [the specified treebank code]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsPartOfSpeech(string treebankCode)
		{
			return TreebankLookup.ContainsKey(treebankCode);
		}

		/// <summary>
		/// Determines whether the given treebank code is a valid phrase.
		/// </summary>
		/// <param name="treebankCode">The treebank code.</param>
		/// <returns>
		/// 	<c>true</c> if [is phrase type] [the specified treebank code]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsPhraseType(string treebankCode)
		{
			return PhraseTypeLookup.ContainsKey(treebankCode);
		}

		#endregion
	}
}