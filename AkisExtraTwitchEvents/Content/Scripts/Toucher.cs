using System.Collections.Generic;

namespace Twitchery.Content.Scripts
{
	public class Toucher : KMonoBehaviour
	{
		protected HashSet<int> alreadyVisitedCells;

		public void IgnoreCell(int cell)
		{
			alreadyVisitedCells?.Add(cell);
		}
	}
}
