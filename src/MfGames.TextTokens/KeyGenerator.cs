// <copyright file="KeyGenerator.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Threading;

using MfGames.TextTokens.Lines;
using MfGames.TextTokens.Tokens;

namespace MfGames.TextTokens
{
	/// <summary>
	/// A simplistic key generator which starts the ID at 1 and simply increments it
	/// in a thread-safe manner for both lines and tokens.
	/// </summary>
	public class KeyGenerator : IKeyGenerator
	{
		#region Fields

		/// <summary>
		/// Contains the next ID for either lines or tokens.
		/// </summary>
		private int currentId;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		/// Initializes static members of the <see cref="KeyGenerator"/> class.
		/// </summary>
		static KeyGenerator()
		{
			Instance = new KeyGenerator();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="KeyGenerator"/> class.
		/// </summary>
		public KeyGenerator()
		{
			currentId = 0;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets a singleton instance of the current key generator.
		/// </summary>
		public static IKeyGenerator Instance { get; set; }

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Generates and retrieves the next line key.
		/// </summary>
		/// <returns>
		/// A LineKey that represents the next ID.
		/// </returns>
		public LineKey GetNextLineKey()
		{
			int id = Interlocked.Increment(ref currentId);
			var lineKey = new LineKey(id);
			return lineKey;
		}

		/// <summary>
		/// Generates and retrieves the next token key.
		/// </summary>
		/// <returns>
		/// A TokenKey that represents the next ID.
		/// </returns>
		public TokenKey GetNextTokenKey()
		{
			int id = Interlocked.Increment(ref currentId);
			var tokenKey = new TokenKey(id);
			return tokenKey;
		}

		#endregion
	}
}
