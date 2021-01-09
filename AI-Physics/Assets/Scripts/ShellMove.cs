using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellMove : MonoBehaviour
{
  // Start is called before the first frame update
  float speed = 1;
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    this.transform.Translate(0, speed * Time.deltaTime, speed * Time.deltaTime);
  }
}
