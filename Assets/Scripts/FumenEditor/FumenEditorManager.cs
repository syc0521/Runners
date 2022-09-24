using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Runner.UI.Panel;
using UnityEngine.SceneManagement;
using System;
using Runner.Core;
using Runner.GamePlay;

namespace Runner.FumenEditor
{
    /// <summary>
    /// 谱面编辑器: FumenEditorManager
    /// Author: 单元琛 2022-7-5
    /// </summary>
    public partial class FumenEditorManager : Utils.Singleton<FumenEditorManager>
    {
        private PlayerControls playerControls;
        public Camera mainCamera;
        private string fumenPath;
        private DataStudio.Serialize.FumenData fumenData;
        private bool isPlaying = false, isLoaded = false, canPlace = false, canDelete = false;
        private List<GameObject> currentObjs = new();
        private Vector3[] editorCorners = new Vector3[4]; //左下 左上 右上 右下
        private GameObject tempObj;
        private Dictionary<int, int> divisionDic = new();
        private int currentDivisionIndex = 1;
        public DataStudio.Asset.FieldObjectsAsset fieldObjectsAsset;
        private string currentObjStr;
        public Action RefreshStateAction;

        private int totalBar;
        private long length;
        public float MusicTime { get => Utils.MusicHandler.GetMusicTime(AudioManager.Instance.GetAudioSource(AudioEnum.Track)); }
        public int TotalBar { get => totalBar; }

        public bool IsLoaded { get => isLoaded; }

        public int CurrentDivisionIndex
        {
            get
            {
                return currentDivisionIndex;
            }
            set
            {
                currentDivisionIndex = value;
                CurrentDivision = divisionDic[value];
            }
        }
        public int CurrentDivision { get; set; }

        public int CurrentBar // 当前小节数
        {
            get
            {
                return currentBar;
            }
            set
            {
                if (value >= 0 && value <= TotalBar)
                {
                    if (currentBar != value)
                    {
                        currentBar = value;
                        RefreshState();
                    }
                }

            }
        }
        private int currentBar;

        protected override void OnAwake()
        {
            playerControls = new PlayerControls();
            divisionDic.Add(0, 4);
            divisionDic.Add(1, 8);
            divisionDic.Add(2, 12);
            divisionDic.Add(3, 16);
            ChangeRenderCamera();
            UIManager.Instance.CreatePanel(PanelEnum.FumenEditor);
        }

        public void ChangeRenderCamera()
        {
            UIManager.Instance.ChangeRenderMode(PanelEnum.FumenEditor, mainCamera);
        }

        private void ReadFiles(string path)
        {
            fumenPath = path;
            fumenData = DataStudio.FumenReader.GetFumenData(path);
            LoadMusic(Utils.Utils.GetMusicID(path));
            InitializeMusicData();
            isLoaded = true;
        }

        public void SaveFile()
        {
            DataStudio.FumenWriter.WriteFumenData(fumenData, fumenPath);
        }

        public void Preview()
        {
            UIManager.Instance.CreatePanel(PanelEnum.Loading, new LoadingPanel.Option { sceneEnum = SceneEnum.Gameplay });
            int id = Utils.Utils.GetMusicID(fumenPath);
            GamePlayManager.MusicID = id;
        }

        /// <summary>
        /// 小节栏改变时刷新状态
        /// </summary>
        private void RefreshState()
        {
            RefreshStateAction?.Invoke();
            ClearItems();
            GenerateItems();
            Debug.Log(CurrentBar);
        }

        /// <summary>
        /// 清除在场上的物件
        /// </summary>
        public void ClearItems()
        {
            foreach (var item in currentObjs)
            {
                Destroy(item);
            }
            currentObjs.Clear();
        }

