using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.FumenEditor
{
    public class LineController : Utils.Singleton<LineController>
    {
        public Material material;
        private List<DataStudio.Line> lines = new(); //���List������߶μ��ɻ���

        void OnRenderObject()
        {
            GL.Clear(false, false, Color.black);
            if (!material)
            {
                Debug.LogError("���������Դ��ֵ");
                return;
            }
            //���øò���ͨ����0ΪĬ��ֵ
            material.SetPass(0);
            //���û���2Dͼ��
            GL.LoadOrtho();
            //��ʾ��ʼ���ƣ���������Ϊ�߶�
            GL.Begin(GL.LINES);

            foreach (var item in lines)
            {
                DrawLine(item);
            }

            //��������
            GL.End();
        }

        /// <summary>
        /// �����߶Σ�Ĭ�ϲ����ӿ�����ϵ
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
