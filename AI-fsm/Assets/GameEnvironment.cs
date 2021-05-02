using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnvironment : MonoBehaviour
{
  private static GameEnvironment instance;
  private List<GameObject> _checkPoints = new List<GameObject>();
  public List<GameObject> checkPoints = new List<GameObject>();

  public static GameEnvironment Singleton
  {
    get
    {
      if (instance == null)
      {
        instance = new GameEnvironment();
        instance.checkPoints.AddRange(GameObject.FindGameObjectsWithTag("Checkpoint"));
      }
      return instance;
    }
  }
}
