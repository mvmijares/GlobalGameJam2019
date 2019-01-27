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

    bool isCoroutineRunning = false;
    bool isPlaying = false;
    bool isLoaded = false;
    public void InitializeDialogue(GameManager gameManager) {
        _gameManager = gameManager;
        sentences = new Queue<string>();
        proUGUI.gameObject.SetActive(false);
        titaImage.gameObject.SetActive(false);

    }
    public void StartDialogue() {
        proUGUI.gameObject.SetActive(true);
        titaImage.gameObject.SetActive(true);

        sentences.Clear();

        if (!isLoaded) { 
            foreach (string s in lines) {
                sentences.Enqueue(s);
            }
            isLoaded = true;
        }
        if (!isPlaying)
            DisplayNextSentence();

    }
    void DisplayNextSentence() {
        isPlaying = true;
        if (sentences.Count == 0) {
            isPlaying = false;
            return;
        }
        string sentence = sentences.Dequeue();

        StopAllCoroutines();
        if(!isCoroutineRunning) 
            StartCoroutine(TypeSentence(sentence));

        if (proUGUI.text.Length == sentence.Length) {
            isCoroutineRunning = false;
            isPlaying = false;
        }

    }
    IEnumerator TypeSentence(string sentence) {
        isCoroutineRunning = true;
        proUGUI.text = "";
        foreach(char letter in sentence.ToCharArray()) {
            proUGUI.text += letter;
            if (proUGUI.text.Length == sentence.ToCharArray().Length - 1) {
                isCoroutineRunning = false;
                isPlaying = false;
            }
            yield return null;
        }
    }

}
