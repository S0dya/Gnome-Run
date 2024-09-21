using UnityEngine;
using YG;

public interface IInteractable
{
    public int GetInfluenceValue();
    public void OnInteracted();
}

public class Interactable : MonoBehaviour, IInteractable, ICharacterRelatedChange
{
    [Header("Interactable")]
    [SerializeField] private int playerMoneyInfluenceValue;

    [SerializeField] private bool destroyOnInteracted;

    [SerializeField] private GameObject distanceToggleObj;

    [Header("Character related details")]
    [SerializeField] private GameObject[] characterRelatedObjs;

    [Header("Additional")]
    [SerializeField] private bool setsPlayerMoneyInfluence;
    [SerializeField] private int playerMoneyInfluenceValueMin, playerMoneyInfluenceValueMax;

    private protected int _curCharacterRelatedObjI = -1;

    public int GetInfluenceValue() => playerMoneyInfluenceValue;

    public virtual void OnInteracted()
    {
        if (destroyOnInteracted) Destroy(gameObject);
    }

    public void SetPlayerInfluenceValue()
    {
        if (setsPlayerMoneyInfluence && playerMoneyInfluenceValueMin != 0 && playerMoneyInfluenceValueMin != 0)
            playerMoneyInfluenceValue = Random.Range(playerMoneyInfluenceValueMin, playerMoneyInfluenceValueMax);
    }

    public virtual void CharacterChanged(int characterIndex)
    {
        if (_curCharacterRelatedObjI != -1) characterRelatedObjs[_curCharacterRelatedObjI].SetActive(false);
        _curCharacterRelatedObjI = characterIndex;
        characterRelatedObjs[_curCharacterRelatedObjI].SetActive(true);
    }

    public virtual void ToggleDistanceToggleObj(bool toggle) => distanceToggleObj.SetActive(toggle);
}
