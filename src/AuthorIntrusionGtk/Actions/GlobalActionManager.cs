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

using Gtk;

using MfGames.GtkExt.Actions;
using MfGames.Reporting;

using StructureMap;

#endregion

namespace AuthorIntrusionGtk.Actions
{
	/// <summary>
	/// Customizes an <see cref="ActionManager"/> for use within this application.
	/// </summary>
	[PluginFamily(IsSingleton = true)]
	public class GlobalActionManager : ActionManager
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="GlobalActionManager"/> class.
		/// </summary>
		/// <param name="layout">The layout.</param>
		/// <param name="keybindings">The keybindings.</param>
		/// <param name="actions">The actions.</param>
		public GlobalActionManager(
			GlobalActionLayout layout,
			GlobalActionKeybindings keybindings,
			Action[] actions)
		{
			// Set up the layout and keybindings.
			SetLayout(layout);
			SetKeybindings(keybindings);

			// Add all the actions and action factories.
			Add(actions);
		}

		#endregion
	}
}