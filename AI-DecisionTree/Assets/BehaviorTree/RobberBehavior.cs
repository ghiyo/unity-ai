using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobberBehavior : MonoBehaviour
{

  BehaviorTree tree;
  public GameObject diamond;
  public GameObject van;
  public GameObject backDoor;
  public GameObject frontDoor;
  NavMeshAgent agent;

  public enum ActionState { IDLE, WORKING };

  [Range(0, 1000)]
  public int money = 800;
  ActionState state = ActionState.IDLE;

  Node.Status treeStatus = Node.Status.RUNNING;
  // Start is called before the first frame update
  void Start()
  {
    agent = this.GetComponent<NavMeshAgent>();
    tree = new BehaviorTree();
    Sequence steal = new Sequence("Steal Something");
    Leaf goToBackDoor = new Leaf("Go To Back Door", () => GoToDoor(backDoor));
    Leaf goToFrontDoor = new Leaf("Go To Front Door", () => GoToDoor(frontDoor));
    Leaf goToDiamond = new Leaf("Go To Diamond", () =>
    {
      Node.Status s = GoToLocation(diamond.transform.position);
      if (s == Node.Status.SUCCESS)
      {
        diamond.transform.parent = this.gameObject.transform;
      }
      return s;
    });
    Leaf needsMoney = new Leaf("Needs Money", () => { if (money >= 500) return Node.Status.FAILURE; else return Node.Status.SUCCESS; });
    Leaf goToVan = new Leaf("Go To Van", () =>
    {
      Node.Status s = GoToLocation(van.transform.position);
      if (s == Node.Status.SUCCESS)
      {
        money += 300;
        Destroy(diamond);
      }
      return s;
    });
    Selector openDoor = new Selector("Open Door");
    openDoor.AddChild(goToBackDoor);
    openDoor.AddChild(goToFrontDoor);

    steal.AddChild(needsMoney);
    steal.AddChild(openDoor);
    steal.AddChild(goToDiamond);
    steal.AddChild(goToVan);
    tree.AddChild(steal);

    tree.PrintTree();
    Time.timeScale = 5;
  }

  Node.Status GoToLocation(Vector3 destination)
  {
    float distanceToTarget = Vector3.Distance(destination, this.transform.position);
    if (state == ActionState.IDLE)
    {
      agent.SetDestination(destination);
      state = ActionState.WORKING;
    }
    else if (Vector3.Distance(agent.pathEndPosition, destination) >= 2)
    {
      state = ActionState.IDLE;
      return Node.Status.FAILURE;
    }
    else if (distanceToTarget < 2)
    {
      state = ActionState.IDLE;
      return Node.Status.SUCCESS;
    }
    return Node.Status.RUNNING;
  }

  public Node.Status GoToDoor(GameObject door)
  {
    Node.Status s = GoToLocation(door.transform.position);
    if (s == Node.Status.SUCCESS)
    {
      if (!door.GetComponent<Lock>().isLocked)
      {
        door.SetActive(false);
        return Node.Status.SUCCESS;
      }
      else return Node.Status.FAILURE;
    }
    else return s;
  }

  // Update is called once per frame
  void Update()
  {
    if (treeStatus != Node.Status.SUCCESS)
      treeStatus = tree.Process();
  }
}
