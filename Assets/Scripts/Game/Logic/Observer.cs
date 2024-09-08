
public enum EventEnum
{
    LevelStarted, 
    LevelFinishedVictory,
    LevelFinishedGameover,
    LevelRestarted,
    
    ShopOpened,
    ShopClosed,

    LanguageChanged,
    AdOpened,
    AdClosed,
}

public static class Observer
{
    public delegate void EventHandler(EventEnum eventEnum);   
    public static event EventHandler OnEvent;

    public static void OnHandleEvent(EventEnum eventEnum) => OnEvent?.Invoke(eventEnum);
}

