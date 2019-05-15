using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MiniGame { None, Exploration, Lumpia, Lechon, HaloHalo, Credits}

public class GameManager : MonoBehaviour {
    #region Data
    public AudioClip introMusic;
    public AudioClip lumpiaMusic;
    public AudioClip lechonMusic;
    public AudioClip creditsMusic; 
    public AudioSource audioSource;
    [SerializeField] private Player player;
    public Player GetPlayer() { return player; }
    float volume; 

    public void OnLeftBumperPressedEventCalled() {
        if(volume > 0)
            volume -= Time.deltaTime;

        audioSource.volume = volume;
    }
    public void OnRightBumperPressedEventCalled() {
        if (volume < 1)
            volume += Time.deltaTime;

        audioSource.volume = volume;
    }
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
    [SerializeField] public float loadingScreenAlpha = 0.0f;

    public float switchSceneTime;
    public float waitLoadingTime;
    [SerializeField] private float currentSceneTime;
    [SerializeField] private float currentWaitTime;

    [SerializeField] public string objectName;

    private TitleScreen titleScreen;
    public TitaDialogue titaDialogue;
    public LumpiaMinigame lumpiaMinigame;
    public LechonMinigame lechonMinigame;
    public CreditsScene creditsScene;

    bool startDialogue = false;
    
    public float distanceFromLumpia;

    public bool credits;
    #endregion
    private void Awake() {

        player = FindObjectOfType<Player>();
        if (player)
            player.InitializePlayer(this);

        playerCamera = FindObjectOfType<FirstPersonCamera>();
        if (playerCamera)
            playerCamera.InitializePlayerCamera(this);

        miniGame = MiniGame.None;

        titleScreen = FindObjectOfType<TitleScreen>();
        if (titleScreen)
            titleScreen.InitializeTitleScreen(this);
    

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

        titaDialogue = FindObjectOfType<TitaDialogue>();
        if (titaDialogue)
            titaDialogue.InitializeDialogue(this);

        creditsScene = FindObjectOfType<CreditsScene>();
        if (creditsScene)
            creditsScene.InitializeCreditsScene(this);


        loadingScreen = FindObjectOfType<LoadingScreen>();
        if(loadingScreen)
            loadingScreen.InitializeLoadingScreen(this);


        InitializeLumpiaMiniGame();
        InitializeLechonMiniGame();
        volume = 0.5f;

        credits = false;

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = introMusic;
        audioSource.Play();

    }
    public void InteractWithObjectEventCalled(Transform t) {
        if (miniGame == MiniGame.Exploration) {
            switch (t.name) {
                case "Lumpia": {
                        if ((t.transform.position - player.transform.position).magnitude < distanceFromLumpia) {
                            switchScreen = true;
                            objectName = t.name;
                        }
                        break;
                    }
                case "Lechon": {
                        if ((t.transform.position - player.transform.position).magnitude < distanceFromLumpia) {
                            switchScreen = true;
                            objectName = t.name;
                        }
                        break;
                    }

            }
        } else if(miniGame == MiniGame.Credits) {
            if (t.name == "Credits")
                creditsScene.creditsUserInterface.EnableReciepeCard(true);
        }
    }
    private void OnEnable() {
        player.RegisterPlayerEvents();
        playerCamera.RegisterPlayerCameraEvents();
        checkObjectCamera.InteractWithObjectEvent += InteractWithObjectEventCalled;

        player.playerInput.OnLeftBumperPressedEvent += OnLeftBumperPressedEventCalled;
        player.playerInput.OnRightBumperPressedEvent += OnRightBumperPressedEventCalled;
    }
    private void OnDisable() {
        player.DeregisterPlayerEvents();
        playerCamera.DeregisterPlayerCameraEvents();
        checkObjectCamera.InteractWithObjectEvent -= InteractWithObjectEventCalled;

        player.playerInput.OnLeftBumperPressedEvent -= OnLeftBumperPressedEventCalled;
        player.playerInput.OnRightBumperPressedEvent -= OnRightBumperPressedEventCalled;
    }
    private void Update() {
      
        if (player) {
            player.UpdatePlayer();
        }
        switch (miniGame) {
            case MiniGame.None: {
                    titleScreen.UpdateTitleScreen();
                    break;
                }
            case MiniGame.Exploration: {
                    Exploration();
                    break;
                }
            case MiniGame.Lumpia: {
                    LumpiaMiniGame();
                    break;
                }
            case MiniGame.Lechon: {
                    LechonMiniGame();
                    break;
                }
            case MiniGame.Credits: {
                    CreditsScene();
                    break;
                }
        }
        if(startDialogue)
            titaDialogue.PlayDialogue();

        if (switchScreen) {
            SwitchScene();
        }
    }

