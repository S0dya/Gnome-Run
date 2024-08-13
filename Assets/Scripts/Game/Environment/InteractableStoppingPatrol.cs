using UnityEngine;

public class InteractableStoppingPatrol : InteractableStopping, IInteractable
{
    [Header("Settings")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private Vector3 movementDirection;

    [Header("Settings")]
    [SerializeField] private Animator animator;

    private Vector3[] _targetPositions;
    private Vector3 _curTargetPosition;
    private int _curTargetPositionI;

    private Vector3 _directionToTarget;

    //hash
    private int _animatorIDIdle;

    private void Awake()
    {
        _animatorIDIdle = Animator.StringToHash("Idle");
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

    public override void OnInteracted()
    {
        base.OnInteracted();

        this.enabled = false;

        transform.rotation = Quaternion.LookRotation(Vector3.zero);

        animator?.Play(_animatorIDIdle);
    }
}