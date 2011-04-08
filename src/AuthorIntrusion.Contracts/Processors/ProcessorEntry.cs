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

namespace AuthorIntrusion.Contracts.Processors
{
	/// <summary>
	/// Represents information about a processor.
	/// </summary>
	public class ProcessorEntry : IEquatable<ProcessorEntry>
	{
		#region Fields

		private readonly Processor processor;
		private ProcessorEntryType processorEntryType;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates an entry that represents a processor.
		/// </summary>
		/// <param name="processor">The processor.</param>
		public ProcessorEntry(Processor processor)
		{
			// Save the properties.
			if (processor == null)
			{
				throw new ArgumentNullException("processor");
			}

			this.processor = processor;

			// Set the type of node we are creating.
			processorEntryType = ProcessorEntryType.Processor;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the processor if this is a processor node.
		/// </summary>
		/// <value>The processor.</value>
		public Processor Processor
		{
			get { return processor; }
		}

		/// <summary>
		/// Gets the type of the entry.
		/// </summary>
		/// <value>The type of the processor entry.</value>
		public ProcessorEntryType ProcessorEntryType
		{
			get { return processorEntryType; }
		}

		#endregion

		#region Operators

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(ProcessorEntry other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}

			return Equals(other.processor, processor) &&
			       Equals(other.processorEntryType, processorEntryType);
		}

		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
		/// </returns>
		/// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. </param><exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is null.</exception><filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}

			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			if (obj.GetType() != typeof(ProcessorEntry))
			{
				return false;
			}

			return Equals((ProcessorEntry) obj);
		}

		/// <summary>
		/// Serves as a hash function for a particular type. 
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"/>.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			unchecked
			{
				return ((processor != null ? processor.GetHashCode() : 0) * 397) ^
				       processorEntryType.GetHashCode();
			}
		}

		/// <summary>
		/// Implements the operator ==.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator ==(ProcessorEntry left,
		                               ProcessorEntry right)
		{
			return Equals(left, right);
		}

		/// <summary>
		/// Implements the operator !=.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator !=(ProcessorEntry left,
		                               ProcessorEntry right)
		{
			return !Equals(left, right);
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
			return ProcessorEntryType + " " + processor;
		}

		#endregion
	}
}