using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Input
    private InputSystem_Actions _inputSystemActions;
    private Vector2 _input;
    private Vector2 _cameraInput;

    // Character
    private CharacterController _characterController;

    // Movement
    private Vector3 _movementDirection;
    private float _velocity = 5.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _inputSystemActions = new InputSystem_Actions();
        _inputSystemActions.Player.Enable();

        _characterController = GetComponent<CharacterController>();
    }

    private void OnDisable()
    {
        _inputSystemActions.Player.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        GatherInputMovement();
        UpdateMovement();
    }

    private void GatherInputMovement()
    {
        _input = _inputSystemActions.Player.Move.ReadValue<Vector2>();
        _cameraInput = _inputSystemActions.Player.Look.ReadValue<Vector2>();
    }

    private void UpdateMovement()
    {
        _movementDirection = transform.right * _input.x + transform.forward * _input.y;
        _characterController.Move(_movementDirection * _velocity * Time.deltaTime);
    }
}
