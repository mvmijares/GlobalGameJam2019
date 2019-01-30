using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryingPan : MonoBehaviour {

    private GameManager _gameManager;
    private bool cooking;

    private float currentCookingTimer;
    public float maxCookingTime = 3f;

    public void InitializeFryingPan(GameManager gameManager) {
        _gameManager = gameManager;
        cooking = false;
    }
    public void UpdateFryingPan() {
        if (cooking) {
            currentCookingTimer += Time.deltaTime;
            if(currentCookingTimer >= maxCookingTime) {
                _gameManager.playerScore++;
                cooking = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Lumpia")) {
            cooking = true;
            Destroy(collision.gameObject);
        } else {
            if (collision.gameObject.GetComponent<PrepIngredient>()) {
                collision.gameObject.GetComponent<PrepIngredient>().isWrong = true;
            }
        }

    }
}
