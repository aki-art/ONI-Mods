using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Moonlet.Graph
{
	public class Graph<T> : IEnumerable
	{
		List<Node<T>> nodes = [];

		public int Count => nodes.Count;
		public IList<Node<T>> Nodes => nodes.AsReadOnly();
		public IEnumerator GetEnumerator() => Nodes.GetEnumerator();

		public void Clear()
		{
			foreach (Node<T> node in nodes)
			{
				node.RemoveAllNeighbors();
			}

			for (int i = nodes.Count - 1; i >= 0; i--)
			{
				nodes.RemoveAt(i);
			}
		}

		public bool AddNode(T value)
		{
			if (Find(value) != null)
				return false;
			else
			{
				nodes.Add(new Node<T>(value));
				return true;
			}
		}

		public Node<T> AddOrGet(T value)
		{
			var node = Find(value);
			if (node != null)
				return node;
			else
			{
				node = new Node<T>(value);
				nodes.Add(node);
				return node;
			}
		}

		public bool AddEdge(T value1, T value2)
		{
			Node<T> node1 = Find(value1);
			Node<T> node2 = Find(value2);

			if (node1 == null || node2 == null)
				return false;

			else if (node1.Neighbors.Contains(node2))
				return false;

			else
			{
				node1.AddNeighbor(node2);
				node2.AddNeighbor(node1);

				return true;
			}
		}

		public bool RemoveNode(T value)
		{
			Node<T> nodeToRemove = Find(value);

			if (nodeToRemove == null)
				return false;
			else
			{
				nodes.Remove(nodeToRemove);

				foreach (Node<T> node in nodes)
					node.RemoveNeighbor(nodeToRemove);

				return true;
			}
		}

		public bool RemoveEdge(T value1, T value2)
		{
			Node<T> node1 = Find(value1);
			Node<T> node2 = Find(value2);

			if (node1 == null || node2 == null)
				return false;

			else if (!node1.Neighbors.Contains(node2))
				return false;

			else
			{
				node1.RemoveNeighbor(node2);
				node2.RemoveNeighbor(node1);
				return true;
			}
		}

		public Node<T> Find(T value)
		{
			foreach (Node<T> node in nodes)
			{
				if (node.Value.Equals(value))
					return node;
			}

			return null;
		}

		public bool Contains(T value)
		{
			foreach (Node<T> node in nodes)
			{
				if (node.Value.Equals(value))
					return true;
			}

			return false;
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();

			for (int i = 0; i < Count; i++)
			{
				builder.Append(nodes[i].ToString());

				if (i < Count)
					builder.Append("\n");
			}

			return builder.ToString();
		}
	}
}
