using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
using YG;

public class LanguageManager : MonoBehaviour
{

    Coroutine _changeLanguageCoroutine;

    private void Start()
    {
        YandexGame.LanguageRequest();

        //YandexGame.lang;
    }

    public bool ChangeLanguageIfPossible(int i)
    {
        bool canChangeLanguage = _changeLanguageCoroutine == null;

        if (canChangeLanguage) _changeLanguageCoroutine = StartCoroutine(ChangeLanguageCoroutine(i));

        return canChangeLanguage;
    }

    IEnumerator ChangeLanguageCoroutine(int localeId)
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeId];

        _changeLanguageCoroutine = null;
    }
}
