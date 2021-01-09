using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToGoal : MonoBehaviour
{
  public float speed = 2.0f;
  public float accuracy = 0.2f;
  public Transform goal;
  // Start is called before the first frame update
  void Start()
  {
  }

  // Update is called once per frame
  void LateUpdate()
  {
    this.transform.LookAt(goal.position);
    Vector3 direction = goal.position - this.transform.position;
    Debug.DrawRay(this.transform.position, direction, Color.red);
    if (direction.magnitude > accuracy)
    {
      this.transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
    }
  }
}
