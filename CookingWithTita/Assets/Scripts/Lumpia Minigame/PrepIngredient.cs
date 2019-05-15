using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepIngredient : MonoBehaviour {
    public bool isPlaced;
    public bool isLumpia;
    public bool destroy;
    private float currentTimer;
    public float destructionTime;

    GameManager _gameManager;

    public void InitializePrepIngredient(GameManager gameManager) {
        _gameManager = gameManager;
        destroy = false;

    }
    public void Update() {
        if(destroy)
            Destruction();
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            transform.position = _gameManager.lumpiaMinigame.trashCan.position;
            destroy = true;
        }
    }

    void Destruction() {
        currentTimer += Time.deltaTime;
        if (currentTimer >= destructionTime) {
            Destroy(this.gameObject);
        }
    }
}
