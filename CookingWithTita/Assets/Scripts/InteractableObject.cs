using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour {

    #region Data
    [SerializeField] private GameManager _gameManager;
    public bool isInView;
    #endregion

    public void InitializeInteractableObject(GameManager gameManager) {
        _gameManager = gameManager;
        isInView = false;
    }
}
