using System.Collections.Generic;
using UnityEngine;

namespace Saving
{
    public class PrefsSave : ISaveSystem
    {
        public void Save(GameData data)
        {
            foreach (var intKvp in data.IntDict)
            {
                PlayerPrefs.SetInt(intKvp.Key, intKvp.Value);
            }
            
            foreach (var boolKvp in data.BoolDict)
            {
                PlayerPrefs.SetInt(boolKvp.Key, boolKvp.Value ? 1 : 0);
            }

            foreach (var intsKvp in data.IntsDict)
            {
                for (int i = 0; i < intsKvp.Value.Length; i++)
                {
                    PlayerPrefs.SetInt(intsKvp.Key + i.ToString(), intsKvp.Value[i]);
                }
            }
        }

        public GameData Load()
        {
            if (!PlayerPrefs.HasKey(Settings.MoneyAmount_Key)) return null;
            
            var gameData = new GameData();

            //game
            gameData.IntDict.Add(Settings.MoneyAmount_Key, PlayerPrefs.GetInt(Settings.MoneyAmount_Key));

            //level
            gameData.IntDict.Add(Settings.CurrentLevel_Key, PlayerPrefs.GetInt(Settings.CurrentLevel_Key));
            gameData.IntDict.Add(Settings.CompleteLevelCount_Key, PlayerPrefs.GetInt(Settings.CompleteLevelCount_Key));
            gameData.IntDict.Add(Settings.LastLevelIndex_Key, PlayerPrefs.GetInt(Settings.LastLevelIndex_Key));
            gameData.IntDict.Add(Settings.CurrentLocation_Key, PlayerPrefs.GetInt(Settings.CurrentLocation_Key));
            gameData.IntDict.Add(Settings.CurrentAttempt_Key, PlayerPrefs.GetInt(Settings.CurrentAttempt_Key));

            //settings
            gameData.BoolDict.Add(Settings.HasVibration_Key, PlayerPrefs.GetInt(Settings.HasVibration_Key) == 1);
            gameData.BoolDict.Add(Settings.HasSound_Key, PlayerPrefs.GetInt(Settings.HasSound_Key) == 1);
            gameData.IntDict.Add(Settings.LanguageIndex_Key, PlayerPrefs.GetInt(Settings.LanguageIndex_Key));

            //shop
            gameData.IntDict.Add(Settings.CurCharacterI_Key, PlayerPrefs.GetInt(Settings.CurCharacterI_Key));
            gameData.IntsDict.Add(Settings.ShopUnlockedCharacters_Key, GetArray(Settings.ShopUnlockedCharacters_Key));

            return gameData;
        }

        int[] GetArray(string key)
        {
            var intList = new List<int>();
            int i = 0;
            while (true)
            {
                if (!PlayerPrefs.HasKey(key + i.ToString())) break;

                int val = PlayerPrefs.GetInt(key + i.ToString());

                intList.Add(val);
                i++;
            }

            return intList.ToArray();
        }
    }
}
