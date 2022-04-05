namespace AsphaltStairs.Patches
{
    public class StairsPatch
    {
        public static class Stairs_Patches_Navigator_BeginTransition_Patch_Postfix_Patch
        {
            public static void Postfix(Navigator navigator, Navigator.ActiveTransition transition)
            {
                if (transition.navGridTransition.isCritter)
                    return;

                var cell = Grid.CellBelow(Grid.PosToCell(navigator));
                var tile = Grid.Objects[cell, (int)ObjectLayer.LadderTile];

                if (tile != null)
                {
                    if (tile.GetComponent<KPrefabID>().HasTag(AsphaltStairsConfig.ID))
                    {
                        transition.speed *= 3.0f; //asphaltSpeedMultiplier;
                        transition.animSpeed *= 3.0f; //asphaltSpeedMultiplier;
                    }
                }
            }
        }
    }
}