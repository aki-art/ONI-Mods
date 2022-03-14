using FUtility;
using System.Collections.Generic;
using UnityEngine;

namespace SketchPad.Tools.Pencil
{
    public class SketchBookLine : KMonoBehaviour
    {
        public List<Vector3> Points { get; private set; }

        [MyCmpReq]
        private LineRenderer lineRenderer;

        //private BezierCurve[] curves;

        private CubicBezierPath path;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Points = new List<Vector3>();
        }

        public void AddPoint(Vector2 point)
        {
            Points.Add(point);
        }

        public void Clear()
        {
            Points.Clear();
            lineRenderer.positionCount = 0;

            if (PencilTool.Instance.activeLine == this)
            {
                PencilTool.Instance.activeLine = null;
            }
        }

        public void Set(List<Vector3> points)
        {
            Points = points;
            lineRenderer.positionCount = Points.Count;
            lineRenderer.SetPositions(Points.ToArray());
        }

        public void Smooth(int sections)
        {
            // not enough points to smooth so just pass as is
            if (Points.Count < 3)
            {
                lineRenderer.positionCount = Points.Count;
                lineRenderer.SetPositions(Points.ToArray());

                return;
            }

            path = new CubicBezierPath(Points.ToArray());

            var num = path.GetControlVerts().Length;
            var segmentCount = num * sections;

            var test = new Vector3[segmentCount];

            for (int i = 0; i < num; i++)
            {
                int index = i * sections;

                for (int j = 0; j < sections; j++)
                {
                    test[index + j] = path.GetPointNorm((float)(index + j) / segmentCount);
                }
            }

            lineRenderer.positionCount = test.Length;
            lineRenderer.SetPositions(test);
        }
    }
}
