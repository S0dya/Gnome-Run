using UnityEngine;
using UnityEngine.UI;

public class UIShopCharacter : MonoBehaviour
{
    [SerializeField] private Image LockedImage;

    [SerializeField] private Image ChosenImage;
    [SerializeField] private Sprite[] ChosenSprites;
    [SerializeField] private GameObject SelectedCharacterMarkObj;

    public void UnlockCharacter()
    {
        LockedImage.enabled = false;
    }

    public void SelectCharacter()
    {
        ToggleSelection(true);
    }
    public void DeselectCharacter()
    {
        ToggleSelection(false);
    }

    private void ToggleSelection(bool selected)
    {
        ChosenImage.sprite = ChosenSprites[selected ? 0 : 1];
        SelectedCharacterMarkObj.SetActive(selected);
    }
}
