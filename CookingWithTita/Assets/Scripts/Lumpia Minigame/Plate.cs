using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlateState { Prepare, Serving}
[System.Serializable]
public class Ingredients {
    public List<string> ingredientsList;
    public bool isDone;
    public Ingredients() {
        ingredientsList = new List<string>();
        isDone = false;
    }
    public void SetReciepe(LumpiaCombo combo) {
        ingredientsList = combo._combo;
    }
    public bool Contains(string ingredient) {
        if (ingredientsList.Contains(ingredient)) {
            ingredientsList.Remove(ingredient);
            if (ingredientsList.Count == 0) {
                isDone = true;
            }
            return true;
        } else {
            return false;
        }
    }
}
public class Plate : MonoBehaviour {
    #region Data
    GameManager _gameManager;
    PlateState state;

    public Ingredients reciepeList = null;
    public bool badDish = false;

    public Transform doneLumpiaPrefab;

    float currentWaitTime;
    float maxWaitTime = 1.0f;
    public LumpiaMinigameUserInterface lumpiaMinigameUserInterface;

    #endregion

    public void InitializePlate(GameManager gameManager) {
        _gameManager = gameManager;
        state = PlateState.Prepare;
        reciepeList = new Ingredients();
        reciepeList.ingredientsList = null;
        lumpiaMinigameUserInterface = FindObjectOfType<LumpiaMinigameUserInterface>();

        SetupPlate();
    }
    public void SetupPlate() {
        SetReciepe(_gameManager.lumpiaMinigame.RequestNewReciepe());
    }
    public void UpdatePlate() {
        if (reciepeList.isDone) {
            lumpiaMinigameUserInterface.ResetCard();
            currentWaitTime += Time.deltaTime;
            if(currentWaitTime > maxWaitTime) {
                DoneReciepe();
                currentWaitTime = 0.0f;
            }
        }
    }

    private void DoneReciepe() {
        lumpiaMinigameUserInterface.ResetCard();
        Transform clone = Instantiate(doneLumpiaPrefab, transform.position + new Vector3(0,0.5f,0), doneLumpiaPrefab.rotation);
        clone.name = doneLumpiaPrefab.name;
        clone.GetComponent<PrepIngredient>().isLumpia = true;
        reciepeList.ingredientsList = null;
        reciepeList.isDone = false;
        SetReciepe(_gameManager.lumpiaMinigame.RequestNewReciepe());
    }
    public void SetReciepe(LumpiaCombo combo) {
        lumpiaMinigameUserInterface.SetCardImage(combo.sprite);
        reciepeList.SetReciepe(combo);
    }
    private void OnCollisionEnter(Collision other) {
        if (!reciepeList.Contains(other.transform.name)) {
            other.transform.GetComponent<PrepIngredient>().isWrong = true;
        } else {
            Destroy(other.gameObject);
        }
    }



}