    public void PlayLumpiaMusic() {
        audioSource.Stop();
        audioSource.clip = lumpiaMusic;
        audioSource.Play();
    }
    public void PlayLechonMusic() {
        audioSource.Stop();
        audioSource.clip = lechonMusic;
        audioSource.Play();
    }
    public void PlayCreditsMusic() {
        audioSource.Stop();
        audioSource.clip = creditsMusic;
        audioSource.Play();
    }
    void InitializeLumpiaMiniGame() {
        lumpiaMinigame = FindObjectOfType<LumpiaMinigame>();

        if (lumpiaMinigame)
            lumpiaMinigame.InitializeLumpiaMinigame(this);
    }
    void InitializeLechonMiniGame() {
        lechonMinigame = FindObjectOfType<LechonMinigame>();

        if (lechonMinigame)
            lechonMinigame.InitializeLechonMinigame(this);

    }

    private void Exploration() {
        startDialogue = true;
        SwitchScene();
    }
    public void SwitchScene() {
        if (switchScreen) {
            currentSceneTime += Time.deltaTime;
            if (currentSceneTime < switchSceneTime) {
                if (loadingScreenAlpha < 1)
                    loadingScreenAlpha += Time.deltaTime;
                else {
                    playerCamera.gameObject.SetActive(false);
                    if (!credits) {
                        switch (objectName) {
                            case "Lumpia": {
                                    lumpiaMinigame.SetGameplayCamera(true);
                                    break;
                                }
                            case "Lechon": {
                                    lumpiaMinigame.SetGameplayCamera(false);
                                    lechonMinigame.SetGameplayCamera(true);
                                    break;
                                }
                        }
                    } else {
                        miniGame = MiniGame.Credits;
                        lechonMinigame.SetGameplayCamera(false);
                        playerCamera.gameObject.SetActive(true);
                        creditsScene.SetPlayerLocation();
                    }
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
                    if (!credits) {
                        switch (objectName) {
                            case "Lumpia": {
                                    miniGame = MiniGame.Lumpia;
                                    lumpiaMinigame.startTimer = true;
                                    break;
                                }
                            case "Lechon": {
                                    miniGame = MiniGame.Lechon;
                                    break;
                                }
                        }
                    }
                }
            }
        }
    }
    void ResetVariables() {
        currentWaitTime = 0.0f;
        currentSceneTime = 0.0f;
    }
    void LechonMiniGame() {
        if (!lechonMinigame.lechonCamera.gameObject.activeSelf)
            lechonMinigame.lechonCamera.gameObject.SetActive(true);

        if (audioSource.clip != lechonMusic) {
            PlayLechonMusic();
        }

        lechonMinigame.UpdateLechonMinigame();
    }
    void LumpiaMiniGame() {
        if(audioSource.clip != lumpiaMusic) {
            PlayLumpiaMusic();
        }
        lumpiaMinigame.UpdateMiniGame();
    }
    void CreditsScene() {
        if (audioSource.clip != creditsScene)
            PlayCreditsMusic();

        creditsScene.UpdateCreditsScene();
    }
    private void LateUpdate() {
        if (playerCamera)
            playerCamera.UpdatePlayerCamera();

        if (checkObjectCamera)
            checkObjectCamera.UpdateCheckObjectCamera();
    }
}
