// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.Runtime.InteropServices;

namespace AuthorIntrusion.Plugins.Spelling.NHunspell.Interop
{
	/// <summary>
	/// Contains the P/Invoke calls for Hunspell.
	/// </summary>
	public static class HunspellInterop
	{
		#region Methods

		[DllImport("hunspell-1.2", CharSet = CharSet.Auto)]
		internal static extern int Hunspell_add(
			HunspellHandle handle,
			string word);

		[DllImport("hunspell-1.2", CharSet = CharSet.Auto)]
		internal static extern int Hunspell_add_with_affix(
			HunspellHandle handle,
			string word,
			string example);

		[DllImport("hunspell-1.2", CharSet = CharSet.Auto)]
		internal static extern IntPtr Hunspell_create(
			string affpath,
			string dpath);

		[DllImport("hunspell-1.2")]
		internal static extern void Hunspell_destroy(HunspellHandle handle);

		[DllImport("hunspell-1.2")]
		internal static extern void Hunspell_free_list(
			HunspellHandle handle,
			IntPtr slst,
			int n);

		[DllImport("hunspell-1.2", CharSet = CharSet.Auto)]
		internal static extern int Hunspell_spell(
			HunspellHandle handle,
			string word);

		[DllImport("hunspell-1.2", CharSet = CharSet.Auto)]
		internal static extern int Hunspell_suggest(
			HunspellHandle handle,
			IntPtr slst,
			string word);

		#endregion
	}
}
