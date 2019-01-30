using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepIngredient : MonoBehaviour {
    public bool isWrong;
    public bool isLumpia;
    private float currentTimer;
    public float destructionTime;

    GameManager _gameManager;

    public void InitializePrepIngredient(GameManager gameManager) {
        _gameManager = gameManager;

    }
    public void Update() {
        if (isWrong) {
            transform.position = _gameManager.lumpiaMinigame.trashCan.position;
            currentTimer += Time.deltaTime;
            if(currentTimer >= destructionTime) {
                Destroy(this.gameObject);
            }
        }
    }
}
