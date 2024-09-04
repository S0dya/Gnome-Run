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
    [SerializeField] private string[] languages;
    [SerializeField] private LocalizationTable localizationTable;

    [SerializeField] private AdditionalKVLocalization[] additionalKVLocalizations;

    private Coroutine _changeLanguageCoroutine;

    private void Start()
    {
        if (Settings.LanguageIndex == -1)
        {
#if UNITY_WEBGL
            YandexGame.LanguageRequest();
            Settings.LanguageIndex = Array.IndexOf(languages, YandexGame.lang);
#endif
#if UNITY_ANDROID || UNITY_IOS
            string languageCode = Application.systemLanguage == SystemLanguage.Russian ? "ru" : "en";
            Settings.LanguageIndex = Array.IndexOf(languages, languageCode);
#endif
        }
    }

    public bool ChangeLanguageIfPossible()
    {
        bool canChangeLanguage = _changeLanguageCoroutine == null;

        if (canChangeLanguage)
        {
            Settings.LanguageIndex = Settings.LanguageIndex == languages.Length - 1 ? 0 : Settings.LanguageIndex + 1;

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

