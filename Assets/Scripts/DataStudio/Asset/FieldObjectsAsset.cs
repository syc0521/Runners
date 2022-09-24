using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.DataStudio.Asset
{
    [CreateAssetMenu(fileName = "FieldObjectsTable", menuName = "ScriptableObjs/FieldObjectsTable")]
    public class FieldObjectsAsset : ScriptableObject
    {
        public Dictionary<string, GameObject> editorFieldObjectsDic = new();
        public Dictionary<string, GameObject> fieldObjectsDic = new();


        [System.Serializable]
        public struct EditorFieldObjectsStruct
        {
            public string key;
            public GameObject objectPrefab;
        }

        [System.Serializable]
        public struct FieldObjectsStruct
        {
            public string key;
            public GameObject objectPrefab;
        }

        public EditorFieldObjectsStruct[] editorFieldObjects;
        public FieldObjectsStruct[] fieldObjects;

        private void OnEnable()
        {
            foreach (var item in editorFieldObjects)
            {
                if (!editorFieldObjectsDic.ContainsKey(item.key))
                {
                    editorFieldObjectsDic.Add(item.key, item.objectPrefab);
                }
            }
            foreach (var item in fieldObjects)
            {
                if (!fieldObjectsDic.ContainsKey(item.key))
                {
                    fieldObjectsDic.Add(item.key, item.objectPrefab);
                }
            }
        }
    }

}
