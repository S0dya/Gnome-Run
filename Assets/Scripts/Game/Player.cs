using System;
using UnityEngine;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    [SerializeField] private float MovementSpeed = 4;
    [SerializeField] private float MovementBoundsRange = 3;
    [SerializeField] private float MovementInputSensitivity = 0.1f;
    [SerializeField] private float MovementLerpSensitivity = 1;

    [SerializeField] private float RotationSpeed = 5;
    [SerializeField] private float RotationBoundsRange = 30;
    [SerializeField] private float RotationLerpSensitivity = 10;

    [SerializeField] private Transform CharacterTransform;

    private Inputs _inputs;
    private float _movementDirection;

    private void OnEnable()
    {
        _inputs = new Inputs();

        _inputs.InGame.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());

        _inputs.Enable();
    }

    private void OnDisable()
    {
        _inputs.InGame.Move.performed -= ctx => Move(ctx.ReadValue<Vector2>());

        _inputs.Disable();
    }

    private void Update()
    {
        if (Math.Abs(_movementDirection) > 0.01f)
        {
            _movementDirection = Mathf.Lerp(_movementDirection, 0, MovementLerpSensitivity * Time.deltaTime);
        }

        Vector3 newPosition = transform.position + new Vector3(_movementDirection, 0, 1) * MovementSpeed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, -MovementBoundsRange, MovementBoundsRange);

        transform.position = newPosition;

        Quaternion targetRotation = Quaternion.Euler(0, Mathf.Clamp(_movementDirection * RotationSpeed, -RotationBoundsRange, RotationBoundsRange), 0);
        CharacterTransform.rotation = Quaternion.Lerp(CharacterTransform.rotation, targetRotation, RotationLerpSensitivity * Time.deltaTime);
    }

    public void Move(Vector2 direction)
    {
        _movementDirection = direction.x * MovementInputSensitivity;
    }
}
