using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public enum ImageState { None, Cooking, With, Tita, Image, Ready}
public class TitleScreen : MonoBehaviour {

    private GameManager _gameManager;

    public Image CookingText;
    public Image WithText;
    public Image TitaText;
    public Image TitaImage;
    public Image ActionKeyImage;
    public Image backgroundImage;

    List<Image> imageList;
    [SerializeField] Animator anim;
    ImageState imageState;

    public bool actionKeyPressed;

    public float maxFadeTime = 2f;
    float currentFadeTime = 0.0f;

    public void OnActionKeyPressedCalled() {
        if(imageState == ImageState.Ready) {
            actionKeyPressed = true;
        }
    }
    public void InitializeTitleScreen(GameManager gameManager) {
        _gameManager = gameManager;
        actionKeyPressed = false;
        imageState = ImageState.Cooking;
        imageList = new List<Image>();
        ActionKeyImage.gameObject.SetActive(false);

        imageList.Add(CookingText);
        imageList.Add(WithText);
        imageList.Add(TitaText);
        imageList.Add(TitaImage);
        imageList.Add(ActionKeyImage);
        imageList.Add(backgroundImage);

        _gameManager.GetPlayer().playerInput.OnActionKeyPressedEvent += OnActionKeyPressedCalled;

    }
    private void OnDestroy() {
        _gameManager.GetPlayer().playerInput.OnActionKeyPressedEvent -= OnActionKeyPressedCalled;
    }
    public void UpdateTitleScreen() {
        switch (imageState) {
            case ImageState.Cooking: {
                    anim = CookingText.GetComponent<Animator>();
                    anim.SetBool("Play", true);
                    if (CookingText.GetComponent<UserInterfaceAnimation>().state == AnimationState.IsFinished) {
                        anim.SetBool("Play", false);
                        anim = null;
                        imageState = ImageState.With;
                    }
                    break;
                }
            case ImageState.With: {
                    anim = WithText.GetComponent<Animator>();
                    anim.SetBool("Play", true);
                    if (WithText.GetComponent<UserInterfaceAnimation>().state == AnimationState.IsFinished) {
                        anim.SetBool("Play", false);
                        anim = null;
                        imageState = ImageState.Tita;
                    }
                    break;
                }
            case ImageState.Tita: {
                    anim = TitaText.GetComponent<Animator>();
                    anim.SetBool("Play", true);
                    if (TitaText.GetComponent<UserInterfaceAnimation>().state == AnimationState.IsFinished) {
                        anim.SetBool("Play", false);
                        anim = null;
                        imageState = ImageState.Image;
                    }
                    break;
                }
            case ImageState.Image: {
                    anim = TitaImage.GetComponent<Animator>();
                    anim.SetBool("Play", true);
                    if (TitaImage.GetComponent<UserInterfaceAnimation>().state == AnimationState.IsFinished) {
                        imageState = ImageState.Ready;
                        anim.SetBool("Play", false);
                        anim = null;
                
                    }
                    break;
                }
            case ImageState.Ready: {
                    ActionKeyImage.gameObject.SetActive(true);
                    if (actionKeyPressed) {
                        foreach(Image i in imageList) {
                            float alpha = i.color.a;

                            if (i.color.a > 0)
                                alpha -= Time.deltaTime;
                            else {
                                _gameManager.miniGame = MiniGame.Exploration;
                            }
                            Color c = new Color(i.color.r, i.color.g, i.color.b, alpha);
                            
                            i.color = c;
                        }
                    }
                    break;
                }
        }
    }


}
