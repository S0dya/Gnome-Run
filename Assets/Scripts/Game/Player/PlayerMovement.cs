using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float movementSpeed = 4;
    [SerializeField] private float movementBoundsRange = 3;
    
    [Header("PC Settings")]
    [SerializeField] private float movementInputSensitivity = 0.1f;
    [SerializeField] private float movementLerpSensitivity = 1;
    
    [Header("Mobile Settings")]
    [SerializeField] private float mobileMovementInputSensitivity = 0.1f;
    [SerializeField] private float mobileMovementLerpSensitivity = 1;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 5;
    [SerializeField] private float rotationBoundsRange = 30;
    [SerializeField] private float rotationLerpSensitivity = 10;

    [Header("Other")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform characterTransform;

    private Transform _characterMeshesParentTransform;

    private Inputs _inputs;
    private float _movementDirection;

    //reusable
    private Vector3 _curMovementPosition;
    private Quaternion _curTargetRotation;

    private float _curMovementInputSensitivity;
    private float _curMovementLerpSensitivity;

    private void OnEnable()
    {
        _inputs = new Inputs();

        if (Settings.CurrentPlatformType == Settings.PlatformType.Mobile || Settings.IsMobileDevice)
        {
            _inputs.InGame.MobileMove.performed += ctx => Move(ctx.ReadValue<Vector2>());
        }
        else
        {
            _inputs.InGame.PCMove.performed += ctx => Move(ctx.ReadValue<Vector2>());
        }

        _inputs.Enable();
    }

    private void OnDisable()
    {
        if (Settings.CurrentPlatformType == Settings.PlatformType.Mobile || Settings.IsMobileDevice)
        {
            _inputs.InGame.MobileMove.performed -= ctx => Move(ctx.ReadValue<Vector2>());
        }
        else
        {
            _inputs.InGame.PCMove.performed -= ctx => Move(ctx.ReadValue<Vector2>());
        }

        _inputs.Disable();

        _characterMeshesParentTransform.rotation = _curTargetRotation = Quaternion.identity;
    }

    private void Start()
    {
        if (Settings.CurrentPlatformType == Settings.PlatformType.Mobile || Settings.IsMobileDevice)
        {
            _curMovementInputSensitivity = mobileMovementInputSensitivity;
            _curMovementLerpSensitivity = mobileMovementLerpSensitivity;
        }
        else
        {
            _curMovementInputSensitivity = movementInputSensitivity;
            _curMovementLerpSensitivity = movementLerpSensitivity; 
        }
    }

    private void Update()
    {
        _movementDirection = Math.Abs(_movementDirection) < 0.04f ? 0 
            : Mathf.Lerp(_movementDirection, 0, _curMovementLerpSensitivity * Time.deltaTime);

        _curMovementPosition = characterTransform.position + new Vector3(_movementDirection, 0, 1) * movementSpeed * Time.deltaTime;
        _curMovementPosition.x = Mathf.Clamp(_curMovementPosition.x, -movementBoundsRange, movementBoundsRange);

        characterController.Move(_curMovementPosition - characterTransform.position);

        _curTargetRotation = Quaternion.Euler(0, Mathf.Clamp(_movementDirection * rotationSpeed, -rotationBoundsRange, rotationBoundsRange), 0);
        _characterMeshesParentTransform.rotation = Quaternion.Lerp(_characterMeshesParentTransform.rotation, _curTargetRotation, rotationLerpSensitivity * Time.deltaTime);
    }

    public void SetNewCharacter(Transform transform) => _characterMeshesParentTransform = transform;

    public void MoveCharacter(Vector3 pos)
    {
        characterController.enabled = false;
        characterTransform.position = pos;
        characterController.enabled = true;
    }

    //input
    public void Move(Vector2 direction) => _movementDirection = direction.x * _curMovementInputSensitivity;
}
