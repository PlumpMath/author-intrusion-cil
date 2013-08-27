// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
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
	public class TextSpan: IEquatable<TextSpan>
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

		public bool Equals(TextSpan other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}

			if (ReferenceEquals(this, other))
			{
				return true;
			}

			return Equals(Controller, other.Controller) && Equals(Data, other.Data)
				&& StartTextIndex == other.StartTextIndex
				&& StopTextIndex == other.StopTextIndex;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}

			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			if (obj.GetType() != GetType())
			{
				return false;
			}

			return Equals((TextSpan) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = (Controller != null
					? Controller.GetHashCode()
					: 0);
				hashCode = (hashCode * 397) ^ (Data != null
					? Data.GetHashCode()
					: 0);
				hashCode = (hashCode * 397) ^ StartTextIndex;
				hashCode = (hashCode * 397) ^ StopTextIndex;
				return hashCode;
			}
		}

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

		#region Operators

		public static bool operator ==(TextSpan left,
			TextSpan right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(TextSpan left,
			TextSpan right)
		{
			return !Equals(left, right);
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
