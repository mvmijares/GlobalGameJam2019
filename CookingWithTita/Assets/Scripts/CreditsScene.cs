using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScene : MonoBehaviour {

    GameManager _gameManager;
    
    public Transform playerSpawnLocation;

    public void InitializeCreditsScene(GameManager gameManager) {
        _gameManager = gameManager;
    
    }
    public void SetPlayerLocation() {
        _gameManager.GetPlayer().transform.position = playerSpawnLocation.position;
        _gameManager.GetPlayerCamera().gameObject.SetActive(true);
    }


}
