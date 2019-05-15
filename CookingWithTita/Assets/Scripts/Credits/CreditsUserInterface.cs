using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsUserInterface : MonoBehaviour {
    GameManager _gameManager;
    public Image reciepeCard;
    bool enable;
    public float reciepeCardAlpha = 0;

    private float currentTimer;
    public float maxTimer = 1.0f;
    public void IntializeCreditsUserInterface(GameManager gameManager) {
        _gameManager = gameManager;
        enable = false;
        reciepeCardAlpha = 0;
        EnableReciepeCard(enable);
        reciepeCard.color = new Color(reciepeCard.color.r, reciepeCard.color.g, reciepeCard.color.b, reciepeCardAlpha);
    }
    public void EnableReciepeCard(bool condition) {
        enable = condition;
    }

    public void UpdateUserInterface() {
        if (enable) {
            if(reciepeCardAlpha <= 1f)
                reciepeCard.color = new Color(reciepeCard.color.r, reciepeCard.color.g, reciepeCard.color.b, reciepeCardAlpha += Time.deltaTime);
        }
    }

}
