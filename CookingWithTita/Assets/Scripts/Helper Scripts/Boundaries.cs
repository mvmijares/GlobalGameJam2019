using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary {
    Vector2 bottomLeft;
    public Vector2 BottomLeft { get { return bottomLeft; } set { bottomLeft = value; } }

    Vector2 bottomRight;
    public Vector2 BottomRight { get { return bottomRight; } set { bottomRight = value; } }

    Vector2 topLeft;
    public Vector2 TopLeft { get { return topLeft; } set { topLeft = value; } }

    Vector2 topRight;
    public Vector2 TopRight { get { return topRight; } set { topRight = value; } }

    public Boundary() {
        bottomLeft = new Vector2();
        bottomRight = new Vector2();
        topLeft = new Vector2();
        topRight = new Vector2();
    }

    public void SetWorldBoundaries(Vector2 bottomLeft, Vector2 bottomRight, Vector2 topLeft, Vector2 topRight) {
        this.bottomLeft = bottomLeft;
        this.bottomRight = bottomRight;
        this.topLeft = topLeft;
        this.topRight = topRight;
    }

}

public class Boundaries : MonoBehaviour {

    [SerializeField]
    Boundary cameraBoundaries;
    public Boundary GetWorldBoundary() { return cameraBoundaries; }

    float minVerticalExtents;
    float minHorizontalExtents;
    [SerializeField]
    float worldSize;

    Camera _camera;
    Vector3 target;
    Transform position;

    public void SetCamera(Camera camera) {
        _camera = camera;
        target = camera.transform.position - (camera.transform.forward * 2f);
        Vector3 p = _camera.ViewportToScreenPoint((new Vector3(0.5f, 0f, _camera.nearClipPlane)));
        p = _camera.ScreenToWorldPoint(p);
        minVerticalExtents = (p - target).magnitude;
        minHorizontalExtents = minVerticalExtents * Screen.width / Screen.height;
        SetWorldBoundaries();
    }

    public void UpdateCameraBoundaries() {
        UpdateBoundaries();
    }
    void UpdateBoundaries() {
        cameraBoundaries.BottomLeft = new Vector2(target.x + -minHorizontalExtents, target.y + -minVerticalExtents);
        cameraBoundaries.BottomRight = new Vector2(target.x + minHorizontalExtents, target.y + -minVerticalExtents);
        cameraBoundaries.TopLeft = new Vector2(target.x + -minHorizontalExtents, target.y + minVerticalExtents);
        cameraBoundaries.TopRight = new Vector2(target.x + minHorizontalExtents, target.y + minVerticalExtents);
    }
    void SetWorldBoundaries() {
        //Need to read up on camera views
        cameraBoundaries = new Boundary();

        Vector2 bottomLeft = new Vector2(target.x + -minHorizontalExtents, target.y + -minVerticalExtents);
        Vector2 bottomRight = new Vector2(target.x + minHorizontalExtents, target.y + -minVerticalExtents);
        Vector2 topLeft = new Vector2(target.x + -minHorizontalExtents, target.y + minVerticalExtents);
        Vector2 topRight = new Vector2(target.x + minHorizontalExtents, target.y + minVerticalExtents);

        cameraBoundaries.SetWorldBoundaries(bottomLeft, bottomRight, topLeft, topRight);
    }
}