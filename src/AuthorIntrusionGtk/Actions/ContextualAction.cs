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

using StructureMap;

using Action=Gtk.Action;

#endregion

namespace AuthorIntrusionGtk.Actions
{
	/// <summary>
	/// Implements an action that contains the <see cref="AuthorIntrusionGtk.Context"/>.
	/// </summary>
	public abstract class ContextualAction : Action
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ContextualAction"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="name">The name.</param>
		/// <param name="label">The label.</param>
		/// <param name="tooltip">The tooltip.</param>
		/// <param name="stockId">The stock ID.</param>
		protected ContextualAction(
			Context context,
			string name,
			string label,
			string tooltip,
			string stockId)
			: base(name, label, tooltip, stockId)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			Context = context;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the container for the IoC.
		/// </summary>
		/// <value>The container.</value>
		protected IContainer Container
		{
			get { return Context.Container; }
		}

		/// <summary>
		/// Gets or sets the context.
		/// </summary>
		/// <value>The action manager.</value>
		protected Context Context { get; private set; }

		#endregion
	}
}