#region Namespaces

using System;

#endregion

namespace AuthorIntrusion.English.Attributes
{
	/// <summary>
	/// Implements an attribute placed on enumerations that allow for encoding
	/// of Penn Treebank codes on the values.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class TreebankCodeAttribute : Attribute
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="TreebankCodeAttribute"/> class.
		/// </summary>
		/// <param name="treebankCode">The treebank code.</param>
		public TreebankCodeAttribute(string treebankCode)
		{
			this.treebankCode = treebankCode;
		}

		#endregion

		#region Codes

		private readonly string treebankCode;

		/// <summary>
		/// Gets the treebank code associated with this attribute.
		/// </summary>
		/// <value>The treebank code.</value>
		public string TreebankCode
		{
			get { return treebankCode; }
		}

		#endregion
	}
}