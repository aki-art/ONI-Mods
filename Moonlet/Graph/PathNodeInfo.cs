namespace Moonlet.Graph
{
	public class PathNodeInfo<T>
	{
		public Node<T> Previous { get; }

		public PathNodeInfo(Node<T> previous)
		{
			Previous = previous;
		}
	}
}
