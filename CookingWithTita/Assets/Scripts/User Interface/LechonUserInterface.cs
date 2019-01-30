using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LechonUserInterface : MonoBehaviour {
    private GameManager _gameManager;

    public Image directionalKey;

    public Sprite counterClockWiseImage;
    public Sprite clockWiseImage;

    [SerializeField] private Vector3 originalPos;

    Animator directionKeyAnim;
    public void InitializeLechonUserInterface(GameManager gameManager) {
        _gameManager = gameManager;
        originalPos = directionalKey.transform.position;
        directionKeyAnim = directionalKey.GetComponent<Animator>();
    }

    public void UpdateLechonUserInterface() {

    }
    public bool GetPlayAnimationState() {
        if (directionKeyAnim.GetCurrentAnimatorStateInfo(0).IsName("Opening_DirectionKeyImage")) {
            return true;
        }else if (directionKeyAnim.GetCurrentAnimatorStateInfo(0).IsName("Closing_DirectionKeyImage")) {
            return true;
        } else {
            return false;
        }
    }
    public void SetImageCondition(bool condition) {
        directionalKey.gameObject.SetActive(condition);
    }
    public void SetDoneCondition(bool condition) {
        directionKeyAnim.SetBool("Done", condition);
    }
    public void SetPlayCondition(bool condition) {
        directionKeyAnim.SetBool("Play", condition);
    }
    public void ResetImage() {
        directionalKey.gameObject.SetActive(false);
        directionalKey.transform.position = originalPos;
    }
    public void SetImageBasedOnDirection(JoystickDirection direction) {
        if (direction == JoystickDirection.CounterClockwise)
            directionalKey.sprite = counterClockWiseImage;
        else if (direction == JoystickDirection.Clockwise)
            directionalKey.sprite = clockWiseImage;
    }
}
