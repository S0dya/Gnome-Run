using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 4;
    [SerializeField] private float movementBoundsRange = 3;
    [SerializeField] private float movementInputSensitivity = 0.1f;
    [SerializeField] private float movementLerpSensitivity = 1;

    [SerializeField] private float rotationSpeed = 5;
    [SerializeField] private float rotationBoundsRange = 30;
    [SerializeField] private float rotationLerpSensitivity = 10;

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
            _movementDirection = Mathf.Lerp(_movementDirection, 0, movementLerpSensitivity * Time.deltaTime);
        }

        Vector3 newPosition = transform.position + new Vector3(_movementDirection, 0, 1) * movementSpeed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, -movementBoundsRange, movementBoundsRange);

        transform.position = newPosition;

        Quaternion targetRotation = Quaternion.Euler(0, Mathf.Clamp(_movementDirection * rotationSpeed, -rotationBoundsRange, rotationBoundsRange), 0);
        CharacterTransform.rotation = Quaternion.Lerp(CharacterTransform.rotation, targetRotation, rotationLerpSensitivity * Time.deltaTime);
    }

    public void Move(Vector2 direction)
    {
        _movementDirection = direction.x * movementInputSensitivity;
    }
}
