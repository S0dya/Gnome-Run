using UnityEngine;

namespace DistanceToggle
{
    public class DistanceToggleEventSwitch : DistanceToggleEvent
    {
        [SerializeField] private GameObject objectToSwitch;

        public override void InvokeEnter()
        {
            base.InvokeEnter();

            objectToSwitch.SetActive(false);
        }
        public override void InvokeExit()
        {
            base.InvokeExit();

            objectToSwitch.SetActive(true);
        }
    }
}