        /// <summary>
        /// 按照小节生成物件
        /// </summary>
        public void GenerateItems()
        {
            var items = fumenData.collections.FindAll((item) => item.met == currentBar);
            var obstacles = fumenData.obstacles.FindAll((item) => item.met == currentBar);
            float height = editorCorners[1].y - editorCorners[0].y;
            float width = editorCorners[2].x - editorCorners[1].x;
            foreach (var item in items)
            {
                Vector3 pos = GetItemPosition(height, width, item);
                GameObject obj = null;
                switch (item.type)
                {
                    case DataStudio.Serialize.CollectionType.Brick:
                        obj = Instantiate(fieldObjectsAsset.editorFieldObjectsDic["Brick"], pos, Quaternion.identity);
                        break;
                    case DataStudio.Serialize.CollectionType.Diamond:
                        break;
                    default:
                        break;
                }
                currentObjs.Add(obj);
            }
            foreach (var item in obstacles)
            {
                Vector3 pos = GetItemPosition(height, width, item);
                GameObject obj = null;
                switch (item.type)
                {
                    case DataStudio.Serialize.ObstacleType.Jump:
                        obj = Instantiate(fieldObjectsAsset.editorFieldObjectsDic["Jump"], pos, Quaternion.identity);
                        break;
                    case DataStudio.Serialize.ObstacleType.Slide:
                        obj = Instantiate(fieldObjectsAsset.editorFieldObjectsDic["Slide"], pos, Quaternion.identity);
                        break;
                    case DataStudio.Serialize.ObstacleType.Kick:
                        obj = Instantiate(fieldObjectsAsset.editorFieldObjectsDic["Kick"], pos, Quaternion.identity);
                        break;
                    case DataStudio.Serialize.ObstacleType.Lantern:
                        obj = Instantiate(fieldObjectsAsset.editorFieldObjectsDic["Lantern"], pos, Quaternion.identity);
                        break;
                    case DataStudio.Serialize.ObstacleType.Missile:
                        obj = Instantiate(fieldObjectsAsset.editorFieldObjectsDic["Missile"], pos, Quaternion.identity);
                        break;
                    case DataStudio.Serialize.ObstacleType.Reverse:
                        obj = Instantiate(fieldObjectsAsset.editorFieldObjectsDic["Reverse"], pos, Quaternion.identity);
                        break;
                    default:
                        break;
                }
                currentObjs.Add(obj);
            }

        }

        void Update()
        {
            if (isPlaying)
            {
                CurrentBar = GetMusicBar();
            }

            if (isLoaded && !isPlaying)
            {
                ChangeBar(Mouse.current.scroll.ReadValue().y);
                if (canPlace || canDelete)
                {
                    DetectMousePosition();
                }
            }

        }

        /// <summary>
        /// 检测鼠标位置，并加入物件
        /// </summary>
        private void DetectMousePosition()
        {
            var mousePos = Mouse.current.position.ReadValue();
            var mouseWorldPos = mainCamera.ScreenToWorldPoint(new(mousePos.x, mousePos.y));
            float height = editorCorners[1].y - editorCorners[0].y;
            float width = editorCorners[2].x - editorCorners[1].x;
            int indexX, indexY, currentSubMet;

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                if (tempObj != null)
                {
                    Destroy(tempObj);
                    canPlace = false;
                    canDelete = false;
                }
            }

