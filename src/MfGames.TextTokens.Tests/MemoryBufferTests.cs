// <copyright file="MemoryBufferTests.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System;

using MfGames.TextTokens.Controllers;
using MfGames.TextTokens.Tokens;

using NUnit.Framework;

namespace MfGames.TextTokens.Tests
{
	/// <summary>
	/// Base class for tests that run against a memory buffer.
	/// </summary>
	public class MemoryBufferTests
	{
		#region Properties

		/// <summary>
		/// Gets the in-memory buffer model.
		/// </summary>
		protected TestBuffer Buffer { get; private set; }

		/// <summary>
		/// Gets the UI controller for the buffer.
		/// </summary>
		protected UserBufferController Controller { get; private set; }

		/// <summary>
		/// Gets the listener which reflects the user-visible state of
		/// the buffer.
		/// </summary>
		protected TestBufferState State { get; private set; }

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Tears down the test and show the final state of the buffer.
		/// </summary>
		[TearDown]
		public void TearDown()
		{
			// Report the state of the final buffer.
			Console.WriteLine();
			Console.WriteLine("Buffer State:");

			for (var index = 0; index < Buffer.Lines.Count; index++)
			{
				// For each line, give the line index and each token separated by [] brackets.
				Console.Write(
					"{0}: ",
					index.ToString()
						.PadLeft(4));

				foreach (IToken token in Buffer.Lines[index].Tokens)
				{
					Console.Write(
						"[{0}]",
						token.Text);
				}

				// Finish up the line.
				Console.WriteLine();
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Generic setup for all memory buffer tests.
		/// </summary>
		protected virtual void Setup()
		{
			KeyGenerator.Instance = new KeyGenerator();
			Buffer = new TestBuffer();
			Controller = new UserBufferController(Buffer);
			State = new TestBufferState(Buffer);
		}

		#endregion
	}
}
