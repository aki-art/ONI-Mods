using System.Text;

namespace Terraformer
{
    public class Util
    {
        public static bool AreAnySpaceCraftsPresent(WorldContainer worldContainer)
        {
            AxialI location = worldContainer.GetMyWorldLocation();
            foreach (Clustercraft clustercraft in Components.Clustercrafts)
            {
                if (clustercraft.Location == location || clustercraft.Destination == location)
                {
                    return true;
                }
            }

            return false;
        }

        public static void DumpGrid()
        {
            StringBuilder str = new StringBuilder();
            for(int x = 0; x< Grid.WidthInCells; x++)
            {
                for(int y = 0; y< Grid.HeightInCells; y++)
                {
                    int cell = Grid.XYToCell(x, y);
                    int id = Grid.WorldIdx[cell];
                    str.Append(id == 255 ? " " : id.ToString());
                }

                str.Append("\n");
            }

            Debug.Log(str.ToString());
        }
    }
}
