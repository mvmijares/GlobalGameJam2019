using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using TMPro;

public class LumpiaMinigameUserInterface : MonoBehaviour {

    GameManager _gameManager;

    public GameObject interfaceObject;
    public Image reciepeCard;
    public bool playAnimation = false;
    Animator anim;
    Vector3 originPosition;

    public TextMeshProUGUI proUGUI;

    public void InitializeLumpiaMinigameUserInterface(GameManager gameManager) {
        _gameManager = gameManager;
        anim = reciepeCard.GetComponent<Animator>();
        originPosition = reciepeCard.transform.position;
        proUGUI.gameObject.SetActive(false);

    }

    public void CloseUserInterface() {
        proUGUI.gameObject.SetActive(false);
        reciepeCard.gameObject.SetActive(false);
    }
    public void SetCardImage(Sprite sprite) {
        reciepeCard.sprite = sprite;
    }
    public void ResetCard() {
        reciepeCard.transform.gameObject.SetActive(false);
        reciepeCard.transform.position = originPosition;
    }
    public void UpdateUserInterface() {
        if (playAnimation) {
            reciepeCard.transform.gameObject.SetActive(true);
            anim.SetBool("Play", true);
            if(reciepeCard.GetComponent<UserInterfaceAnimation>().state == AnimationState.IsFinished) {
                anim.SetBool("Play", false);
                playAnimation = false;
            }
        } 
    }

    public void SetTimer(float timer) {
        if(proUGUI.gameObject.activeSelf == false)
            proUGUI.gameObject.SetActive(true);

        string minutes = Mathf.Floor(timer / 60).ToString("00");
        string seconds = Mathf.Floor(timer % 60).ToString("00");

        proUGUI.text = string.Format("{0}:{1}", minutes, seconds);
    }
}
