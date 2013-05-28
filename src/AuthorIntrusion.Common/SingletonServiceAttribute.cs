// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;

namespace AuthorIntrusion.Common
{
	/// <summary>
	/// A marker interface to indicate that class should be used as a singleton
	/// in the system.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class SingletonServiceAttribute: Attribute
	{
	}
}
