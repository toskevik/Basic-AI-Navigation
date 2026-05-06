using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Variables
    [Tooltip("Player speed")]
    [Range(1.0f, 10.0f)]
    [SerializeField] private float walkSpeed = 1.2f;
    [Tooltip("Jump height")]
    [Range(1.0f, 10.0f)]
    [SerializeField] private float jumpHeight = 1.0f;
    [Tooltip("Gravity value")]
    [Range(-20.0f, 0.0f)]
    [SerializeField] private float gravityValue = -9.81f;
    [Tooltip("Gravity modifier")]
    [Range(-10.0f, 0.0f)]
    [SerializeField] private float gravityModifier = -3.0f;
    [Tooltip("Sprint speed")]
    [Range(1.0f, 10.0f)]
    [SerializeField] private float sprintSpeed = 3.0f;

    // Components
    private CharacterController _controller;
    private Vector3 _playerVelocity;
    private bool _groundedPlayer;
    private Vector3 _move;
    private float _currentSpeed;

    private void Start()
    {
        // Get the character controller component
        _controller = gameObject.GetComponent<CharacterController>();
        _currentSpeed = walkSpeed;
    }

    private void Update()
    {
        // Check if the player is grounded
        _groundedPlayer = _controller.isGrounded;
        // If the player is grounded and has been falling down, reset the y velocity
        if (_groundedPlayer && _playerVelocity.y < 0)
            _playerVelocity.y = 0f;

        // Get the player input
        if (_groundedPlayer)
        {
            if (Input.GetKey(KeyCode.LeftShift))
                _currentSpeed = sprintSpeed;
            else
                _currentSpeed = walkSpeed;

            // Apply horizontal movement to the player's velocity
            _move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized * _currentSpeed;
        }

        // Rotate the player to the direction of movement
        if (_move != Vector3.zero)
            gameObject.transform.forward = _move;

        // Changes the height position of the player - aka jumps
        if (Input.GetButtonDown("Jump") && _groundedPlayer)
            _playerVelocity.y += Mathf.Sqrt(jumpHeight * gravityModifier * gravityValue);

        // Apply gravity to the player
        _playerVelocity.y += gravityValue * Time.deltaTime;
        // Move the player
        _controller.Move((_move + _playerVelocity) * Time.deltaTime);
    }
}
