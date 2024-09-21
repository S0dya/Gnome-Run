using System.Collections.Generic;

public static class Settings
{
    public static PlatformType CurrentPlatformType = PlatformType.Yandex;
    public enum PlatformType { Mobile = 0, WebGL = 1, Yandex = 2 }

    public static bool IsMobileDevice;

    //game
    public static int MoneyAmount;

    //level
    public static int CurrentLevel;
    public static int CompleteLevelCount;
    public static int LastLevelIndex;
    public static int CurrentLocation;
    public static int CurrentAttempt;

    //shop
    public static int CurCharacterI = 0;
    public static List<int> ShopUnlockedCharacters = new() { 0 };

    //settings
    public static bool HasVibration = true;
    public static bool HasSound = true;
    public static int LanguageIndex = -1;


    //game
    public const string MoneyAmount_Key = "Money Amount";

    //level
    public const string CurrentLevel_Key = "Current Level";
    public const string CompleteLevelCount_Key = "Complete Lvl Count";
    public const string LastLevelIndex_Key = "Last Level Index";
    public const string CurrentLocation_Key = "Current Location Index";
    public const string CurrentAttempt_Key = "Current Attempt";

    //settings
    public const string HasVibration_Key = "Has Vibration";
    public const string HasSound_Key = "Has Sound";
    public const string LanguageIndex_Key = "Language Index";

    //shop
    public const string CurCharacterI_Key = "Current Character Index";
    public const string ShopUnlockedCharacters_Key = "Shop Unlocked Characters";
}
