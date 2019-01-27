using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LechonUserInterface : MonoBehaviour {
    private GameManager _gameManager;

    public Image directionalKey;

    public Sprite counterClockWiseImage;
    public Sprite clockWiseImage;

    public void InitializeLechonUserInterface(GameManager gameManager) {
        _gameManager = gameManager;
        directionalKey.gameObject.SetActive(false);
    }

    public void UpdateLechonUserInterface() {

    }

    public void SetImageBasedOnDirection(JoystickDirection direction) {

        if (direction == JoystickDirection.CounterClockwise)
            directionalKey.sprite = counterClockWiseImage;
        else if (direction == JoystickDirection.Clockwise)
            directionalKey.sprite = clockWiseImage;
        directionalKey.gameObject.SetActive(true);
    }
}
