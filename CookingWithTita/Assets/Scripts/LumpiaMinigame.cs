using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LumpiaCombo {
    public List<string> _combo;
    public Sprite sprite;

    public LumpiaCombo(string[] combo) {
        _combo = new List<string>();

        foreach (string s in combo)
            _combo.Add(s);
    }

}
public class LumpiaMinigame : MonoBehaviour {
    #region Data
    GameManager _gameManager;
    public Camera gamePlayCamera;
    public PlayerHand rightHand;

    public float handMoveSpeed;
    [SerializeField] Vector2 rightHandInput;
    [SerializeField] Vector2 leftHandInput;

    [SerializeField] Plate plate;
    [SerializeField] List<LumpiaCombo> lumpiaCombos;

    [SerializeField] FryingPan fryingPan;

    public Material red;
    public Material blue;
    public Material green;
    public Material purple;

    public bool tutorial = false;
    public bool startTimer = false;

    public List<Sprite> reciepeCards;
    LumpiaMinigameUserInterface lumpiaMinigameUserInterface;

    public float miniGameDuration;
    private float currentMiniGameDuration;
    #endregion

    #region Event Data
    public void OnActionKeyPressedCalled() {
        if (rightHand.GetComponent<PlayerHand>().heldItem == null) {
            if (_gameManager.miniGame == MiniGame.Lumpia)
                rightHand.GetComponent<PlayerHand>().CheckRaycast();
        } else {
            PlayerHand hand = rightHand.GetComponent<PlayerHand>();
            hand.heldItem.SetParent(null);
            hand.heldItem.GetComponent<Rigidbody>().isKinematic = false;
            hand.heldItem = null;
        }
    }
    public void OnLookEventCalled(Vector2 look) {
        rightHandInput = look;
    }
    #endregion

    public void CloseMiniGame() {
        lumpiaMinigameUserInterface.CloseUserInterface();
    }

    public void InitializeLumpiaMinigame(GameManager gameManager) {
        _gameManager = gameManager;
        gamePlayCamera.gameObject.SetActive(false);

        lumpiaCombos = new List<LumpiaCombo>();
        CreateCombos();

        plate = FindObjectOfType<Plate>();
        if (plate)
            plate.InitializePlate(_gameManager);

        rightHand = FindObjectOfType<PlayerHand>();
        if (rightHand)
            rightHand.InitializePlayerHand(_gameManager);


        fryingPan = FindObjectOfType<FryingPan>();

        if (fryingPan)
            fryingPan.InitializeFryingPan(_gameManager);

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Food"), LayerMask.NameToLayer("Vegetable"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Food"), LayerMask.NameToLayer("Meat"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Food"), LayerMask.NameToLayer("Shrimp"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Food"), LayerMask.NameToLayer("Wrapper"));

        lumpiaMinigameUserInterface = FindObjectOfType<LumpiaMinigameUserInterface>();
        if (lumpiaMinigameUserInterface)
            lumpiaMinigameUserInterface.InitializeLumpiaMinigameUserInterface(_gameManager);

        _gameManager.GetPlayer().playerInput.OnLookEvent += OnLookEventCalled;
        _gameManager.GetPlayer().playerInput.OnActionKeyPressedEvent += OnActionKeyPressedCalled;

    }
    private void OnDestroy() {
        _gameManager.GetPlayer().playerInput.OnLookEvent -= OnLookEventCalled;
        _gameManager.GetPlayer().playerInput.OnActionKeyPressedEvent -= OnActionKeyPressedCalled;
    }

    private void CreateCombos() {
        lumpiaCombos.Add(new LumpiaCombo(new string[] { "Wrapper", "Meat" }));
        lumpiaCombos.Add(new LumpiaCombo(new string[] { "Wrapper", "Vegetable" }));
        lumpiaCombos.Add(new LumpiaCombo(new string[] { "Wrapper", "Shrimp" }));
        lumpiaCombos.Add(new LumpiaCombo(new string[] { "Wrapper", "Meat", "Vegetable"}));
        lumpiaCombos.Add(new LumpiaCombo(new string[] { "Wrapper", "Meat", "Shrimp"}));
        lumpiaCombos.Add(new LumpiaCombo(new string[] { "Wrapper", "Vegetable", "Shrimp" }));
        lumpiaCombos.Add(new LumpiaCombo(new string[] { "Wrapper", "Meat", "Shrimp", "Vegetable" }));

        int index = 0;

        foreach (LumpiaCombo c in lumpiaCombos) {
            c.sprite = reciepeCards[index];
            index++;
        }
    }

    void AddLumpiaCombo(LumpiaCombo combo) {
        lumpiaCombos.Add(combo);
    }

    public void SetGameplayCamera(bool condition) {
        gamePlayCamera.gameObject.SetActive(condition);
    }

    public LumpiaCombo RequestNewReciepe() {
        int index = Random.Range(0, lumpiaCombos.Count);
        return lumpiaCombos[index];
    }

    public void UpdateMiniGame() {
        if (tutorial) {

        } else {
            if (lumpiaMinigameUserInterface)
                lumpiaMinigameUserInterface.UpdateUserInterface();

            if (plate)
                plate.UpdatePlate();

            if (fryingPan)
                fryingPan.UpdateFryingPan();

            rightHand.transform.Translate(new Vector3(rightHandInput.x, 0, rightHandInput.y) * Time.deltaTime * handMoveSpeed);
        }

        if (startTimer) {
            currentMiniGameDuration += Time.deltaTime;
            lumpiaMinigameUserInterface.SetTimer(currentMiniGameDuration);

            if(currentMiniGameDuration >= miniGameDuration) {
                _gameManager.switchScreen = true;
                _gameManager.objectName = "Lechon";
                _gameManager.SwitchScene();
                CloseMiniGame();
            }

        }
    }


}
