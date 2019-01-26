using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    #region Data
    [SerializeField] private Player player;
    public Player GetPlayer() { return player; }

    [SerializeField] private FirstPersonCamera playerCamera;
    public FirstPersonCamera GetPlayerCamera() { return playerCamera; }

    [SerializeField] private CheckObjectCamera checkObjectCamera;
    public CheckObjectCamera GetCheckObjectCamera() { return checkObjectCamera; }
    public int playerScore;

    public List<InteractableObject> interfactableObjectList;

    #endregion
    private void Awake() {
        player = FindObjectOfType<Player>();
        if (player)
            player.InitializePlayer(this);

        playerCamera = FindObjectOfType<FirstPersonCamera>();
        if (playerCamera)
            playerCamera.InitializePlayerCamera(this);

        checkObjectCamera = FindObjectOfType<CheckObjectCamera>();
        if (checkObjectCamera)
            checkObjectCamera.InitializeCheckObjectCamera(this);

        playerCamera.target = player.headPosition;

        interfactableObjectList = new List<InteractableObject>();

        InteractableObject[] interactableObjects = FindObjectsOfType<InteractableObject>();

        foreach (InteractableObject i in interactableObjects) {
            i.InitializeInteractableObject(this);
            interfactableObjectList.Add(i);
        }

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

        if (checkObjectCamera)
            checkObjectCamera.UpdateCheckObjectCamera();
    }
}
