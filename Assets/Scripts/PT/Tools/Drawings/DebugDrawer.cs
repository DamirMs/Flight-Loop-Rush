using System.Collections.Generic;
using UnityEngine;

namespace PT.Tools.Drawings
{
    public static class DebugDrawer
    {
        private struct DebugShape
        {
            public ShapeType Type;
            public Vector2 Center;
            public Vector2 Size;
            public float Angle;
            public Color Color;
            public float ExpireTime;
        }

        private enum ShapeType { Box }

        public static bool Enabled { get; set; }

        private static List<DebugShape> shapes = new List<DebugShape>();

        public static void DrawBox(Vector2 center, Vector2 size, float angle, Color color, float duration = 2f)
        {
            if (!Enabled) return;
            shapes.Add(new DebugShape
            {
                Type = ShapeType.Box,
                Center = center,
                Size = size,
                Angle = angle,
                Color = color,
                ExpireTime = Time.time + duration
            });
        }

        public static void UpdateGizmos()
        {
            if (!Enabled) return;

            for (int i = shapes.Count - 1; i >= 0; i--)
            {
                var shape = shapes[i];

                if (Time.time > shape.ExpireTime)
                {
                    shapes.RemoveAt(i);
                    continue;
                }

                Gizmos.color = shape.Color;
                Gizmos.matrix = Matrix4x4.TRS(shape.Center, Quaternion.Euler(0, 0, shape.Angle), Vector3.one);

                if (shape.Type == ShapeType.Box)
                    Gizmos.DrawWireCube(Vector3.zero, shape.Size);

                Gizmos.matrix = Matrix4x4.identity;
            }
        }
    }
}