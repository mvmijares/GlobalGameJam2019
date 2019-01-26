using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckObjectCamera : MonoBehaviour {

    #region Data

    [SerializeField] private GameManager _gameManager;
    [SerializeField] private bool actionKey;

    #endregion
    #region Event Data
    public void OnActionKeyPressed() {
        foreach (InteractableObject i in _gameManager.interfactableObjectList) {
            if (i.isInView) {
                Debug.Log("Interacting with " + i.name);
            }
        }
    }
    #endregion

    public void InitializeCheckObjectCamera(GameManager gameManager) {
        _gameManager = gameManager;
        actionKey = false;
        _gameManager.GetPlayer().playerInput.OnActionKeyPressedEvent += OnActionKeyPressed;
    }
    public void OnDestroy() {
        _gameManager.GetPlayer().playerInput.OnActionKeyPressedEvent -= OnActionKeyPressed;
    }
    public void UpdateCheckObjectCamera() {
        foreach(InteractableObject i in _gameManager.interfactableObjectList) {
            if (IsObjectInView(i.GetComponent<Renderer>()))
                i.isInView = true;
            else
                i.isInView = false;
        }
    }
    public bool IsObjectInView(Renderer reference) {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(transform.GetComponent<Camera>());
        if ((GeometryUtility.TestPlanesAABB(planes, reference.bounds)))
            return true;
        else
            return false;
    }
}
