using UnityEngine;

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

    private protected int _curCharacterRelatedObjI = -1;

    public int GetInfluenceValue() => playerMoneyInfluenceValue;

    public virtual void OnInteracted()
    {
        if (destroyOnInteracted) Destroy(gameObject);
    }

    public virtual void CharacterChanged(int characterIndex)
    {
        if (_curCharacterRelatedObjI != -1) characterRelatedObjs[_curCharacterRelatedObjI].SetActive(false);
        _curCharacterRelatedObjI = characterIndex;
        characterRelatedObjs[_curCharacterRelatedObjI].SetActive(true);
    }

    public virtual void ToggleDistanceToggleObj(bool toggle) => distanceToggleObj.SetActive(toggle);
}
