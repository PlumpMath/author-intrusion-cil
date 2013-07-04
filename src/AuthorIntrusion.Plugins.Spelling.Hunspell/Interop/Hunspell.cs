// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;

namespace AuthorIntrusion.Plugins.Spelling.NHunspell.Interop
{
	/// <summary>
	/// Wrapper around the Hunspell library.
	/// </summary>
	internal sealed class Hunspell
	{
		#region Methods

		/// <summary>Returns a list of misspelled words in the text.</summary>
		public string[] Check(string text)
		{
			var words = new List<string>();

			int index = 0;
			while (index < text.Length)
			{
				if (DoIsWordStart(text, index))
				{
					int count = DoGetWordLength(text, index);
					string word = text.Substring(index, count);
					index += count;

					count = DoSkipMarkup(text, index);
					if (count == 0)
					{
						word = word.Trim('\'', '-');
						if (words.IndexOf(word) < 0
							&& !CheckWord(word))
						{
							words.Add(word);
						}
					}
					else
					{
						index += count;
					}
				}
				else
				{
					int count = Math.Max(1, DoSkipMarkup(text, index));
					index += count;
				}
			}

			return words.ToArray();
		}

		public bool CheckWord(string word)
		{
			// If the word is mixed case or has numbers call it good.
			for (int i = 1;
				i < word.Length;
				++i)
			{
				if (char.IsUpper(word[i])
					|| char.IsNumber(word[i]))
				{
					return true;
				}
			}

			int result = HunspellInterop.Hunspell_spell(handle, word);
			GC.KeepAlive(this);
			return result != 0;
		}

		/// <summary>Returns suggested spellings for a word.</summary>
		public string[] Suggest(string word)
		{
			var suggestions = new List<string>();

			int ptrSize = Marshal.SizeOf(typeof (IntPtr));
			IntPtr slst = Marshal.AllocHGlobal(ptrSize);
			int count = HunspellInterop.Hunspell_suggest(handle, slst, word);

			if (count > 0)
			{
				IntPtr sa = Marshal.ReadIntPtr(slst);
				for (int i = 0;
					i < count;
					++i)
				{
					IntPtr sp = Marshal.ReadIntPtr(sa, i * ptrSize);
					string suggestion = Marshal.PtrToStringAuto(sp);
					suggestions.Add(suggestion);
				}
				HunspellInterop.Hunspell_free_list(handle, slst, count);
			}

			Marshal.FreeHGlobal(slst);

			return suggestions.ToArray();
		}

		private static int DoGetWordLength(
			string text,
			int index)
		{
			int count = 0;

			while (index + count < text.Length)
			{
				char ch = text[index + count];
				switch (char.GetUnicodeCategory(ch))
				{
						//                                      case UnicodeCategory.DashPunctuation:
					case UnicodeCategory.DecimalDigitNumber:
					case UnicodeCategory.LowercaseLetter:
					case UnicodeCategory.NonSpacingMark:
					case UnicodeCategory.TitlecaseLetter:
					case UnicodeCategory.UppercaseLetter:
						++count;
						break;

					case UnicodeCategory.OtherPunctuation:
						if (ch == '\'')
						{
							++count;
						}
						else
						{
							return count;
						}
						break;

					default:
						return count;
				}
			}

			return count;
		}

		private static bool DoIsWordStart(
			string text,
			int index)
		{
			char ch = text[index];
			switch (char.GetUnicodeCategory(ch))
			{
				case UnicodeCategory.LowercaseLetter:
				case UnicodeCategory.TitlecaseLetter:
				case UnicodeCategory.UppercaseLetter:
					return true;
			}

			return false;
		}

		private static int DoSkipMarkup(
			string text,
			int index)
		{
			int count = 0;

			if (index < text.Length
				&& text[index] == '<')
			{
				while (index + count < text.Length
					&& text[index + count] != '>')
				{
					++count;
				}

				if (index + count < text.Length
					&& text[index + count] == '>')
				{
					++count;
				}
				else
				{
					count = 0;
				}
			}

			return count;
		}

		#endregion

		#region Constructors

		internal Hunspell(
			string affixFilename,
			string dictionaryFilename)
		{
			// Make sure we have a sane state.
			if (string.IsNullOrWhiteSpace(affixFilename))
			{
				throw new ArgumentNullException("affixFilename");
			}

			if (string.IsNullOrWhiteSpace(dictionaryFilename))
			{
				throw new ArgumentNullException("dictionaryFilename");
			}

			// Keep the handle we'll be using.
			handle = new HunspellHandle(affixFilename, dictionaryFilename);
		}

		#endregion

		#region Fields

		private readonly HunspellHandle handle;

		#endregion
	}
}
