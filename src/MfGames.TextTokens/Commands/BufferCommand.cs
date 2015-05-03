// <copyright file="BufferCommand.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Collections.Generic;
using System.Linq;

using MfGames.TextTokens.Buffers;

namespace MfGames.TextTokens.Commands
{
	/// <summary>
	/// A command that can be undone or redone.
	/// </summary>
	public class BufferCommand : List<IBufferOperation>
	{
		#region Fields

		/// <summary>
		/// Contains the list of operations the buffer provided to handle
		/// changes such a re-token-splitting.
		/// </summary>
		private List<IBufferOperation> updateOperations;

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Performs the operations on the given buffer.
		/// </summary>
		/// <param name="buffer">
		/// The buffer.
		/// </param>
		public void Do(IBuffer buffer)
		{
			// Perform the operations for this command.
			foreach (IBufferOperation operation in this)
			{
				operation.Do(buffer);
			}

			// Include any operations for tokenization.
			updateOperations = buffer.GetUpdateOperations()
				.ToList();

			foreach (IBufferOperation operation in updateOperations)
			{
				operation.Do(buffer);
			}
		}

		/// <summary>
		/// Reverses the operations on the given buffer.
		/// </summary>
		/// <param name="buffer">
		/// The buffer.
		/// </param>
		public void Undo(IBuffer buffer)
		{
			// Reverse the update operations. Once we are done, we remove the update
			// operations because "Do" will replace them with a new set if the user
			// redoes the command.
			updateOperations.Reverse();

			foreach (IBufferOperation operation in updateOperations)
			{
				operation.Undo(buffer);
			}

			updateOperations = null;

			// Reverse the operations of this command.
			List<IBufferOperation> reversed = this.ToList();
			reversed.Reverse();

			foreach (IBufferOperation operation in reversed)
			{
				operation.Undo(buffer);
			}
		}

		#endregion
	}
}
