using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPopup : MonoBehaviour {

    Camera _camera;

    public void SetCamera(Camera camera) {
        _camera = camera;
    }
    private void Update() {
        if (_camera) {

            transform.forward = _camera.transform.forward;
        }
    }
}
