using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour {
    #region Data
    GameManager _gameManager;
    public Transform target;
    [SerializeField] Vector2 lookDirection;
    public float controllerSensitivity = 1f;
    float yaw;
    float pitch;
    public Vector2 pitchMinMax = new Vector3(-40, 85);
    Vector3 currentRotation;
    float dstFromTarget = 1;
    public float rotationSmoothTime = .12f;
    Vector3 rotationSmoothVel;
    #endregion
    #region Event Data
    public void OnLookEventCalled(Vector2 look) { lookDirection = look; }
    #endregion
    public void InitializePlayerCamera(GameManager gameManager) {
        _gameManager = gameManager;

    }
    public void RegisterPlayerCameraEvents() {
        _gameManager.GetPlayer().playerInput.OnLookEvent += OnLookEventCalled;
    }
    public void DeregisterPlayerCameraEvents() {
        _gameManager.GetPlayer().playerInput.OnLookEvent -= OnLookEventCalled;
    }
    public void UpdatePlayerCamera() {
        yaw += lookDirection.x * controllerSensitivity;
        pitch += lookDirection.y * controllerSensitivity;

        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVel, rotationSmoothTime);

        transform.eulerAngles = currentRotation;

        transform.position = target.position - (transform.forward * -0.5f);
        
    }
}
