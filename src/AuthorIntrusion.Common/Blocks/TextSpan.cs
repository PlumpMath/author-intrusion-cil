// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Plugins;

namespace AuthorIntrusion.Common.Blocks
{
	/// <summary>
	/// Represents a controller-influenced section of the text inside a block.
	/// A text span has both range information on a block and associated data
	/// for a single controller.
	/// 
	/// Like many of the range classes, a text span is read-only and has no public
	/// setters.
	/// </summary>
	public class TextSpan
	{
		#region Properties

		public ITextControllerProjectPlugin Controller { get; set; }
		public object Data { get; private set; }

		public int Length
		{
			get { return StopTextIndex - StartTextIndex; }
		}

		public int StartTextIndex { get; set; }
		public int StopTextIndex { get; set; }

		#endregion

		#region Methods

		/// <summary>
		/// Gets the text for the text span from the input line.
		/// </summary>
		/// <param name="input">The input string.</param>
		/// <returns>The substring that represents the text.</returns>
		public string GetText(string input)
		{
			string part = input.Substring(StartTextIndex, Length);
			return part;
		}

		#endregion

		#region Constructors

		public TextSpan(
			int startTextIndex,
			int stopTextIndex,
			ITextControllerProjectPlugin controller,
			object data)
		{
			StartTextIndex = startTextIndex;
			StopTextIndex = stopTextIndex;
			Controller = controller;
			Data = data;
		}

		public TextSpan()
		{
		}

		#endregion
	}
}
