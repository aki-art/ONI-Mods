using System;
using UnityEngine;

namespace ZipLine.Content.Cmps
{
    public class KPolygonCollider2D : KCollider2D
    {
        [MyCmpAdd]
        private PolygonCollider2D collider;

        public Vector2[] Points => collider.GetPath(0);

        public override Bounds bounds => collider.bounds;

        public void SetPoints(Vector2[] points)
        {
            collider.SetPath(0, points);
        }

        public override Extents GetExtents()
        {
            var minX = float.MaxValue;
            var maxX = float.MinValue;
            var minY = float.MaxValue;
            var maxY = float.MinValue;

            foreach (var point in collider.points)
            {
                var offsetPoint = new Vector2(transform.position.x + point.x, transform.position.y + point.y);
                minX = Math.Min(offsetPoint.x, minX);
                maxX = Math.Max(offsetPoint.x, maxX);
                minY = Math.Min(offsetPoint.y, minY);
                maxY = Math.Max(offsetPoint.y, maxY);
            }

            var extents = new Extents((int)minX, (int)minY, (int)(maxX - minX), (int)(maxY - minY));

            return extents;
        }

        public override bool Intersects(Vector2 pos)
        {
            return collider.OverlapPoint(pos);
        }
    }
}
