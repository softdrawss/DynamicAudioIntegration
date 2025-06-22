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
    private float _velocity = 8.0f;
    private bool _isMoving;
    private Vector3 _verticalVelocity;
    private float _gravity = -9.81f;
    private float _groundedYVelocity = -7f;
    private bool _isGrounded;

    // Camera
    public Transform cameraTransform;
    private float _mouseSensitivity = 1.0f;
    private float _xRotation = 0f;
    private float _mouseX, _mouseY;

    // Debug
   // public AudioSourceSystem audioSourceSystem;

    //// Audio Planes
    //public ZenithPlane zenithPlane;
    //public AzimuthPlane azimuthPlane;

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

    private void FixedUpdate()
    {
        if (_isMoving) 
        {
            AudioSystemManager.Instance.HandleAudioSystem();
        }
    }

    void Update()
    {
        if (!Menu.Instance.gamePaused)
        {
            GatherInputMovement();
            UpdateMovement();
            UpdateCameraLook();
        }
    }

    private void GatherInputMovement()
    {
        _input = _inputSystemActions.Player.Move.ReadValue<Vector2>();
        _cameraInput = _inputSystemActions.Player.Look.ReadValue<Vector2>();

        _isMoving = _input.sqrMagnitude > 0.01f;
    }

    private void UpdateMovement()
    {
        _movementDirection = transform.right * _input.x + transform.forward * _input.y;
        _isGrounded = _characterController.isGrounded;

        if (_isGrounded && _verticalVelocity.y < 0)
        {
            _verticalVelocity.y = _groundedYVelocity; // small downward force to keep grounded
        }
        else
        {
            _verticalVelocity.y += _gravity * Time.deltaTime;
        }

        _characterController.Move((_movementDirection * _velocity + _verticalVelocity) * Time.deltaTime);
    }

    private void UpdateCameraLook()
    {
        _mouseX = _cameraInput.x * _mouseSensitivity *  Time.deltaTime;
        _mouseY = _cameraInput.y * _mouseSensitivity * 5f *  Time.deltaTime;

        _xRotation -= _mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * _mouseX * _velocity);
    }
}
