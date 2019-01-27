using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumpiaCombo {
    public List<string> _combo;

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
    #endregion

    #region Event Datad
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


    public void InitializeLumpiaMinigame(GameManager gameManager) {
        _gameManager = gameManager;
        gamePlayCamera = GetComponentInChildren<Camera>();
        gamePlayCamera.gameObject.SetActive(false);

        lumpiaCombos = new List<LumpiaCombo>();
        CreateCombos();
        rightHand = FindObjectOfType<PlayerHand>();
        if (rightHand)
            rightHand.InitializePlayerHand(_gameManager);

        plate = FindObjectOfType<Plate>();
        if (plate)
            plate.InitializePlate(_gameManager);

        fryingPan = FindObjectOfType<FryingPan>();
        if (fryingPan)
            fryingPan.InitializeFryingPan(_gameManager);

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Food"), LayerMask.NameToLayer("Vegetable"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Food"), LayerMask.NameToLayer("Meat"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Food"), LayerMask.NameToLayer("Shrimp"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Food"), LayerMask.NameToLayer("Wrapper"));

       
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
        if (plate)
            plate.UpdatePlate();

        if (fryingPan)
            fryingPan.UpdateFryingPan();

        rightHand.transform.Translate(new Vector3(rightHandInput.x, 0, rightHandInput.y) * Time.deltaTime * handMoveSpeed);
    }


}
