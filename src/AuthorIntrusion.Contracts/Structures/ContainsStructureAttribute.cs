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

#endregion

namespace AuthorIntrusion.Contracts.Structures
{
	/// <summary>
	/// Used to identify what types of structures can be contained inside the
	/// given structure. This is put on the <see cref="StructureType"/> enumeration and used by
	/// <see cref="StructureTypeHelper"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public class ContainsStructureAttribute : Attribute
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ContainsStructureAttribute"/> class.
		/// </summary>
		/// <param name="structureType">Type of the structure.</param>
		public ContainsStructureAttribute(StructureType structureType)
		{
			StructureType = structureType;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the type of the structure that can be contained in the given
		/// type.
		/// </summary>
		/// <value>The type of the structure.</value>
		public StructureType StructureType { get; private set; }

		#endregion
	}
}