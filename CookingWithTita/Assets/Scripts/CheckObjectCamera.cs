using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CheckObjectCamera : MonoBehaviour {

    #region Data

    [SerializeField] private GameManager _gameManager;
    [SerializeField] private bool actionKey;

    #endregion
    #region Event Data
    public event Action<Transform> InteractWithObjectEvent;

    public void OnActionKeyPressed() {
        foreach (InteractableObject i in _gameManager.interfactableObjectList) {
            if (i.isInView) {
                if (InteractWithObjectEvent != null)
                    InteractWithObjectEvent(i.transform);
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
