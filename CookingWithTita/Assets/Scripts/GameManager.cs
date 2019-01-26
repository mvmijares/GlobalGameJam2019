using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MiniGame { None, Lumpia, Lechon, HaloHalo}

public class GameManager : MonoBehaviour {
    #region Data
    [SerializeField] private Player player;
    public Player GetPlayer() { return player; }

    [SerializeField] private FirstPersonCamera playerCamera;
    public FirstPersonCamera GetPlayerCamera() { return playerCamera; }

    [SerializeField] private CheckObjectCamera checkObjectCamera;
    public CheckObjectCamera GetCheckObjectCamera() { return checkObjectCamera; }
    public int playerScore;

    [SerializeField] private LoadingScreen loadingScreen;
    public LoadingScreen GetLoadingScreen { get { return loadingScreen; } }
    public List<InteractableObject> interfactableObjectList;

    public MiniGame miniGame;

    public bool switchScreen = false;
    [SerializeField] private float loadingScreenAlpha = 0.0f;

    public float switchSceneTime;
    public float waitLoadingTime;
    [SerializeField] private float currentSceneTime;
    [SerializeField] private float currentWaitTime;

    string objectName;
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

        miniGame = MiniGame.None;

        loadingScreen = FindObjectOfType<LoadingScreen>();
        if(loadingScreen)
            loadingScreen.InitializeLoadingScreen(this);
     
    }
    public void InteractWithObjectEventCalled(Transform t) {
        switch (t.name) {
            case "Lumpia": {
                    switchScreen = true;
                    objectName = t.name;
                    break;
                }
        }
    }
    private void OnEnable() {
        player.RegisterPlayerEvents();
        playerCamera.RegisterPlayerCameraEvents();
        checkObjectCamera.InteractWithObjectEvent += InteractWithObjectEventCalled;
    }
    private void OnDisable() {
        player.DeregisterPlayerEvents();
        playerCamera.DeregisterPlayerCameraEvents();
        checkObjectCamera.InteractWithObjectEvent -= InteractWithObjectEventCalled;
    }
    private void Update() { 
        if (player) {
            player.UpdatePlayer();
        }
        switch (miniGame) {
            case MiniGame.None: {
                    Exploration();
                    break;
                }
            case MiniGame.Lumpia: {
                    LumpiaMiniGame();
                    break;
                }
        }
    }

    void Exploration() {
        if (switchScreen) {
            currentSceneTime += Time.deltaTime;
            if (currentSceneTime < switchSceneTime) {
                if (loadingScreenAlpha < 1)
                    loadingScreenAlpha += Time.deltaTime;
                else {
                    playerCamera.gameObject.SetActive(false);
                }
                loadingScreen.SetLoadingScreenAlpha(loadingScreenAlpha);
            } else {
                currentWaitTime += Time.deltaTime;
                if (currentWaitTime < waitLoadingTime) {
                    if (loadingScreenAlpha > 0)
                        loadingScreenAlpha -= Time.deltaTime;

                    loadingScreen.SetLoadingScreenAlpha(loadingScreenAlpha);
                } else {
                    currentWaitTime = 0f;
                    currentSceneTime = 0f;
                    switchScreen = false;
                    switch (objectName) {
                        case "Lumpia": {
                                miniGame = MiniGame.Lumpia;
                                break;
                            }
                    }
                }
            }
        }
    }

    void LumpiaMiniGame() {

    }

    private void LateUpdate() {
        if (playerCamera)
            playerCamera.UpdatePlayerCamera();

        if (checkObjectCamera)
            checkObjectCamera.UpdateCheckObjectCamera();
    }
}
