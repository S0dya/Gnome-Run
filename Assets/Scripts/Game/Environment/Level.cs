using UnityEngine;

namespace LevelsRelated
{
    public class Level : MonoBehaviour
    {
        [HideInInspector] public Transform PlayerSpawnPoint;
        [HideInInspector] public int MaxMoney;

        //[HideInInspector]
        [SerializeField] private Interactable[] _interactableCharacterRelatedDetails;

        public void InitInteractionDetails(Interactable[] interactableCharacterRelatedChanges)
        {
            _interactableCharacterRelatedDetails = interactableCharacterRelatedChanges;
        }

        public void SetInteractionDetails(int characterIndex)
        {
            ChangeDetails(_interactableCharacterRelatedDetails, characterIndex);
        }

        private void ChangeDetails(Interactable[] objectsToChange, int index)
        {
            foreach (var objectToChange in objectsToChange)
            {
                objectToChange.GetComponent<ICharacterRelatedChange>().CharacterChanged(index);
            }
        }
    }
}
