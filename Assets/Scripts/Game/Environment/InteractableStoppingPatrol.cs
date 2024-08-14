using UnityEngine;

public interface IInteractablePatrol : IInteractable
{
    public void OnInteracted(Vector3 playerPosition);
}

public class InteractableStoppingPatrol : InteractableStopping, IInteractablePatrol, ICharacterRelatedChange
{
    [Header("Settings")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private Vector3 movementDirection;

    [Header("Character related details")]
    [SerializeField] private Animator[] characterRelatedAnimators;

    private Vector3[] _targetPositions;
    private Vector3 _curTargetPosition;
    private int _curTargetPositionI;

    private Vector3 _directionToTarget;

    //hash
    private int _animatorIDIdle;
    private int _animatorIDWalk;

    private void Awake()
    {
        _animatorIDIdle = Animator.StringToHash("Idle");
        _animatorIDWalk = Animator.StringToHash("Walk");
    }

    private void Start()
    {
        _targetPositions = new Vector3[2]
        {
            transform.position + movementDirection,
            transform.position - movementDirection
        };

        SetTargetIndexPositionDirectionRotation(Random.Range(0, _targetPositions.Length));
    }

    private void Update()
    {
        transform.Translate(_directionToTarget * Time.deltaTime * movementSpeed, Space.World);

        if (Vector3.Distance(transform.position, _curTargetPosition) < 0.5f)
            SetTargetIndexPositionDirectionRotation((_curTargetPositionI + 1) % _targetPositions.Length);
    }

    private void SetTargetIndexPositionDirectionRotation(int i)
    {
        _curTargetPositionI = i;
        _curTargetPosition = _targetPositions[_curTargetPositionI];

        _directionToTarget = (_curTargetPosition - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(_directionToTarget);
    }

    public void OnInteracted(Vector3 playerPosition)
    {
        OnInteracted();

        this.enabled = false;

        var directionToPlayer = (playerPosition - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));

        characterRelatedAnimators[_curCharacterRelatedObjI].Play(_animatorIDIdle);
    }
    public override void CharacterChanged(int characterIndex)
    {
        base.CharacterChanged(characterIndex);

        characterRelatedAnimators[_curCharacterRelatedObjI].Play(_animatorIDWalk);
    }
}