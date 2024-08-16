using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using YG;

public class LanguageManager : MonoBehaviour
{
    [SerializeField] private string[] languages;
    [SerializeField] private LocalizationTable localizationTable;

    private Coroutine _changeLanguageCoroutine;

    private void Start()
    {
        if (Settings.LanguageIndex == -1)
        {
            YandexGame.LanguageRequest();
            Settings.LanguageIndex = Array.IndexOf(languages, YandexGame.lang);
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
        var stringTable = LocalizationSettings.StringDatabase.GetTable(localizationTable.TableCollectionName);

        return stringTable.GetEntry(key).GetLocalizedString();
    }

    IEnumerator ChangeLanguageCoroutine(int localeId)
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeId];

        _changeLanguageCoroutine = null;
        Observer.OnHandleEvent(EventEnum.LanguageChanged);
    }
}
