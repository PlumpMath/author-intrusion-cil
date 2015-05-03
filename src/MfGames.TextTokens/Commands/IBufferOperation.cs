// <copyright file="IBufferOperation.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using MfGames.TextTokens.Buffers;

namespace MfGames.TextTokens.Commands
{
	/// <summary>
	/// Represents a single undoable operation that can be performed against the
	/// buffer.
	/// </summary>
	public interface IBufferOperation
	{
		#region Public Methods and Operators

		/// <summary>
		/// Performs the operation on the buffer.
		/// </summary>
		/// <param name="buffer">
		/// The buffer to execute the operations on.
		/// </param>
		void Do(IBuffer buffer);

		/// <summary>
		/// Reverses the operation on the given buffer.
		/// </summary>
		/// <param name="buffer">
		/// The buffer to execute the operations on.
		/// </param>
		void Undo(IBuffer buffer);

		#endregion
	}
}
