using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitaDialogue : MonoBehaviour {
    GameManager _gameManager;

    private Queue<string> sentences;
    [TextArea(3, 10)]
    public string[] lines;

    public Transform titaImage;
    public TextMeshProUGUI proUGUI;

    [SerializeField] bool isCoroutineRunning = false;
    [SerializeField] bool isPlaying = false;
    [SerializeField] bool isLoaded = false;

    private float currentWaitTimer = 0f;
    public float waitTimeForDialogue = 1f;
    [SerializeField] bool isFinished;
    bool waitForNextDialogue;
    [SerializeField] int index;

    public void InitializeDialogue(GameManager gameManager) {
        _gameManager = gameManager;
        proUGUI.gameObject.SetActive(false);
        titaImage.gameObject.SetActive(false);
        index = 0;
        waitForNextDialogue = false;
        isFinished = false;
    }
    public void PlayDialogue() {
        if (!isFinished) {
            proUGUI.gameObject.SetActive(true);
            titaImage.gameObject.SetActive(true);
        } else {
            proUGUI.gameObject.SetActive(false);
            titaImage.gameObject.SetActive(false);
        }
        if (waitForNextDialogue) {
            currentWaitTimer += Time.deltaTime;
            if (currentWaitTimer >= waitTimeForDialogue) {
                currentWaitTimer = 0f;
                if (index < lines.Length - 1) {
                    index++;
                } else {
                    isFinished = true;
                }
                waitForNextDialogue = false;
            }
        } else {
            DisplayNextSentence();
        }
    }

    void DisplayNextSentence() {
        if (!isCoroutineRunning && !isFinished) {
            StartCoroutine(TypeSentence(lines[index]));
        }
    }
    IEnumerator TypeSentence(string sentence) {
        isCoroutineRunning = true;
        proUGUI.text = "";
        foreach(char letter in sentence.ToCharArray()) {
            proUGUI.text += letter;
            yield return null;
            if (proUGUI.text.Length >= sentence.Length) {
                waitForNextDialogue = true;
                isCoroutineRunning = false;
            }
        }
    }

}
