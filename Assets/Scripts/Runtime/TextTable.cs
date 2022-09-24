using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

///
/// !!! Machine generated code !!!
///
/// A class which deriveds ScritableObject class so all its data 
/// can be serialized onto an asset data file.
/// 
[System.Serializable]
public class TextTable : ScriptableObject 
{	
    [HideInInspector] [SerializeField] 
    public string SheetName = "";
    
    [HideInInspector] [SerializeField] 
    public string WorksheetName = "";
    
    // Note: initialize in OnEnable() not here.
    public TextTableData[] dataArray;
    public List<TextTableData> dataList;
    public Dictionary<int, string> textDic = new();

    void OnEnable()
    {		
//#if UNITY_EDITOR
        //hideFlags = HideFlags.DontSave;
//#endif
        // Important:
        //    It should be checked an initialization of any collection data before it is initialized.
        //    Without this check, the array collection which already has its data get to be null 
        //    because OnEnable is called whenever Unity builds.
        // 		
        if (dataArray == null)
            dataArray = new TextTableData[0];
        foreach (var item in dataList)
        {
            if (!textDic.ContainsKey(item.ID))
            {
                textDic.Add(item.ID, item.Text);
            }
        }

    }

    //
    // Highly recommand to use LINQ to query the data sources.
    //

}
