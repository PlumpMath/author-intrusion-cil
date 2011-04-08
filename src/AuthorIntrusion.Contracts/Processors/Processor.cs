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

using C5;

#endregion

namespace AuthorIntrusion.Contracts.Processors
{
	/// <summary>
	/// Contains the project-specific settings for a process, including the
	/// enabled state. This is serialized as part of the project.
	/// </summary>
	public abstract class Processor
	{
		#region Processor Dependencies

		/// <summary>
		/// Contains a list of services or processors that this item provides.
		/// This are analogous to the Requires to build up a dependency graph
		/// between the various providers.
		/// </summary>
		public abstract ICollection<string> Provides { get; }

		/// <summary>
		/// Contains a list of processors required to use this processor. These
		/// are string values as returned by the Provides of another processor.
		/// Circular references are not allow and will be disabled.
		/// </summary>
		public abstract ICollection<string> Requires { get; }

		#endregion

		#region Processing

		/// <summary>
		/// Processes information using the given context.
		/// </summary>
		/// <param name="context">The context.</param>
		public abstract void Process(ProcessorContext context);

		#endregion
	}
}