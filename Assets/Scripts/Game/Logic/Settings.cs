
using System.Collections.Generic;

public static class Settings
{
    
    //game
    public static int MoneyAmount;

    //level
    public static int CurrentLevel;
    public static int CompleteLevelCount;
    public static int LastLevelIndex;
    public static int CurrentLocation;
    public static int CurrentAttempt;

    //shop
    public static int SetCharacterI;
    public static List<int> ShopUnlockedCharacters = new();

}
