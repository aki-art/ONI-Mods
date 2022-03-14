using FUtility;
using SketchPad.History;
using System.Collections.Generic;
using UnityEngine;

namespace SketchPad.Tools.Pencil
{
    public class DrawCommand : ICommand
    {
        private readonly float strokeWidth;
        private readonly Color color;
        private List<Vector3> savedPoints;

        public SketchBookLine Stroke { get; private set; }

        public DrawCommand(float strokeWidth, Color color)
        {
            this.strokeWidth = strokeWidth;
            this.color = color;
        }

        public void Execute()
        {
            if (ModAssets.linePrefab == null)
            {
                ModAssets.CreateLinePrefab();
            }

            Stroke = Object.Instantiate(ModAssets.linePrefab).GetComponent<SketchBookLine>();

            LineRenderer renderer = Stroke.GetComponent<LineRenderer>();
            renderer.startWidth = renderer.endWidth = strokeWidth;
            renderer.startColor = renderer.endColor = color;

            Stroke.gameObject.SetActive(true);

            if(savedPoints != null)
            {
                Stroke.Set(savedPoints);
                savedPoints = null;
            }
        }

        public void Undo()
        {
            savedPoints = new List<Vector3>(Stroke.Points);

            Stroke.Clear();

            Object.Destroy(Stroke.gameObject);
            Log.Debuglog(Stroke?.gameObject == null);
        }
    }
}
