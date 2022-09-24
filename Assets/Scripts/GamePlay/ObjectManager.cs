using Runner.DataStudio.Serialize;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriWare;
using System.Linq;
using System;
using Runner.Core;
using Runner.UI.Panel;

namespace Runner.GamePlay
{
    public class ObjectManager : Utils.Singleton<ObjectManager>
    {
        public DataStudio.Asset.FieldObjectsAsset fieldObjectsAsset;
        [HideInInspector]
        public List<Objects.BaseCollection> collections;
        [HideInInspector]
        public List<Objects.BaseObstacle> obstacles;
        public Transform up, down, player;
        private Vector3 playerPos;
        public static float baseMovetime = 1.25f;
        public static float baseMoveSpeed = 7.1f;
        public CriAtomSource collectionSound, kickSound, jumpSound, HurtSound, TransitionSound;
        public bool isFly, isBoss;
        public static float moveSpeedMultiplier = 1.0f;
        public int CurrentCollection { get; set; }
        public Action CollectionHandler;

        private void Start()
        {
            CollectionHandler += IncreaseGameSpeed;
            playerPos = player.position;
            var distance = up.position - player.position;
            baseMovetime = distance.x / (baseMoveSpeed * moveSpeedMultiplier);
        }

        public void StartGame(int startTime = 0)
        {
            collections?.Clear();
            obstacles?.Clear();
            if (isFly)
            {
                isFly = false;
                SetFly(false);
            }
            isBoss = false;
            CurrentCollection = 0;
            foreach (var collection in GamePlayManager.Instance.fumenData?.collections?
                .Where(collection => collection.time >= startTime / 1000.0f))
            {
                StartCoroutine(CreateNote(collection));
            }

            foreach (Obstacle obstacle in GamePlayManager.Instance.fumenData?.obstacles?
                .Where(collection => collection.time >= startTime / 1000.0f))
            {
                StartCoroutine(CreateNote(obstacle));
            }

            foreach (Composition composition in GamePlayManager.Instance.fumenData?.compositions?
                .Where(composition => composition.time >= startTime / 1000.0f))
            {
                composition.isFinished = false;
                StartCoroutine(CreateComposition(composition));
            }
        }

        private void Update()
        {
            if (!GamePlayManager.Instance.isPlaying) return;
            foreach (Composition composition in GamePlayManager.Instance.fumenData?.compositions?
                .Where(composition => composition.isFinished == false && GamePlayManager.Instance.CurrentTime >= composition.time))
            {
                switch (composition?.compositionType)
                {
                    case CompositionType.Fly:
                        Debug.LogWarning("SetFlyComposition");
                        SetFly(composition.para == 1);
                        break;
                    case CompositionType.Boss:
                        isBoss = composition.para == 1;
                        break;
                    case CompositionType.Tutorial:
                        SetTutorial(composition.para);
                        break;
                    case CompositionType.Credit:
                        SetCredits(composition.para);
                        break;
                    default:
                        break;
                }
                composition.isFinished = true;
            }
        }

        private void SetTutorial(uint para)
        {
            if (para == 0)
            {
                HideTutorial();
            }
            else
            {
                ShowTutorial((int)para);
            }
        }

        private void SetCredits(uint para)
        {
            ShowCredits((int)para);
        }

        private void ShowTutorial(int id)
        {
            UIManager.Instance.CreatePanel(PanelEnum.Tutorial, new TutorialPanel.Option()
            {
                tutorialStruct = TableManager.Instance.GetTutorialInfo(id),
            });
        }

        private void HideTutorial()
        {
            UIManager.Instance.DestroyPanel(PanelEnum.Tutorial);
        }

        private void ShowCredits(int id)
        {
            UIManager.Instance.CreatePanel(PanelEnum.CreditsInfo, new CreditsInfoPanel.Option()
            {
                id = id,
            });
        }

        private IEnumerator CreateComposition(Composition composition)
        {
            yield return new WaitForSeconds(composition.time - GamePlayManager.StartTime / 1000.0f);

            switch (composition.compositionType)
            {
                case CompositionType.Fly:
                    isFly = composition.para == 1;
                    break;
                case CompositionType.Boss:
                    isBoss = composition.para == 1;
                    break;
                case CompositionType.Tutorial:
                    break;
                case CompositionType.Credit:
                    break;
                default:
                    break;
            }
        }

        private void SetFly(bool canFly)
        {
            if (canFly)
            {
                FlyManager.Instance.FlyToTop();
            }
            else
            {
                FlyManager.Instance.BackToBottom();
            }
        }

