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

[Serializable]
public struct CreditStruct
{
    public int titleid;
    public int descid;
}

[System.Serializable]
public class CreditsTable : ScriptableObject 
{	
    [HideInInspector] [SerializeField] 
    public string SheetName = "";
    
    [HideInInspector] [SerializeField] 
    public string WorksheetName = "";
    
    // Note: initialize in OnEnable() not here.
    public CreditsTableData[] dataArray;
    public List<CreditsTableData> dataList;
    public Dictionary<int, CreditStruct> creditsDic = new();
    
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
            dataArray = new CreditsTableData[0];
        foreach (var item in dataList)
        {
            if (!creditsDic.ContainsKey(item.ID))
            {
                var creditStruct = new CreditStruct
                {
                    titleid = item.Titleid,
                    descid = item.Descid
                };
                creditsDic.Add(item.ID, creditStruct);
            }
        }
    }
    
    //
    // Highly recommand to use LINQ to query the data sources.
    //

}
