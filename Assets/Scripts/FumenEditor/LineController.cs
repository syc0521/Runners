using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.FumenEditor
{
    public class LineController : Utils.Singleton<LineController>
    {
        public Material material;
        private List<DataStudio.Line> lines = new(); //向此List中添加线段即可画线

        void OnRenderObject()
        {
            GL.Clear(false, false, Color.black);
            if (!material)
            {
                Debug.LogError("请给材质资源赋值");
                return;
            }
            //设置该材质通道，0为默认值
            material.SetPass(0);
            //设置绘制2D图像
            GL.LoadOrtho();
            //表示开始绘制，绘制类型为线段
            GL.Begin(GL.LINES);

            foreach (var item in lines)
            {
                DrawLine(item);
            }

            //结束绘制
            GL.End();
        }

        /// <summary>
        /// 绘制线段，默认采用视口坐标系
        /// </summary>
        private void DrawLine(DataStudio.Line line)
        {
            DrawLine(line.start.x, line.start.y, line.end.x, line.end.y, line.color);
        }

        private void DrawLine(float x1, float y1, float x2, float y2, Color color)
        {
            GL.Color(color);
            GL.Vertex(new Vector3(x1, y1, 0));
            GL.Vertex(new Vector3(x2, y2, 0));
        }

        public void ClearAllLines()
        {
            lines.Clear();
        }

        public void AddLine(DataStudio.Line line)
        {
            lines.Add(line);
        }

    }
}