        private IEnumerator CreateNote(BaseFieldObject fieldObject)
        {
            yield return new WaitForSeconds(fieldObject.time - GamePlayManager.StartTime / 1000.0f);
            var upPos = up.position + (isFly ? new Vector3(0, 1, 0) : Vector3.zero);
            var delta = up.position - down.position;
            var skyPos = up.position + Vector3.up * (delta.y + 3);

            Vector3 endDown = new(playerPos.x, down.position.y, playerPos.z);
            Vector3 endUp = new(playerPos.x, upPos.y, playerPos.z);
            Vector3 endSky = new(playerPos.x, skyPos.y, playerPos.z);

            Vector3 startPos = new(), endPos = new();

            switch (fieldObject.positionType)
            {
                case PositionType.Normal:
                    startPos = down.position;
                    endPos = endDown;
                    break;
                case PositionType.Up:
                    startPos = upPos;
                    endPos = endUp;
                    break;
                case PositionType.Sky:
                    startPos = skyPos;
                    endPos = endSky;
                    break;
            }
            startPos += isFly ? new Vector3(0, 30, 0) : Vector3.zero;
            endPos += isFly ? new Vector3(0, 30, 0) : Vector3.zero;
            //Debug.Log(startPos);
            //Debug.Log(endPos);

            if (fieldObject.GetType().BaseType == typeof(Collection))
            {
                CreateCollection((Collection)fieldObject, startPos, endPos);
            }
            else if (fieldObject.GetType().BaseType == typeof(Obstacle))
            {
                CreateObstacle((Obstacle)fieldObject, startPos, endPos);
            }
        }

        private void CreateCollection(Collection collection, Vector3 startPos, Vector3 endPos)
        {
            Objects.BaseCollection obj = null;
            switch (collection.type)
            {
                case CollectionType.Brick:
                    obj = Instantiate(fieldObjectsAsset.fieldObjectsDic["Brick"], startPos, Quaternion.identity).GetComponent<Objects.Brick>();
                    break;
            }
            if (obj != null)
            {
                obj.data = collection;
                obj.startPos = startPos;
                obj.endPos = endPos;
                collections.Add(obj);
            }
        }

        private void CreateObstacle(Obstacle obstacle, Vector3 startPos, Vector3 endPos)
        {
            Objects.BaseObstacle obj = null;
            switch (obstacle.type)
            {
                case ObstacleType.Jump:
                    obj = Instantiate(fieldObjectsAsset.fieldObjectsDic["Jump"], startPos, Quaternion.identity).GetComponent<Objects.JumpObstacle>();
                    break;
                case ObstacleType.Slide:
                    obj = Instantiate(fieldObjectsAsset.fieldObjectsDic["Slide"], startPos, Quaternion.identity).GetComponent<Objects.SlideObstacle>();
                    break;
                case ObstacleType.Kick:
                    obj = Instantiate(fieldObjectsAsset.fieldObjectsDic["Kick"], startPos, Quaternion.identity).GetComponent<Objects.KickObstacle>();
                    break;
                case ObstacleType.Lantern:
                    obj = Instantiate(fieldObjectsAsset.fieldObjectsDic["Lantern"], startPos, Quaternion.identity).GetComponent<Objects.LanternObstacle>();
                    break;
                case ObstacleType.Missile:
                    obj = Instantiate(fieldObjectsAsset.fieldObjectsDic["Missile"], startPos, Quaternion.identity).GetComponent<Objects.MissileObstacle>();
                    break;
                case ObstacleType.Reverse:
                    obj = Instantiate(fieldObjectsAsset.fieldObjectsDic["Reverse"], startPos, Quaternion.identity).GetComponent<Objects.ReverseObstacle>();
                    break;
            }
            if (obj != null)
            {
                obj.data = obstacle;
                obj.startPos = startPos;
                obj.endPos = endPos;
                obstacles.Add(obj);
            }
        }

        public void PlayCollectionSound()
        {
            collectionSound.volume = SaveManager.Instance.GetSEVolume();
            collectionSound.Play();
        }

        public void PlayKickSound()
        {
            kickSound.volume = SaveManager.Instance.GetSEVolume();
            kickSound.Play();
        }

        public void PlayJumpSound()
        {
            jumpSound.volume = SaveManager.Instance.GetSEVolume();
            jumpSound.Play();
        }

        public void PlayHurtSound()
        {
            HurtSound.volume = SaveManager.Instance.GetSEVolume();
            HurtSound.Play();
        }

        public void PlayTransitionSound()
        {
            TransitionSound.volume = SaveManager.Instance.GetSEVolume();
            TransitionSound.Play();
        }

        public void StopGenerateNotes()
        {
            StopAllCoroutines();
            foreach (var item in collections)
            {
                if (item != null && item.gameObject != null)
                {
                    Destroy(item.gameObject);
                }
            }
            collections.Clear();
            foreach (var item in obstacles)
            {
                if (item != null && item.gameObject != null)
                {
                    Destroy(item.gameObject);
                }
            }
            obstacles.Clear();
        }

        private void IncreaseGameSpeed()
        {
            moveSpeedMultiplier *= 1.003f;
            var distance = up.position - playerPos;
            baseMovetime = distance.x / (baseMoveSpeed * moveSpeedMultiplier);
        }

        public void DecreaseGameSpeed()
        {
            moveSpeedMultiplier = 1f;
            var distance = up.position - playerPos;
            baseMovetime = distance.x / (baseMoveSpeed * moveSpeedMultiplier);

        }

        public int GetTotalCollections() => GamePlayManager.Instance.fumenData.collections.Count;

    }
}