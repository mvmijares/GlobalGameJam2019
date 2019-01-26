using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    #region Data
    [SerializeField] private Player player;
    public Player GetPlayer() { return player; }

    [SerializeField] private FirstPersonCamera playerCamera;
    public FirstPersonCamera GetPlayerCamera() { return playerCamera; }
    public int playerScore;

    #endregion
    private void Awake() {
        player = FindObjectOfType<Player>();
        if (player)
            player.InitializePlayer(this);

        playerCamera = FindObjectOfType<FirstPersonCamera>();
        if (playerCamera)
            playerCamera.InitializePlayerCamera(this);

        playerCamera.target = player.headPosition;
    }
    private void OnEnable() {
        player.RegisterPlayerEvents();
        playerCamera.RegisterPlayerCameraEvents();
    }
    private void OnDisable() {
        player.DeregisterPlayerEvents();
        playerCamera.DeregisterPlayerCameraEvents();
    }
    private void Update() {
        if (player) {
            player.UpdatePlayer();

        }
    }

    private void LateUpdate() {
        if (playerCamera)
            playerCamera.UpdatePlayerCamera();
    }
}
