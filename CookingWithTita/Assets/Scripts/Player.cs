using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data involving the player
/// </summary>
public class Player : MonoBehaviour {
    #region Data
    private GameManager _gameManager;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private PlayerInput _playerInput;
    public PlayerInput playerInput { get { return _playerInput; } }

    [SerializeField] private FirstPersonCamera playerCamera;
    public float turnSmoothTime = 0.2f; // num of secs for smooth damp to go from curr to target val;
    private float turnSmoothVel;
    float walkSpeed = 2f;
    float currentSpeed;
    public float speedSmoothTime = 0.1f;
    float speedSmoothVel;

    [SerializeField] private Vector2 _movement;
    [SerializeField] private Vector2 _look;

    public Transform headPosition;

    public void OnMovementEventCalled(Vector2 movement) { _movement = movement; }
    public void OnLookEventCalled(Vector2 look) { _look = look; }

    #endregion

    public void InitializePlayer(GameManager gameManager) {
        _gameManager = gameManager;
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.InitializePlayerController(this);
    }

    public void RegisterPlayerEvents() {
        Debug.Log("Registered Input Events");
        _playerInput.OnMovementEvent += OnMovementEventCalled;
        _playerInput.OnLookEvent += OnLookEventCalled;
    }
    public void DeregisterPlayerEvents() {
        _playerInput.OnMovementEvent -= OnMovementEventCalled;
        _playerInput.OnLookEvent -= OnLookEventCalled;
    }

    public void UpdatePlayer() {
        _playerInput.UpdatePlayerInput();
        if (_movement != Vector2.zero) {
            float targetRotation = Mathf.Atan2(_movement.x, _movement.y) * Mathf.Rad2Deg + _gameManager.GetPlayerCamera().transform.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVel, turnSmoothTime);
            float targetSpeed = walkSpeed * _movement.magnitude; //speed is 0 if no input happens

            currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVel, speedSmoothTime);
            transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);
        }
    }
}
