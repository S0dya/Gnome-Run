using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PlatformRelated
{
    public class PlatformToggler : PlatformChanger
    {
        [SerializeField] private GameObject[] mobilePlatformObjects;
        [SerializeField] private GameObject[] webPlatformObjects;

        [SerializeField] private AdditionalPlatformAction[] additionalPlatformActions;

        private void Awake()
        {
            bool isMobile = Settings.CurrentPlatformType == Settings.PlatformType.Mobile
                || (Settings.CurrentPlatformType == Settings.PlatformType.Yandex && Settings.IsMobileDevice);

            foreach (var obj in mobilePlatformObjects) obj.SetActive(isMobile);
            foreach (var obj in webPlatformObjects) obj.SetActive(!isMobile);

            additionalPlatformActions?
                .Where(apa => apa.CurrentPlatformType == Settings.CurrentPlatformType).ToList()
                .ForEach(apa => apa.toggleEvent?.Invoke());
        }
    }

    [System.Serializable]
    public class PlatformImageSpritesToggle
    {
        [SerializeField] public Image image;
        [SerializeField] public Sprite mobileSprite;
        [SerializeField] public Sprite webSprite;
    }

    [System.Serializable]
    public class AdditionalPlatformAction
    {
        [SerializeField] public Settings.PlatformType CurrentPlatformType;
        [SerializeField] public UnityEvent toggleEvent;
    }
}
