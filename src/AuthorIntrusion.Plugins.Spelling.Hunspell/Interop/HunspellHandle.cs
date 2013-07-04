// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.Runtime.InteropServices;

namespace AuthorIntrusion.Plugins.Spelling.NHunspell.Interop
{
	/// <summary>
	/// Encapsulates an Hunspell handle.
	/// </summary>
	internal sealed class HunspellHandle: SafeHandle
	{
		#region Properties

		public override bool IsInvalid
		{
			get { return handle == IntPtr.Zero; }
		}

		#endregion

		#region Methods

		protected override bool ReleaseHandle()
		{
			HunspellInterop.Hunspell_destroy(this);

			return true;
		}

		#endregion

		#region P/Invokes

		#endregion

		#region Constructors

		public HunspellHandle()
			: base(IntPtr.Zero, false)
		{
		}

		public HunspellHandle(
			string affpath,
			string dicpath)
			: base(IntPtr.Zero, true)
		{
			handle = HunspellInterop.Hunspell_create(affpath, dicpath);

			if (IsInvalid)
			{
				throw new InvalidOperationException("Couldn't load hunspell.");
			}
		}

		#endregion
	}
}
