#region Namespaces

using System;

using AuthorIntrusion.Contracts.Interfaces;

#endregion

namespace AuthorIntrusion.English.Tags
{
	/// <summary>
	/// Implements a tag that defines an English phrase.
	/// </summary>
	public class EnglishTreebankTag : IElementTag
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="EnglishTreebankTag"/> class.
		/// </summary>
		/// <param name="treebankCode">The treebank code.</param>
		public EnglishTreebankTag(string treebankCode)
		{
			if (string.IsNullOrEmpty(treebankCode))
			{
				throw new ArgumentNullException("treebankCode");
			}

			this.treebankCode = treebankCode;
		}

		#endregion

		#region English

		private readonly string treebankCode;

		/// <summary>
		/// Gets the treebank code associated with this tag.
		/// </summary>
		/// <value>The treebank code.</value>
		public string TreebankCode
		{
			get { return treebankCode; }
		}

		#endregion

		#region Conversion

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		public override string ToString()
		{
			return treebankCode;
		}

		#endregion
	}
}