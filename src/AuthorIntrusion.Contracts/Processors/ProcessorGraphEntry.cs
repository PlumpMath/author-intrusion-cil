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
	public class ProcessorGraphEntry : IEquatable<ProcessorGraphEntry>, IComparable<ProcessorGraphEntry>
	{
		private readonly string feature;

		#region Fields

		private readonly Processor processor;
		private readonly ProcessorGraphEntryType processorEntryType;
		private int depth;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new root entry.
		/// </summary>
		public ProcessorGraphEntry()
		{
			processorEntryType = ProcessorGraphEntryType.Root;
			depth = -1;
		}

		/// <summary>
		/// Creates a graph entry that represents a feature or requirement.
		/// </summary>
		/// <param name="feature">The requirement.</param>
		public ProcessorGraphEntry(string feature)
		{
			// Save the properties.
			if (feature == null)
			{
				throw new ArgumentNullException("feature");
			}

			this.feature = feature;

			// Set the type to a feature.
			processorEntryType = ProcessorGraphEntryType.Feature;
		}

		/// <summary>
		/// Creates an entry that represents a processor.
		/// </summary>
		/// <param name="processor">The processor.</param>
		public ProcessorGraphEntry(Processor processor)
		{
			// Save the properties.
			if (processor == null)
			{
				throw new ArgumentNullException("processor");
			}

			this.processor = processor;

			// Set the type of node we are creating.
			processorEntryType = ProcessorGraphEntryType.Processor;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the depth of the entry from the root or -1 if this
		/// entry does not connect to the root.
		/// </summary>
		/// <value>The depth.</value>
		public int Depth
		{
			get { return depth; }
			set { depth = value; }
		}

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
		public ProcessorGraphEntryType ProcessorEntryType
		{
			get { return processorEntryType; }
		}

		/// <summary>
		/// Resets this instance.
		/// </summary>
		public void Reset()
		{
			depth = -1;
		}

		#endregion

		#region Comparison

		/// <summary>
		/// Compares the current object with another object of the same type.
		/// </summary>
		/// <returns>
		/// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>. 
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public int CompareTo(ProcessorGraphEntry other)
		{
			// Lowest depth always comes first.
			if (depth != other.depth)
			{
				return depth.CompareTo(other.depth);
			}

			// Roots have very simple rules for comparison.
			if (processorEntryType == ProcessorGraphEntryType.Root)
			{
				return -1;
			}

			if (other.processorEntryType == ProcessorGraphEntryType.Root)
			{
				return 1;
			}

			// Otherwise, sort on the key value.
			return processor.ProcessorKey.CompareTo(other.processor.ProcessorKey);
		}

		#endregion

		#region Operators

		/// <summary>
		/// Gets the key object used for comparisons.
		/// </summary>
		/// <value>The key object.</value>
		private object KeyObject
		{
			get
			{
				switch (processorEntryType)
				{
					case ProcessorGraphEntryType.Feature:
						return feature;
					case ProcessorGraphEntryType.Processor:
						return processor.ProcessorKey;
					case ProcessorGraphEntryType.Root:
						return null;
					default:
						throw new InvalidOperationException(
							"Cannot identify type: " + processorEntryType);
				}
			}
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(ProcessorGraphEntry other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}

			return Equals(other.KeyObject, KeyObject) &&
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

			if (obj.GetType() != typeof(ProcessorGraphEntry))
			{
				return false;
			}

			return Equals((ProcessorGraphEntry) obj);
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
				return ((KeyObject != null ? KeyObject.GetHashCode() : 0) * 397) ^
				       processorEntryType.GetHashCode();
			}
		}

		/// <summary>
		/// Implements the operator ==.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator ==(ProcessorGraphEntry left,
		                               ProcessorGraphEntry right)
		{
			return Equals(left, right);
		}

		/// <summary>
		/// Implements the operator !=.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator !=(ProcessorGraphEntry left,
		                               ProcessorGraphEntry right)
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
			object key = KeyObject;

			return ProcessorEntryType + " " + depth + (key == null ? String.Empty : ": " + key);
		}

		#endregion
	}
}