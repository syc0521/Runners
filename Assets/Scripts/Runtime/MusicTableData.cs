using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class MusicTableData
{
  [SerializeField]
  int id;
  public int ID { get {return id; } set { id = value;} }
  
  [SerializeField]
  int musicid;
  public int Musicid { get {return musicid; } set { musicid = value;} }
  
  [SerializeField]
  int nameid;
  public int Nameid { get {return nameid; } set { nameid = value;} }
  
  [SerializeField]
  int descid;
  public int Descid { get {return descid; } set { descid = value;} }
  
  [SerializeField]
  int[] unlockcondition = new int[0];
  public int[] Unlockcondition { get {return unlockcondition; } set { unlockcondition = value;} }
  
}