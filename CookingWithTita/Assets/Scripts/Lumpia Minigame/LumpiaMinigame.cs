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

    Boundaries gamePlayBoundaries;
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

    public Transform foodPrefab; // Will change this for actual food prefab.

    public Transform trashCan;
    public float foodDestructionTime;

    public TextPopup wrapperText;
    public TextPopup shrimpText;
    public TextPopup meatText;
    public TextPopup vegText;
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
        if (gamePlayCamera) {
            gamePlayCamera.gameObject.SetActive(false);
            gamePlayBoundaries = gamePlayCamera.GetComponent<Boundaries>();
            gamePlayBoundaries.SetCamera(gamePlayCamera);
        }
        wrapperText.SetCamera(gamePlayCamera);
        shrimpText.SetCamera(gamePlayCamera);
        meatText.SetCamera(gamePlayCamera);
        vegText.SetCamera(gamePlayCamera);

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
        int index = Random.Range(0, lumpiaCombos.Count - 1);
        Debug.Log("Lumpia index is " + index);
        return lumpiaCombos[index];
    }

    public Transform CreateIngrdient(string foodName) {
        Transform clone = Instantiate(foodPrefab, rightHand.raycastPoint.position, foodPrefab.rotation);
        clone.GetComponent<PrepIngredient>().InitializePrepIngredient(_gameManager);
        clone.GetComponent<MeshRenderer>().sharedMaterial = GetMaterial(foodName);
        clone.name = foodName;
        clone.GetComponent<PrepIngredient>().destructionTime = foodDestructionTime;
        return clone;
    }
    Material GetMaterial(string name) {
        Material newMat = null;
        switch (name) {
            case "Wrapper": {
                    newMat = _gameManager.lumpiaMinigame.purple;
                    break;
                }
            case "Shrimp": {
                    newMat = _gameManager.lumpiaMinigame.red;
                    break;
                }
            case "Vegetable": {
                    newMat = _gameManager.lumpiaMinigame.green;
                    break;
                }
            case "Meat": {
                    newMat = _gameManager.lumpiaMinigame.blue;
                    break;
                }

        }
        return newMat;
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


            float xPos = (rightHandInput.x * handMoveSpeed);
            float zPos = (rightHandInput.y * handMoveSpeed);

            float newXPos = rightHand.transform.position.x + xPos * handMoveSpeed * Time.deltaTime;

            if (newXPos <= gamePlayBoundaries.GetWorldBoundary().BottomLeft.x
                || newXPos >= gamePlayBoundaries.GetWorldBoundary().BottomRight.x) { 
                xPos = 0;
            }

            float newZPos = rightHand.transform.position.z + zPos * handMoveSpeed * Time.deltaTime;
            if (newZPos <= gamePlayBoundaries.GetWorldBoundary().BottomLeft.y ||
                newZPos >= gamePlayBoundaries.GetWorldBoundary().TopLeft.y) {
                zPos = 0;
            }
            rightHand.transform.position += new Vector3(xPos, 0, zPos) * Time.deltaTime;
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

    private void LateUpdate() {
        if(gamePlayBoundaries)
            gamePlayBoundaries.UpdateCameraBoundaries();
    }


}
