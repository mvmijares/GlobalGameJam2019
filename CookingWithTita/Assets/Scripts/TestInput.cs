using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Line {
    public Vector3 startPoint;
    public Vector3 endPoint;

    public Line(Vector3 pointA, Vector3 pointB) {
        startPoint = pointA;
        endPoint = pointB;
    }

    public Vector3 GetDirectionNormalized() {
        return (endPoint - startPoint).normalized;
    }
    public float GetMagnitude() {
        return (endPoint - startPoint).magnitude;
    }
    public Vector2 GetNormal() {
        return Vector2.Perpendicular(GetDirectionNormalized()); // Formula needs to be edited
    }
}
/// <summary>
/// We will test for clockwise rotation
/// </summary>

public class TestInput : MonoBehaviour {

    public Transform pointA;
    public Transform pointB;

    Line line;
    Line normalLine;
    [SerializeField] private Vector3 directionVector;
    private float magnitude;
    [SerializeField] private Vector3 normalVector;

    public float timer;

    private void Awake() {
        line = new Line(pointA.position, pointB.position);
        magnitude = line.GetMagnitude();
        
    }
    
    public void Update() {
        line.startPoint = pointA.position;
        line.endPoint = pointB.position;
        directionVector = line.GetDirectionNormalized();
        magnitude = line.GetMagnitude();
        normalVector = line.GetNormal();

        if(Input.GetMouseButton(0)) {
            timer += Time.deltaTime;

        }
        if (Input.GetMouseButtonUp(0)) {
            
        }
       
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;

        if (line != null) {
            //Representation of a line segment
            Gizmos.DrawLine(line.startPoint, line.endPoint);

            Gizmos.color = Color.blue;

            Gizmos.DrawLine(line.startPoint + (line.GetDirectionNormalized() * (magnitude / 2)), normalVector * 1f);
        }
    }
}
