using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {

    #region Data
    GameManager _gameManager;
    public Image loadingScreen;
    private Color color;
    #endregion

    public void InitializeLoadingScreen(GameManager gameManager) {
        _gameManager = gameManager;
        color = loadingScreen.color;
        loadingScreen.color = new Color(color.r,color.g, color.b, 0);
    }

    public void SetLoadingScreenAlpha(float percentage) {
        if (loadingScreen) {
            Color newColor = new Color(color.r, color.g, color.b, percentage);
            loadingScreen.color = newColor;
        }
    }
}
