using UnityEngine;

namespace Runner.DataStudio
{
    public struct Line
    {
        public Vector3 start, end;
        public Color color;

        public Line(Vector3 start, Vector3 end)
        {
            this.start = start;
            this.end = end;
            color = Color.white;
        }

        public Line(Vector3 start, Vector3 end, Color color)
        {
            this.start = start;
            this.end = end;
            this.color = color;
        }
    }
}