// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using C5;
using MfGames.HierarchicalPaths;

namespace AuthorIntrusion.Common.Blocks
{
	/// <summary>
	/// Implements a dictionary of properties to be assigned to a block.
	/// </summary>
	public class BlockPropertyDictionary: HashDictionary<HierarchicalPath, object>
	{
		#region Methods

		/// <summary>
		/// Either adds a value to an existing key or adds a new one.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="amount">The amount.</param>
		/// <exception cref="System.NotImplementedException"></exception>
		public void AdditionOrAdd(
			HierarchicalPath path,
			int amount)
		{
			if (Contains(path))
			{
				this[path] = (int) this[path] + amount;
			}
			else
			{
				this[path] = amount;
			}
		}

		public TResult Get<TResult>(HierarchicalPath path)
		{
			var result = (TResult) this[path];
			return result;
		}

		public TResult GetOrDefault<TResult>(
			HierarchicalPath path,
			TResult defaultValue)
		{
			if (Contains(path))
			{
				return (TResult) this[path];
			}

			return defaultValue;
		}

		#endregion
	}
}
