using UnityEngine;

public interface IInteractable
{
    public int GetInfluenceValue();
    public void OnInteracted();
}

public class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField] private int playerMoneyInfluenceValue;

    [SerializeField] private bool destroyOnInteracted;

    public int GetInfluenceValue()
    {
        return playerMoneyInfluenceValue;
    }

    public void OnInteracted()
    {
        if (destroyOnInteracted)
            Destroy(gameObject);
    }
}
