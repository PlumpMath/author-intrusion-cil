// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using C5;
using MfGames.Conversion;
using MfGames.HierarchicalPaths;

namespace AuthorIntrusion.Common.Blocks
{
	/// <summary>
	/// Implements a dictionary of properties to be assigned to a block.
	/// </summary>
	public class BlockPropertyDictionary: HashDictionary<HierarchicalPath, string>
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
				int value = Convert.ToInt32(this[path]);
				this[path] = Convert.ToString(value + amount);
			}
			else
			{
				this[path] = Convert.ToString(amount);
			}
		}

		public TResult Get<TResult>(HierarchicalPath path)
		{
			string value = this[path];
			TResult result = ExtendableConvert.Instance.Convert<string, TResult>(value);
			return result;
		}

		/// <summary>
		/// Gets the value at the path, or the default if the item is not a stored
		/// property.
		/// </summary>
		/// <typeparam name="TResult">The type of the result.</typeparam>
		/// <param name="path">The path.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Either the converted value or the default value.</returns>
		public TResult GetOrDefault<TResult>(
			HierarchicalPath path,
			TResult defaultValue)
		{
			return Contains(path)
				? Get<TResult>(path)
				: defaultValue;
		}

		#endregion
	}
}
