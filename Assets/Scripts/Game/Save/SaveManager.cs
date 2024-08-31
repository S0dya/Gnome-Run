using AdsSystem;
using System.Linq;
using UnityEngine;
#if UNITY_WEBGL
using YG;
#endif
using Zenject;

namespace Saving
{
    public class SaveManager : MonoBehaviour
    {

        private ISaveSystem _saveSystem;

        private LevelManager _levelManager;

        [Inject]
        public void Construct(LevelManager levelManager)
        {
            _levelManager = levelManager;
        }

        public void Init()
        {
#if UNITY_WEBGL
            _saveSystem = new YandexSave();
#else
            _saveSystem = new PrefsSave();
      
#endif

            Load();
        }
#if UNITY_WEBGL
        private void OnEnable()
        {
            if (YandexGame.SDKEnabled) YandexGame.GetDataEvent += Load;
        }
        private void OnDisable()
        {
            if (YandexGame.SDKEnabled) YandexGame.GetDataEvent -= Load;
        }
#endif

        private void OnDestroy() => Save();
        private void OnApplicationQuit() => Save();
        void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus) Save();
        }
        void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus) Save();
        }


        public void Save() => _saveSystem.Save(GetGameData());
        public void Load() => SetGameData(_saveSystem.Load());

        private GameData GetGameData()
        {
            GameData gameData = new();

            //game
            gameData.IntDict.Add(Settings.MoneyAmount_Key, Settings.MoneyAmount);

            //level
            gameData.IntDict.Add(Settings.CurrentLevel_Key, Settings.CurrentLevel);
            gameData.IntDict.Add(Settings.CompleteLevelCount_Key, Settings.CompleteLevelCount);
            gameData.IntDict.Add(Settings.LastLevelIndex_Key, _levelManager.CurrentLevelIndex);
            gameData.IntDict.Add(Settings.CurrentLocation_Key, Settings.CurrentLocation);
            gameData.IntDict.Add(Settings.CurrentAttempt_Key, Settings.CurrentAttempt);

            //settings
            gameData.BoolDict.Add(Settings.HasVibration_Key, Settings.HasVibration);
            gameData.BoolDict.Add(Settings.HasSound_Key, Settings.HasSound);
            gameData.IntDict.Add(Settings.LanguageIndex_Key, Settings.LanguageIndex);

            //shop
            gameData.IntDict.Add(Settings.CurCharacterI_Key, Settings.CurCharacterI);
            gameData.IntsDict.Add(Settings.ShopUnlockedCharacters_Key, Settings.ShopUnlockedCharacters.ToArray());

            return gameData;
        }
        private void SetGameData(GameData gameData)
        {
            if (gameData == null) return;

            //game
            gameData.IntDict.TryGetValue(Settings.MoneyAmount_Key, out Settings.MoneyAmount);

            //level
            gameData.IntDict.TryGetValue(Settings.CurrentLevel_Key, out Settings.CurrentLevel);
            gameData.IntDict.TryGetValue(Settings.CompleteLevelCount_Key, out Settings.CompleteLevelCount);
            gameData.IntDict.TryGetValue(Settings.LastLevelIndex_Key, out Settings.LastLevelIndex);
            gameData.IntDict.TryGetValue(Settings.CurrentLocation_Key, out Settings.CurrentLocation);
            gameData.IntDict.TryGetValue(Settings.CurrentAttempt_Key, out Settings.CurrentAttempt);

            //settings
            gameData.BoolDict.TryGetValue(Settings.HasVibration_Key, out Settings.HasVibration);
            gameData.BoolDict.TryGetValue(Settings.HasSound_Key, out Settings.HasSound);
            gameData.IntDict.TryGetValue(Settings.LanguageIndex_Key, out Settings.LanguageIndex);

            //shop
            gameData.IntDict.TryGetValue(Settings.CurCharacterI_Key, out Settings.CurCharacterI);
            if (gameData.IntsDict.TryGetValue(Settings.ShopUnlockedCharacters_Key, out int[] shopUnlockedCharactersArray))
                Settings.ShopUnlockedCharacters = shopUnlockedCharactersArray.ToList();
        }
    }
}
