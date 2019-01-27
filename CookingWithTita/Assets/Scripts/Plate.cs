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
            if (ingredientsList.Count == 0)
                isDone = true;
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
    public bool finished = false;

    public Transform doneLumpiaPrefab;
    #endregion

    public void InitializePlate(GameManager gameManager) {
        _gameManager = gameManager;
        state = PlateState.Prepare;
        reciepeList.ingredientsList = null;
        SetReciepe(_gameManager.lumpiaMinigame.RequestNewReciepe());
    }

    public void UpdatePlate() {
        if (reciepeList.isDone) {
            Transform clone = Instantiate(doneLumpiaPrefab, transform.position, doneLumpiaPrefab.rotation);
            clone.name = doneLumpiaPrefab.name;
            reciepeList.ingredientsList = null;
            SetReciepe(_gameManager.lumpiaMinigame.RequestNewReciepe());
            finished = false;
        }
    }
    public void SetReciepe(LumpiaCombo combo) {
        reciepeList = new Ingredients();
        reciepeList.SetReciepe(combo);
    }

    private void OnCollisionEnter(Collision other) {
        if (!reciepeList.Contains(other.transform.name)) {
            badDish = true;
        } else {
            Destroy(other.gameObject);
        }
    }



}
