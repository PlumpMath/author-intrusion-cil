#region Copyright and License

// Copyright (c) 2011, Moonfire Games
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

using System;
using System.Collections.Generic;

using AuthorIntrusion;

using C5;

using Ninject;
using Ninject.Extensions.Conventions;

#endregion

namespace AuthorIntrusionGui
{
	internal static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			// Setup the IoC container to connect everything up. This uses a
			// "by convention" setup to automatically include all the proper
			// bindings for the plugins.
			var kernel = new StandardKernel();

			kernel.Bind(
				x => x
				     	.FromAssembliesInPath(".")
				     	.SelectAllClasses()
				     	.Join
						.FromAssembliesInPath("Plugins")
				     	.SelectAllClasses()
				     	.BindAllInterfaces()
						.Configure(b => b.InSingletonScope()));

			// Get the GUI factory which handles creating the windows and
			// starting up the application. This allows for multiple GUI
			// interfaces to be included in the same installation. Each GUI
			// factory has a priority which is based on the environment (so the
			// Windows-specific version will have a high priority on Windows
			// but a low priority on Linux and Mac). The highest valid factory
			// is used to start up the application.
			var guiFactories = new ArrayList<IGuiFactory>();

			guiFactories.AddAll(kernel.GetAll<IGuiFactory>());
			guiFactories.Sort(new GuiFactoryComparer());

			// Go through and find the first GUI factory that we can use.
			foreach (IGuiFactory guiFactory in guiFactories)
			{
				// If it isn't valid, then skip it. Otherwise, use it.
				if (guiFactory.IsValid)
				{
					guiFactory.Start();
					return;
				}
			}

			// If we got this far, we could not find a GUI factory to use.
			throw new ApplicationException(
				"Cannot start the graphical user iterface. "
				+ "Cannot find a plugin that implements the IGuiFactory interface.");
		}

		#region Nested type: GuiFactoryComparer

		/// <summary>
		/// Comparison class used for sorting GUI factories.
		/// </summary>
		private class GuiFactoryComparer : IComparer<IGuiFactory>
		{
			/// <summary>
			/// Compares the two GUI factories and determines the highest priority one.
			/// </summary>
			/// <param name="guiFactory1">The GUI factory1.</param>
			/// <param name="guiFactory2">The GUI factory2.</param>
			/// <returns></returns>
			public int Compare(
				IGuiFactory guiFactory1,
				IGuiFactory guiFactory2)
			{
				// Get each GUI factory's self-determined score.
				int priority1 = guiFactory1.Priority;
				int priority2 = guiFactory2.Priority;

				return priority1.CompareTo(priority2);
			}
		}

		#endregion
	}
}