using Runner.Core;
using Runner.DataStudio.Serialize;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

namespace Runner.FumenEditor
{
    /// <summary>
    /// 谱面编辑器: FumenEditorManager
    /// Author: 单元琛 2022-7-5
    /// </summary>
    public partial class FumenEditorManager : Utils.Singleton<FumenEditorManager>
    {
        #region File
        public void OpenFile()
        {
            FileOpenDialog dialog = new();
            dialog.structSize = Marshal.SizeOf(dialog);
            dialog.filter = "txt files\0*.txt\0All Files\0*.*\0\0";
            dialog.file = new string(new char[256]);
            dialog.maxFile = dialog.file.Length;
            dialog.fileTitle = new string(new char[64]);
            dialog.maxFileTitle = dialog.fileTitle.Length;
            dialog.initialDir = Global.musicPath;  //默认路径
            dialog.title = "打开文件";
            dialog.defExt = "txt";//显示文件的类型
                                  //注意一下项目不一定要全选 但是0x00000008项不要缺少
            dialog.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;  //OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR
            if (DialogShow.GetOpenFileName(dialog))
            {
                ReadFiles(dialog.file);
            }
        }
        #endregion

        #region EditorPosition
        /// <summary>
        /// 确定编辑区位置
        /// </summary>
        public float[] CreateEditorViewportPosition(RawImage image)
        {
            image.rectTransform.GetWorldCorners(editorCorners);
            Vector3 viewportPos = mainCamera.WorldToViewportPoint(image.transform.position);
            Vector3 backgroundScale = new(image.rectTransform.rect.width, image.rectTransform.rect.height);
            Vector3 backgroundViewportScale = mainCamera.ScreenToViewportPoint(backgroundScale);

            float left = viewportPos.x - backgroundViewportScale.x / 2;
            float right = viewportPos.x + backgroundViewportScale.x / 2;
            float top = viewportPos.y + backgroundViewportScale.y / 2;
            float bottom = viewportPos.y - backgroundViewportScale.y / 2;
            return new float[] { top, left, bottom, right};
        }
        #endregion

        #region Music
        public void PlayMusic()
        {
            if (isLoaded && !isPlaying)
            {
                isPlaying = true;
                AudioManager.Instance.PlayMusic(AudioEnum.Track,
                        Utils.MusicHandler.ConvertBarToTime(CurrentBar, fumenData.mainbpm, fumenData.barCount));
            }
        }

        private void PlayItemSound()
        {
            var currentTime = Utils.MusicHandler.ConvertBarToTime(CurrentBar, fumenData.mainbpm, fumenData.barCount);
            var items = fumenData.collections.FindAll((item) => item.time > currentTime);
        }

        public void StopMusic()
        {
            if (isPlaying)
            {
                isPlaying = false;
                AudioManager.Instance.StopMusic(AudioEnum.Track);
                CurrentBar = GetMusicBar();
            }
        }

        public void LoadMusic(int id)
        {
            if (AudioManager.Instance.isLoaded) return;
            length = Utils.MusicHandler.LoadMusic(id, AudioManager.Instance.GetAudioSource(AudioEnum.Track));
        }

        public void InitializeMusicData()
        {
            totalBar = Utils.MusicHandler.GetMusicTotalBar(length, fumenData.mainbpm, fumenData.barCount);
            Debug.Log(totalBar);
        }

        public int GetMusicBar()
        {
            return Utils.MusicHandler.GetMusicBar(AudioManager.Instance.GetAudioSource(AudioEnum.Track),
                fumenData.mainbpm, fumenData.barCount);
        }

        public int GetPreviewTime()
        {
            return (int)Utils.MusicHandler.ConvertBarToTime(CurrentBar, fumenData.mainbpm, fumenData.barCount);
        }


        #endregion

        #region Bar
        /// <summary>
        /// 改变小节数
        /// </summary>
        /// <param name="value">鼠标滑轮参数</param>
        private void ChangeBar(float value)
        {
            if (value > 0)
            {
                CurrentBar++;
                Debug.Log("+");
            }
            else if (value < 0)
            {
                CurrentBar--;
                Debug.Log("-");

            }
        }
        #endregion

        #region Composition
		public List<Composition> GetCompositionList()
        {
            return fumenData.compositions;
        }

        public void AddComposition(Composition composition)
        {
            fumenData.compositions.Add(composition);
        }

	    #endregion  
        private Vector3 GetItemPosition(float height, float width, BaseFieldObject item)
        {
            float x = editorCorners[0].x + width / 6 + width / 3 * (int)item.positionType;
            float y = editorCorners[0].y + height / 384 * item.submet + 0.4f;
            Vector3 pos = new Vector2(x, y);
            return pos;
        }
    }
}
