using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour {

    GameManager _gameManager;
    public Transform raycastPoint;
    public float raycastDistance;
    public LayerMask foodLayerMask;
    public Transform foodPrefab; // Will change this for actual food prefab.

    public Transform heldItem;

    public float overlapSphereRadius;

    private Transform selectedObject = null;
    public void InitializePlayerHand(GameManager gameManager) {
        _gameManager = gameManager;
    }

    public void CheckRaycast() {

        LayerMask checkMask = ~(1 << foodLayerMask);

        Collider[] hitColliders = Physics.OverlapSphere(raycastPoint.position, overlapSphereRadius, checkMask);

        int i = 0;
        while(i < hitColliders.Length) {
            if (selectedObject == null)
                selectedObject = hitColliders[i].transform;
            else {
                float distance1 = (hitColliders[i].transform.position - raycastPoint.position).magnitude;
                float distance2 = (selectedObject.transform.position - raycastPoint.position).magnitude;
                if (distance1 < distance2)
                    selectedObject = hitColliders[i].transform;
            }
            i++;
        }
        if (selectedObject != null) {
            if (selectedObject.GetComponent<PrepIngredient>()) {
                if (selectedObject.GetComponent<PrepIngredient>().isWrong) {
                    selectedObject.GetComponent<PrepIngredient>().isWrong = false;
                    heldItem = selectedObject;
                    heldItem.GetComponent<Rigidbody>().isKinematic = true;
                    heldItem.transform.position = raycastPoint.position;
                    heldItem.SetParent(this.transform);
                    selectedObject = null;
                }
            } else {
                if (selectedObject.name != "Lumpia")
                    CreateFoodClone(selectedObject.name);
                else {
                    heldItem = selectedObject;
                    heldItem.transform.position = raycastPoint.position;
                    heldItem.SetParent(this.transform);
                    selectedObject = null;
                }
            }
        }
    }

    void CreateFoodClone(string name) {
        if (name != "Lumpia") {
            Debug.Log("name : " + name);
            Transform clone = Instantiate(foodPrefab, raycastPoint.position, foodPrefab.rotation);
            clone.GetComponent<MeshRenderer>().sharedMaterial = GetMaterial(name);
            clone.name = name;
            heldItem = clone;
            heldItem.SetParent(this.transform);
            selectedObject = null;
        } 
    }
    public void PlayLumpiaMusic() {

    }
    Material GetMaterial(string name) {
        Material newMat = null;
        switch (name) {
            case "Wrapper": {
                    newMat = _gameManager.lumpiaMinigame.purple;
                    break;
                }
            case "Shrimp": {
                    newMat = _gameManager.lumpiaMinigame.red;
                    break;
                }
            case "Vegetable": {
                    newMat = _gameManager.lumpiaMinigame.green;
                    break;
                }
            case "Meat": {
                    newMat = _gameManager.lumpiaMinigame.blue;
                    break;
                }
            
        }
                return newMat;
    }

    
}