            if (mouseWorldPos.x > editorCorners[0].x && mouseWorldPos.x < editorCorners[2].x
                && mouseWorldPos.y > editorCorners[0].y && mouseWorldPos.y < editorCorners[1].y) // 在编辑范围内
            {
                var deltaPos = mouseWorldPos - editorCorners[0]; // 计算鼠标位置与左下角的差向量
                //处理x y方向
                indexX = (int)(deltaPos.x / (width / 3.0f));
                indexY = (int)(deltaPos.y / (height / CurrentDivision));
                float x = editorCorners[0].x + width / 6 + width / 3 * indexX;
                float y = editorCorners[0].y + height / CurrentDivision * indexY + 0.4f;
                currentSubMet = indexY * (384 / CurrentDivision);
                if (canPlace && tempObj != null) // 放置物品
                {
                    tempObj.transform.position = new Vector3(x, y);
                    if (Mouse.current.leftButton.wasPressedThisFrame)
                    {

                        DataStudio.Serialize.BaseFieldObject note = null;
                        switch (currentObjStr)
                        {
                            case "Brick":
                                note = new DataStudio.Serialize.Brick((uint)currentBar, (uint)currentSubMet, (uint)indexX);
                                fumenData.collections.Add((DataStudio.Serialize.Collection)note);
                                break;
                            case "Jump":
                                note = new DataStudio.Serialize.JumpObstacle((uint)currentBar, (uint)currentSubMet, (uint)indexX);
                                fumenData.obstacles.Add((DataStudio.Serialize.Obstacle)note);
                                break;
                            case "Slide":
                                note = new DataStudio.Serialize.SlideObstacle((uint)currentBar, (uint)currentSubMet, (uint)indexX);
                                fumenData.obstacles.Add((DataStudio.Serialize.Obstacle)note);
                                break;
                            case "Kick":
                                note = new DataStudio.Serialize.KickObstacle((uint)currentBar, (uint)currentSubMet, (uint)indexX);
                                fumenData.obstacles.Add((DataStudio.Serialize.Obstacle)note);
                                break;
                            case "Lantern":
                                note = new DataStudio.Serialize.LanternObstacle((uint)currentBar, (uint)currentSubMet, (uint)indexX);
                                fumenData.obstacles.Add((DataStudio.Serialize.Obstacle)note);
                                break;
                            case "Missile":
                                note = new DataStudio.Serialize.MissileObstacle((uint)currentBar, (uint)currentSubMet, (uint)indexX);
                                fumenData.obstacles.Add((DataStudio.Serialize.Obstacle)note);
                                break;
                            case "Reverse":
                                note = new DataStudio.Serialize.ReverseObstacle((uint)currentBar, (uint)currentSubMet, (uint)indexX);
                                fumenData.obstacles.Add((DataStudio.Serialize.Obstacle)note);
                                break;

                            default:
                                break;
                        }
                        RefreshState();
                    }
                }

                if (canDelete && tempObj != null)
                {
                    tempObj.transform.position = new Vector3(x, y);
                    if (Mouse.current.leftButton.wasPressedThisFrame)
                    {
                        var collection = fumenData.collections.Find((item) => item.met == currentBar && item.submet == currentSubMet && item.positionType == (DataStudio.Serialize.PositionType)indexX);
                        var obstacle = fumenData.obstacles.Find((item) => item.met == currentBar && item.submet == currentSubMet && item.positionType == (DataStudio.Serialize.PositionType)indexX);
                        fumenData.collections.Remove(collection);
                        fumenData.obstacles.Remove(obstacle);
                        RefreshState();
                    }
                }
            }
        }

        public void DeleteFieldObject()
        {
            if (isLoaded && !isPlaying)
            {
                if (tempObj != null)
                {
                    Destroy(tempObj);
                }
                canDelete = true;
                tempObj = Instantiate(fieldObjectsAsset.editorFieldObjectsDic["Brick"]);
                tempObj.GetComponent<SpriteRenderer>().color = new(1.0f, 0.0f, 0.0f, 0.5f);
            }
        }

        public void AddFieldObject(string s)
        {
            if (isLoaded && !isPlaying)
            {
                if (tempObj != null)
                {
                    Destroy(tempObj);
                }
                canPlace = true;
                currentObjStr = s;
                tempObj = Instantiate(fieldObjectsAsset.editorFieldObjectsDic[s]);
                tempObj.GetComponent<SpriteRenderer>().color = new(1.0f, 1.0f, 1.0f, 0.5f);
            }
        }

        private void OnEnable()
        {
            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }



    }
}