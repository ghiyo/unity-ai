using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// A very simplistic car driving on the x-z plane.

public class Drive : MonoBehaviour
{
  public float speed = 10.0f;
  public float autoSpeed = 0.1f;
  public float rotationSpeed = 100.0f;

  public GameObject fuel;

  private bool autoPilot = false;

  void Start()
  {

  }

  void CalculateAngle()
  {
    Vector3 tankForwardVector = this.transform.up;
    Vector3 fuelDirection = fuel.transform.position - this.transform.position;
    float dotProduct = (tankForwardVector.x * fuelDirection.x) + (tankForwardVector.y * fuelDirection.y);
    float angle = Mathf.Acos(dotProduct / (tankForwardVector.magnitude * fuelDirection.magnitude));
    Debug.DrawRay(this.transform.position, tankForwardVector * 20, Color.green, 2);
    Debug.DrawRay(this.transform.position, fuelDirection, Color.red, 2);
    int clockwise = 1;
    Debug.Log("Angle: " + angle);
    if (Cross(tankForwardVector, fuelDirection).z < 0) clockwise = -1;
    if (!float.IsNaN(angle))
      this.transform.Rotate(0, 0, (angle * clockwise * Mathf.Rad2Deg) * 0.02f);

    // This is the same as the block of code commented above.
    // float unityAngle = Vector3.SignedAngle(tankForwardVector, fuelDirection, this.transform.forward);
    // this.transform.Rotate(0, 0, unityAngle);
  }

  Vector3 Cross(Vector3 v, Vector3 w)
  {
    float xMult = v.y * w.z - v.z * w.y;
    float yMult = v.z * w.x - v.x * w.z;
    float zMult = v.x * w.y - v.y * w.x;
    return new Vector3(xMult, yMult, zMult);
  }

  float CalculateDistance()
  {
    Vector3 tankPosition = this.transform.position;
    Vector3 fuelPosition = fuel.transform.position;
    // float x = fuelPosition.x - tankPosition.x;
    // float y = fuelPosition.y - tankPosition.y;
    // float distance = Mathf.Sqrt(x * x + y * y);

    // float distance = Mathf.Sqrt(Mathf.Pow(tankPosition.x - fuelPosition.x, 2) + Mathf.Pow(tankPosition.y - fuelPosition.y, 2));

    float distance = Vector2.Distance(tankPosition, fuelPosition);

    Debug.Log("Distance: " + distance);
    return distance;
  }

  void AutoPilot()
  {
    CalculateAngle();
    this.transform.Translate(this.transform.up * autoSpeed, Space.World);
  }

  void Update()
  {
    // Get the horizontal and vertical axis.
    // By default they are mapped to the arrow keys.
    // The value is in the range -1 to 1
    float translation = Input.GetAxis("Vertical") * speed;
    float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

    // Make it move 10 meters per second instead of 10 meters per frame...
    translation *= Time.deltaTime;
    rotation *= Time.deltaTime;

    // Move translation along the object's z-axis
    transform.Translate(0, translation, 0);

    // Rotate around our y-axis
    transform.Rotate(0, 0, -rotation);

    if (Input.GetKeyDown(KeyCode.Space))
    {
      CalculateDistance();
      CalculateAngle();
    }

    if (Input.GetKeyDown(KeyCode.T)) autoPilot = !autoPilot;
    if (autoPilot && CalculateDistance() > 5.0f) AutoPilot();
  }
}