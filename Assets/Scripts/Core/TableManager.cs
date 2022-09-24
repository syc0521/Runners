using Runner.DataStudio.Asset;
using Runner.DataStudio.Serialize;
using Runner.Utils;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Runner.Core
{
    /// <summary>
    /// 表管理器: TableManager
    /// Author: 单元琛 2022-8-26
    /// </summary>
    public class TableManager : Singleton<TableManager>
    {
        public TextTable gameTextAsset;
        public TutorialTable tutorialAsset;
        public MusicTable songList;
        public CreditsTable creditsAsset;

        public string GetText(int id)
        {
            return gameTextAsset.textDic[id]?? "";
        }

        public TutorialStruct GetTutorialInfo(int id)
        {
            return tutorialAsset.tutorialDic[id];
        }

        public CreditStruct GetCreditsInfo(int id)
        {
            return creditsAsset.creditsDic[id];
        }

        public List<MusicTableData> GetSongList() => songList.dataList;

        public MusicTableData GetSongData(int id)
        {
            return songList.dataList.Find(item => item.ID == id);
        }

    }

}