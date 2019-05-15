using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScene : MonoBehaviour {

    GameManager _gameManager;
    
    public Transform playerSpawnLocation;
    public CreditsUserInterface creditsUserInterface;

    public void InitializeCreditsScene(GameManager gameManager) {
        _gameManager = gameManager;
        creditsUserInterface = FindObjectOfType<CreditsUserInterface>();
        if (creditsUserInterface)
            creditsUserInterface.IntializeCreditsUserInterface(gameManager);
    }
    public void SetPlayerLocation() {
        _gameManager.GetPlayer().transform.position = playerSpawnLocation.position;
        _gameManager.GetPlayerCamera().gameObject.SetActive(true);
    }

    public void UpdateCreditsScene() {
        if(creditsUserInterface)
            creditsUserInterface.UpdateUserInterface();
    }


}
