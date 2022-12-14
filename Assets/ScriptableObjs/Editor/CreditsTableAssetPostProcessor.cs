using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
public class CreditsTableAssetPostprocessor : AssetPostprocessor 
{
    private static readonly string filePath = "Assets/ScriptableObjs/TextTable.xlsx";
    private static readonly string assetFilePath = "Assets/ScriptableObjs/CreditsTable.asset";
    private static readonly string sheetName = "CreditsTable";
    
    static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets) 
        {
            if (!filePath.Equals (asset))
                continue;
                
            CreditsTable data = (CreditsTable)AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(CreditsTable));
            if (data == null) {
                data = ScriptableObject.CreateInstance<CreditsTable> ();
                data.SheetName = filePath;
                data.WorksheetName = sheetName;
                AssetDatabase.CreateAsset ((ScriptableObject)data, assetFilePath);
                //data.hideFlags = HideFlags.NotEditable;
            }
            
            //data.dataArray = new ExcelQuery(filePath, sheetName).Deserialize<CreditsTableData>().ToArray();		

            //ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
            //EditorUtility.SetDirty (obj);

            ExcelQuery query = new ExcelQuery(filePath, sheetName);
            if (query != null && query.IsValid())
            {
                data.dataArray = query.Deserialize<CreditsTableData>().ToArray();
                data.dataList = query.Deserialize<CreditsTableData>();
                ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
                EditorUtility.SetDirty (obj);
            }
        }
    }
}
