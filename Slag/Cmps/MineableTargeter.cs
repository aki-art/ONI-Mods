using Slag.Content.Critters.Slagmite;

namespace Slag.Cmps
{
    // helper component for the Auto-Miner
    internal class MineableTargeter : KMonoBehaviour
    {
        private MineableCreature target;

        public bool HasTargetInCell(int cell)
        {
            target = null;

            var collision_entries = ListPool<ScenePartitionerEntry, SelectTool>.Allocate();
            GameScenePartitioner.Instance.GatherEntries(Extents.OneCell(cell), GameScenePartitioner.Instance.pickupablesLayer, collision_entries);

            foreach (var entry in collision_entries)
            {
                if (entry.obj is Pickupable obj && obj.TryGetComponent(out MineableCreature mineable) && mineable.IsMineable())
                {
                    target = mineable;
                    //target = mineable.GetSMI<ShellGrowthMonitor>().Min
                    //__result = true;
                    break;
                }
            }

            collision_entries.Recycle();

            return target != null;
        }

        public void UpdateDig()
        {
        }
    }
}
