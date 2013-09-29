// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.Collections.Generic;
using AuthorIntrusion.Common;
using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Plugins;
using MfGames.HierarchicalPaths;

namespace AuthorIntrusion.Plugins.Counter
{
	/// <summary>
	/// Contains some useful constants and static methods used for standardizing
	/// word counter paths.
	/// </summary>
	public static class WordCounterPathUtility
	{
		#region Methods

		public static void GetCounts(
			IProjectPlugin project,
			Block block,
			out int count,
			out int wordCount,
			out int characterCount,
			out int nonWhitespaceCount)
		{
			// Make sure we have a sane state.
			if (project == null)
			{
				throw new ArgumentNullException("project");
			}

			if (block == null)
			{
				throw new ArgumentNullException("block");
			}

			// Figure out the root path for the various components.
			HierarchicalPath rootPath = GetPluginRootPath(project);

			count = GetCount(block, rootPath, "Total/" + CountType);
			wordCount = GetCount(block, rootPath, "Total/" + WordCountType);
			characterCount = GetCount(block, rootPath, "Total/" + CharacterCountType);
			nonWhitespaceCount = GetCount(
				block, rootPath, "Total/" + NonWhitespaceCountType);
		}

		/// <summary>
		/// Gets the deltas as a dictionary of key and deltas for the block.
		/// </summary>
		/// <param name="project">The project.</param>
		/// <param name="block">The block.</param>
		/// <param name="wordDelta">The word delta.</param>
		/// <param name="characterDelta">The character delta.</param>
		/// <param name="nonWhitespaceDelta">The non whitespace delta.</param>
		/// <returns>
		/// A dictionary of paths and deltas.
		/// </returns>
		public static Dictionary<HierarchicalPath, int> GetDeltas(
			IProjectPlugin project,
			Block block,
			int delta,
			int wordDelta,
			int characterDelta,
			int nonWhitespaceDelta)
		{
			// Make sure we have a sane arguments.
			if (project == null)
			{
				throw new ArgumentNullException("project");
			}

			if (block == null)
			{
				throw new ArgumentNullException("block");
			}

			// Create the dictionary and figure out the top-level elements.
			var deltas = new Dictionary<HierarchicalPath, int>();
			HierarchicalPath rootPath = GetPluginRootPath(project);

			// Add in the path for the totals.
			var totalPath = new HierarchicalPath("Total", rootPath);

			AddDeltas(
				deltas, totalPath, delta, wordDelta, characterDelta, nonWhitespaceDelta);

			// Add in a block-type specific path along with a counter.
			string relativeBlockPath = "Block Types/" + block.BlockType.Name;
			var blockPath = new HierarchicalPath(relativeBlockPath, rootPath);

			AddDeltas(
				deltas, blockPath, delta, wordDelta, characterDelta, nonWhitespaceDelta);

			// Return the resulting delta.
			return deltas;
		}

		/// <summary>
		/// Adds the deltas for the various counters underneath the given path.
		/// </summary>
		/// <param name="deltas">The deltas.</param>
		/// <param name="rootPath">The root path.</param>
		/// <param name="delta"></param>
		/// <param name="wordDelta">The word delta.</param>
		/// <param name="characterDelta">The character delta.</param>
		/// <param name="nonWhitespaceDelta">The non whitespace delta.</param>
		private static void AddDeltas(
			IDictionary<HierarchicalPath, int> deltas,
			HierarchicalPath rootPath,
			int delta,
			int wordDelta,
			int characterDelta,
			int nonWhitespaceDelta)
		{
			AddDeltas(deltas, rootPath, CountType, delta);
			AddDeltas(deltas, rootPath, WordCountType, wordDelta);
			AddDeltas(deltas, rootPath, CharacterCountType, characterDelta);
			AddDeltas(deltas, rootPath, NonWhitespaceCountType, nonWhitespaceDelta);
			AddDeltas(
				deltas, rootPath, WhitespaceCountType, characterDelta - nonWhitespaceDelta);
		}

		/// <summary>
		/// Adds a delta for a given path.
		/// </summary>
		/// <param name="deltas">The deltas.</param>
		/// <param name="rootPath">The root path.</param>
		/// <param name="type">The type.</param>
		/// <param name="delta">The delta.</param>
		private static void AddDeltas(
			IDictionary<HierarchicalPath, int> deltas,
			HierarchicalPath rootPath,
			string type,
			int delta)
		{
			var path = new HierarchicalPath(type, rootPath);

			if (deltas.ContainsKey(path))
			{
				deltas[path] += delta;
			}
			else
			{
				deltas[path] = delta;
			}
		}

		private static int GetCount(
			IPropertiesContainer propertiesContainer,
			HierarchicalPath rootPath,
			string countType)
		{
			var path = new HierarchicalPath(countType, rootPath);
			string count;

			return propertiesContainer.Properties.TryGetValue(path, out count)
				? Convert.ToInt32(count)
				: 0;
		}

		private static HierarchicalPath GetPluginRootPath(IProjectPlugin project)
		{
			return new HierarchicalPath("/Plugins/" + project.Key);
		}

		#endregion

		#region Fields

		private const string CharacterCountType = "Characters";
		private const string CountType = "Count";
		private const string NonWhitespaceCountType = "Non-Whitespace";
		private const string WhitespaceCountType = "Whitespace";
		private const string WordCountType = "Words";

		#endregion
	}
}
