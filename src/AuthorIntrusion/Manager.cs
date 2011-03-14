#region Copyright and License

// Copyright (c) 2005-2011, Moonfire Games
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#endregion

#region Namespaces

using AuthorIntrusion.Contracts.IO;
using AuthorIntrusion.Contracts.Languages;

using StructureMap;

#endregion

namespace AuthorIntrusion
{
	/// <summary>
	/// Manages the entire author process, including extensions.
	/// </summary>
	public static class Manager
	{
		#region IOC

		/// <summary>
		/// Registers the specified instance for a given interface type.
		/// </summary>
		/// <typeparam name="TInterface">The type of the interface.</typeparam>
		/// <param name="instance">The instance.</param>
		public static void Register<TInterface>(TInterface instance)
		{
			ObjectFactory.Inject(instance);
		}

		/// <summary>
		/// Sets up the IoC library and extensions.
		/// </summary>
		public static void Setup()
		{
			// Set up StructureMap with the classes in this assembly and
			// everything in the Extensions folder.
			ObjectFactory.Initialize(
				x => x.Scan(
				     	scanner =>
				     	{
				     		// List the places we are searching for assemblies.
				     		scanner.AssembliesFromApplicationBaseDirectory();
				     		scanner.AssembliesFromPath("Extensions");

				     		// List the common types we need to load.
				     		scanner.AddAllTypesOf<IInputManager>();
				     		scanner.AddAllTypesOf<IInputReader>();

				     		scanner.AddAllTypesOf<IOutputManager>();
				     		scanner.AddAllTypesOf<IOutputWriter>();

				     		scanner.AddAllTypesOf<ILanguageManager>();

				     		scanner.AddAllTypesOf<IContentParser>();
				     	}));

			//System.Diagnostics.Debug.WriteLine(ObjectFactory.Container.WhatDoIHave());
		}

		#endregion
	}
}