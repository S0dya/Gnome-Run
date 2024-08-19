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


        //game
        private const string MoneyAmount_Key = "Money Amount";

        //level
        private const string CurrentLevel_Key = "Current Level";
        private const string CompleteLevelCount_Key = "Complete Lvl Count";
        private const string LastLevelIndex_Key = "Last Level Index";
        private const string CurrentLocation_Key = "Current Location Index";
        private const string CurrentAttempt_Key = "Current Attempt";

        //settings
        private const string HasVibration_Key = "Has Vibration";
        private const string HasSound_Key = "Has Sound";

        //shop
        private const string CurCharacterI_Key = "Current Character Index";
        private const string ShopUnlockedCharacters_Key = "Shop Unlocked Characters";


        private ISaveSystem _saveSystem;

        private LevelManager _levelManager;

        [Inject]
        public void Construct(LevelManager levelManager)
        {
            _levelManager = levelManager;
        }

        public void Init()
        {
#if UNITY_ANDROID || UNITY_IOS || UNITY_TVOS
            _saveSystem = new JsonSave();
#else
            _saveSystem = new YandexSave();
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

        public void Save() => _saveSystem.Save(GetGameData());
        public void Load() => SetGameData(_saveSystem.Load());

        private GameData GetGameData()
        {
            GameData gameData = new();

            //game
            gameData.IntDict.Add(MoneyAmount_Key, Settings.MoneyAmount);

            //level
            gameData.IntDict.Add(CurrentLevel_Key, Settings.CurrentLevel);
            gameData.IntDict.Add(CompleteLevelCount_Key, Settings.CompleteLevelCount);
            gameData.IntDict.Add(LastLevelIndex_Key, _levelManager.CurrentLevelIndex);
            gameData.IntDict.Add(CurrentLocation_Key, Settings.CurrentLocation);
            gameData.IntDict.Add(CurrentAttempt_Key, Settings.CurrentAttempt);

            //settings
            gameData.boolDict.Add(HasVibration_Key, Settings.HasVibration);
            gameData.boolDict.Add(HasSound_Key, Settings.HasSound);

            //shop
            gameData.IntDict.Add(CurCharacterI_Key, Settings.CurCharacterI);
            gameData.IntsDict.Add(ShopUnlockedCharacters_Key, Settings.ShopUnlockedCharacters.ToArray());

            return gameData;
        }
        private void SetGameData(GameData gameData)
        {
            if (gameData == null) return;

            //game
            gameData.IntDict.TryGetValue(MoneyAmount_Key, out Settings.MoneyAmount);

            //level
            gameData.IntDict.TryGetValue(CurrentLevel_Key, out Settings.CurrentLevel);
            gameData.IntDict.TryGetValue(CompleteLevelCount_Key, out Settings.CompleteLevelCount);
            gameData.IntDict.TryGetValue(LastLevelIndex_Key, out Settings.LastLevelIndex);
            gameData.IntDict.TryGetValue(CurrentLocation_Key, out Settings.CurrentLocation);
            gameData.IntDict.TryGetValue(CurrentAttempt_Key, out Settings.CurrentAttempt);

            //settings
            gameData.boolDict.TryGetValue(HasVibration_Key, out Settings.HasVibration);
            gameData.boolDict.TryGetValue(HasSound_Key, out Settings.HasSound);

            //shop
            gameData.IntDict.TryGetValue(CurCharacterI_Key, out Settings.CurCharacterI);
            if (gameData.IntsDict.TryGetValue(ShopUnlockedCharacters_Key, out int[] shopUnlockedCharactersArray))
                Settings.ShopUnlockedCharacters = shopUnlockedCharactersArray.ToList();
        }
    }
}
