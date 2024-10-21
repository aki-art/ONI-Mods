using System.Collections.Generic;
using System.Text;

namespace Moonlet.Graph
{
	public class GraphSearch
	{
		public static string Search<T>(T start, T finish, Graph<T> graph, SearchType searchType)
		{
			LinkedList<Node<T>> searchList = new LinkedList<Node<T>>();

			if (start.Equals(finish))
				return start.ToString();

			Node<T> startNode = graph.Find(start);
			Node<T> finishNode = graph.Find(finish);

			if (startNode == null || finishNode == null)
				return "";

			var pathNodes = new Dictionary<Node<T>, PathNodeInfo<T>>
			{
				{ startNode, new PathNodeInfo<T>(null) }
			};

			searchList.AddFirst(startNode);

			while (searchList.Count > 0)
			{
				Node<T> currentNode = searchList.First.Value;
				searchList.RemoveFirst();

				foreach (Node<T> neighbor in currentNode.Neighbors)
				{
					if (neighbor.Value.Equals(finish))
					{
						pathNodes.Add(neighbor, new PathNodeInfo<T>(currentNode));
						return ConvertPathToString(neighbor, pathNodes);
					}
					else if (pathNodes.ContainsKey(neighbor))
					{
						// Cycle found
						continue;
					}
					else
					{
						pathNodes.Add(neighbor, new PathNodeInfo<T>(currentNode));
						if (searchType == SearchType.DepthFirst)
						{
							searchList.AddFirst(neighbor);
						}
						else
						{
							searchList.AddLast(neighbor);
						}
					}
				}
			}

			return "";
		}

		static string ConvertPathToString<T>(Node<T> endNode, Dictionary<Node<T>, PathNodeInfo<T>> pathNodes)
		{
			var path = new LinkedList<Node<T>>();

			path.AddFirst(endNode);
			Node<T> previous = pathNodes[endNode].Previous;

			while (previous != null)
			{
				path.AddFirst(previous);
				previous = pathNodes[previous].Previous;
			}

			StringBuilder builder = new StringBuilder();
			LinkedListNode<Node<T>> currentNode = path.First;
			int nodeCount = 0;

			while (currentNode != null)
			{
				nodeCount++;
				builder.Append(currentNode.Value.Value);

				if (nodeCount < path.Count)
					builder.Append(" ");

				currentNode = currentNode.Next;
			}

			return builder.ToString();
		}

		public enum SearchType
		{
			BreadthFirst,
			DepthFirst
		}
	}
}
