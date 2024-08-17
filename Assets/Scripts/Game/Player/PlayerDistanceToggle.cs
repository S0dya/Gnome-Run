using UnityEngine;

using DistanceToggle;

public class PlayerDistanceToggle : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        var distanceToggle = other.GetComponent<DistanceToggleEvent>();

        if (distanceToggle is DistanceToggleEventSwitch distanceToggleSwitch) distanceToggleSwitch.InvokeEnter();
        else distanceToggle.InvokeEnter();
    }
    public void OnTriggerExit(Collider other)
    {
        if (other == null && other.gameObject == null) return;

        var distanceToggle = other.GetComponent<DistanceToggleEvent>();

        if (distanceToggle is DistanceToggleEventSwitch distanceToggleSwitch) distanceToggleSwitch.InvokeExit();
        else distanceToggle.InvokeExit();
    }
}