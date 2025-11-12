using NUnit.Framework;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class GOAPAgent : MonoBehaviour
{
    public string goal;

    GOAPPlanner planner; 

    public List<GOAPAction> action = new List<GOAPAction>();
    
    Queue<GOAPAction> actionQueue = null;

    public GOAPAction currentAction;

    bool running = false;   

    private void Awake()
    {
        action = GetComponents<GOAPAction>().ToList(); 
    }
    private void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !running)
        {
            running = true;
            GetPlan();
            ExecutePlan(actionQueue); 
        }
    }
    void GetPlan()
    {
        if(actionQueue == null && action.Count > 0)
        {
            planner = new GOAPPlanner();

            print("Requesting Plan"); 
            actionQueue = planner.Plan(action, goal, null);
        }
    }

    void ExecutePlan(Queue<GOAPAction> actionQueue)
    {
        while(actionQueue.Count > 0) 
        {
            currentAction = actionQueue.Dequeue();
            currentAction.ExecuteAction();
            print($"Executed: " + currentAction.actionName);
        }
    }


}
