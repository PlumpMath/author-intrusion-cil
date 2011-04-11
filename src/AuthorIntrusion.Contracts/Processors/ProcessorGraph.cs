using System;

using C5;

using QuickGraph;

namespace AuthorIntrusion.Contracts.Processors
{
	/// <summary>
	/// Encapsulates the functionality for creating a processor graph which
	/// has the linkage between processors and the features they use and
	/// require.
	/// </summary>
	public class ProcessorGraph : BidirectionalGraph<ProcessorGraphEntry, SEdge<ProcessorGraphEntry>>
	{
		#region Fields

		private readonly ProcessorGraphEntry rootEntry;

		#endregion


		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ProcessorGraph"/> class.
		/// </summary>
		public ProcessorGraph()
		{
			// Add in the initial graph, which represents the root element.
			rootEntry = new ProcessorGraphEntry();
			AddVertex(rootEntry);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the root entry for the graph.
		/// </summary>
		/// <value>The root entry.</value>
		public ProcessorGraphEntry RootEntry
		{
			get { return rootEntry; }
		}

		#endregion

		#region Edges and Vertices

		/// <summary>
		/// Goes through the graph and calculates the depth and accessibility of
		/// the various entries.
		/// </summary>
		public void CalculateDepth()
		{
			// Go through and reset all the entries.
			foreach (var entry in Vertices)
			{
				entry.Reset();
			}

			// Start and the root and start calculating the depth.
			CalculateDepth(rootEntry, 0);
		}

		/// <summary>
		/// A recursive function that goes through the entry, calculates its
		/// depth, and then continues into the child entries.
		/// </summary>
		/// <param name="entry">The entry.</param>
		/// <param name="depth">The depth.</param>
		private void CalculateDepth(ProcessorGraphEntry entry, int depth)
		{
			// Check to see if this entry has a higher depth, if it does, then
			// we don't bother processing it.
			if (entry.Depth >= depth)
			{
				return;
			}

			entry.Depth = depth;

			// Go through all the out-going children of the node.
			foreach (var childEdge in OutEdges(entry))
			{
				CalculateDepth(childEdge.Target, depth + 1);
			}
		}

		/// <summary>
		/// Creates a sorted list of processors.
		/// </summary>
		/// <returns></returns>
		public IList<Processor> CreateSortedProcessors()
		{
			// Go through the vertices and add all the processors that are
			// reachable.
			var entries = new ArrayList<ProcessorGraphEntry>(VertexCount);

			foreach (ProcessorGraphEntry entry in Vertices)
			{
				// Skip non-processors and those without a depth.
				if (entry.ProcessorEntryType != ProcessorGraphEntryType.Processor)
				{
					continue;
				}

				if (entry.Depth < 0)
				{
					continue;
				}

				// Add it to the list.
				entries.Add(entry);
			}

			// Sort the entries.
			entries.Sort();

			// Create a processor list with the items in the correct order.
			var processors = new ArrayList<Processor>(entries.Count);

			for (int i = 0; i < entries.Count; i++)
			{
				processors.Add(entries[i].Processor);
			}

			// Return the resulting list.
			return processors;
		}

		/// <summary>
		/// Links the specified feature to a given processor.
		/// </summary>
		/// <param name="processor">The processor.</param>
		/// <param name="feature">The feature.</param>
		public void LinkProvider(Processor processor, string feature)
		{
			// Get the entries for both.
			var processorEntry = new ProcessorGraphEntry(processor);
			var featureEntry = new ProcessorGraphEntry(feature);

			// Make sure both exist in the graph.
			if (!ContainsVertex(featureEntry))
			{
				AddVertex(featureEntry);
			}

			if (!ContainsVertex(processorEntry))
			{
				AddVertex(processorEntry);
			}

			// Link the two from feature to processor.
			AddEdge(new SEdge<ProcessorGraphEntry>(processorEntry, featureEntry));
		}

		/// <summary>
		/// Links the specified feature to a given processor.
		/// </summary>
		/// <param name="feature">The feature.</param>
		/// <param name="processor">The processor.</param>
		public void LinkRequirement(string feature, Processor processor)
		{
			// Get the entries for both.
			var featureEntry = new ProcessorGraphEntry(feature);
			var processorEntry = new ProcessorGraphEntry(processor);

			// Make sure both exist in the graph.
			if (!ContainsVertex(featureEntry))
			{
				AddVertex(featureEntry);
			}

			if (!ContainsVertex(processorEntry))
			{
				AddVertex(processorEntry);
			}

			// Link the two from feature to processor.
			AddEdge(new SEdge<ProcessorGraphEntry>(featureEntry, processorEntry));
		}

		/// <summary>
		/// Links a given processor to the root element.
		/// </summary>
		/// <param name="processor">The processor.</param>
		public void LinkRoot(Processor processor)
		{
			// This processor is rooted into the root entry.
			var entry = new ProcessorGraphEntry(processor);

			AddVertex(entry);
			AddEdge(
				new SEdge<ProcessorGraphEntry>(
					RootEntry,
					entry));
		}

		#endregion

	}
}