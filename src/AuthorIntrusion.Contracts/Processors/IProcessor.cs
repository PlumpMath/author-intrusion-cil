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

namespace AuthorIntrusion.Contracts.Processors
{
	/// <summary>
	/// Represents a class that processes paragraph data and does one or more
	/// of the following: build up phrases and organize the paragraph (Parse),
	/// annotate the components of the paragraph (Analyze), or update the
	/// severities of various annotations (Report).
	/// </summary>
	public interface IProcessor
	{
		#region Information

		/// <summary>
		/// Gets a value indicating whether this processor is a singleton, which
		/// means there is only one instance for a given processor or if the user
		/// can create multiple instances and customize each one.
		/// </summary>
		bool IsSingleton { get; }

		/// <summary>
		/// Creates a new processor info associated with this processor. For
		/// singleton processes, this should require the same information object
		/// for every call, but for non-singleton, this will create a new
		/// object so allow the user to create multiple instances.
		/// </summary>
		/// <returns></returns>
		ProcessorInfo CreateProcessorInfo();

		#endregion

		#region Processing

		/// <summary>
		/// Performs the manipulation or reporting on a single paragraph. This
		/// will be called off the main GUI thread but only one processor will
		/// be in operation on a single paragraph at a time.
		/// </summary>
		/// <param name="context">The context.</param>
		void Process(ProcessorContext context);

		#endregion
	}
}