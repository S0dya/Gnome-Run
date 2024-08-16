using UnityEngine;

public class PlayerDistanceToggle : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        other.GetComponent<DistanceToggleEvent>()?.InvokeEnter();
    }
    public void OnTriggerExit(Collider other)
    {
        if (other != null && other.gameObject != null) other.GetComponent<DistanceToggleEvent>()?.InvokeExit();
    }
}