using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

#if UNITY_WEBGL
using YG;
#endif

public class LanguageManager : MonoBehaviour
{
    [SerializeField] private string[] languages = new string[] { "en", "ru" };
    [SerializeField] private LocalizationTable localizationTable;

    [SerializeField] private AdditionalKVLocalization[] additionalKVLocalizations;

    private Coroutine _changeLanguageCoroutine;

    public void Init()
    {
        if (Settings.LanguageIndex == -1)
        {
#if UNITY_WEBGL
            if (Settings.CurrentPlatformType == Settings.PlatformType.Yandex)
            {
                YandexGame.LanguageRequest();

                if (Array.Exists(languages, x => x == YandexGame.lang)) Settings.LanguageIndex = Array.IndexOf(languages, YandexGame.lang);
                else Settings.LanguageIndex = 0;
            }
            else Settings.LanguageIndex = 0;
#endif
#if UNITY_ANDROID || UNITY_IOS
            string languageCode = Application.systemLanguage == SystemLanguage.Russian ? "ru" : "en";
            Settings.LanguageIndex = Array.IndexOf(languages, languageCode);
#endif
        }
    }

    public bool ChangeLanguageIfPossible(int i)
    {
        bool canChangeLanguage = _changeLanguageCoroutine == null;

        if (canChangeLanguage)
        {
            Settings.LanguageIndex = i;

            _changeLanguageCoroutine = StartCoroutine(ChangeLanguageCoroutine(Settings.LanguageIndex));
        }

        return canChangeLanguage;
    }

    public string GetLocalizedString(string key)
    {
        return additionalKVLocalizations.First(x => x.LocalizationKey == key).LocalizationValues[Settings.LanguageIndex];
    }

    IEnumerator ChangeLanguageCoroutine(int localeId)
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeId];

        _changeLanguageCoroutine = null;
        Observer.OnHandleEvent(EventEnum.LanguageChanged);
    }


    [System.Serializable]
    class AdditionalKVLocalization
    {
        [SerializeField] public string LocalizationKey;
        [SerializeField] public string[] LocalizationValues;
    }
}

