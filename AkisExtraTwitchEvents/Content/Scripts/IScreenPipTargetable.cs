using System.Collections.Generic;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
    public interface IScreenPipTargetable
    {
        public void Hide();

        public GameObject CreateVisualFake();

        public void Restore();

        public List<Vector3> GetPipPathNodes(Vector3 startPosition);
    }
}
