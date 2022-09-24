using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runner.Utils;

namespace Runner.GamePlay
{
    public class LoopSceneController : MonoBehaviour
    {
        private GameObject curGroup;
        private GameObject nextGroup;

        private int blockCount;
        private float groupWidth;
        private float blockPosInterval;
        private Vector3 genGroupPos;

        private Vector3 originPos;

        public float moveSpeed = 1f;

        public GameObject insertBlockPrefab;

        public GameObject[] blockPrefabs;

        private List<GameObject> groups;

        public bool pause { get; set; }
                
        void Start()
        {
            curGroup = transform.GetChild(0).gameObject;
            nextGroup= transform.GetChild(1).gameObject;            
            
            genGroupPos = nextGroup.transform.localPosition;
            blockPosInterval = nextGroup.transform.GetChild(0).localPosition.x;
            blockCount = nextGroup.transform.childCount;
            groupWidth = blockPosInterval * 2 * blockCount;

            originPos=curGroup.transform.localPosition;

            groups = new List<GameObject>();

            GenerateAllGroup();
        }

        void Update()
        {
            if (pause) return;

            if (Vector3.Magnitude(curGroup.transform.localPosition-originPos)>=groupWidth)
            {
                SwitchGroup();
            }   
            else
            {
                MoveGroup();
            }
        }

        private void SwitchGroup()
        {
            if(curGroup.name=="TempGroup1"||curGroup.name=="TempGroup2")
            {
                Destroy(curGroup);
            }
            else
            {
                curGroup.SetActive(false);
                curGroup.transform.localPosition = genGroupPos;
            }
            var tempGroup = GetGroup();
            curGroup = nextGroup;
            nextGroup = tempGroup;
            nextGroup.SetActive(true);
        }

        private void MoveGroup()
        {
            var dir = -transform.right;
            curGroup.transform.localPosition += moveSpeed * ObjectManager.moveSpeedMultiplier * Time.deltaTime * dir;
            nextGroup.transform.localPosition += moveSpeed * ObjectManager.moveSpeedMultiplier * Time.deltaTime * dir;
        }

        private GameObject GetGroup()
        {
            var randIndex = Random.Range(0, groups.Count);

            return groups[randIndex].activeInHierarchy == false ? groups[randIndex]:GetGroup();
        }

        private void GenerateAllGroup()
        {
            if (blockPrefabs == null || blockPrefabs.Length < blockCount) return;
            List<GameObject[]> permutationList = PermutationAndCombination<GameObject>.GetCombination(blockPrefabs,blockCount);
            
            for(int i=0;i<permutationList.Count;i++)
            {
                GameObject tempGroup = new GameObject("GenGroup" + i);
                tempGroup.transform.parent = transform;
                tempGroup.transform.localPosition = genGroupPos;
                tempGroup.transform.localRotation = Quaternion.identity;
                var tempPermutation = permutationList[i];
                var blockGenPos = tempGroup.transform.position+blockPosInterval*transform.right;
                for(int j = 0; j < tempPermutation.Length;j++)
                {
                    var block = Instantiate(tempPermutation[j], blockGenPos, Quaternion.identity,tempGroup.transform);
                    block.transform.localRotation = Quaternion.identity;
                    blockGenPos += 2 * blockPosInterval*transform.right;
                }
                //多生成一个防止错位露馅
                if(insertBlockPrefab!=null)
                {
                    var lastBlock = Instantiate(insertBlockPrefab, blockGenPos - blockPosInterval * transform.right, Quaternion.identity, tempGroup.transform);
                    lastBlock.transform.localRotation = Quaternion.identity;
                }                
                tempGroup.SetActive(false);
                groups.Add(tempGroup);
            }            
        }           
    }
}
