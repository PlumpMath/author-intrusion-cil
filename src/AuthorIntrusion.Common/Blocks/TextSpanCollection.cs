// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Linq;
using AuthorIntrusion.Common.Plugins;
using C5;

namespace AuthorIntrusion.Common.Blocks
{
	/// <summary>
	/// A specialized collection for TextSpans.
	/// </summary>
	public class TextSpanCollection: ArrayList<TextSpan>
	{
		#region Methods

		/// <summary>
		/// Returns true if there is text span that contains the given index.
		/// </summary>
		/// <param name="textIndex"></param>
		/// <returns></returns>
		public bool Contains(int textIndex)
		{
			// If we have any text span in range, then return true. Otherwise,
			// return false to indicate nothing contains this range.
			return
				this.Any(
					textSpan =>
						textIndex >= textSpan.StartTextIndex && textIndex < textSpan.StopTextIndex);
		}

		/// <summary>
		/// Removes all the text spans of a given controller.
		/// </summary>
		/// <param name="controller">The controller to remove the spans for.</param>
		public void Remove(IProjectPlugin controller)
		{
			var removeSpans = new HashSet<TextSpan>();

			foreach (TextSpan textSpan in
				this.Where(textSpan => textSpan.Controller == controller))
			{
				removeSpans.Add(textSpan);
			}

			foreach (TextSpan textSpan in removeSpans)
			{
				Remove(textSpan);
			}
		}

		#endregion
	}
}
