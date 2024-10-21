using System.Collections.Generic;
using System.Text;

namespace Moonlet.Graph
{
	public class Node<T>
	{
		List<Node<T>> neighbors;

		public T Value { get; }
		public IList<Node<T>> Neighbors => neighbors.AsReadOnly();

		public Node(T value)
		{
			Value = value;
			neighbors = new List<Node<T>>();
		}

		public bool AddNeighbor(Node<T> neighbour)
		{
			if (neighbors.Contains(neighbour))
				return false;
			else
			{
				neighbors.Add(neighbour);
				return true;
			}
		}

		public bool RemoveNeighbor(Node<T> neighbour) => neighbors.Remove(neighbour);

		public bool RemoveAllNeighbors()
		{
			for (int i = neighbors.Count - 1; i >= 0; i--)
			{
				neighbors.RemoveAt(i);
			}

			return true;
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			builder.Append($"[Node: {Value} Neighbors: ");

			foreach (Node<T> node in neighbors)
			{
				builder.Append(node.Value + " ");
			}

			builder.Append("]");
			return builder.ToString();
		}
	}
}
