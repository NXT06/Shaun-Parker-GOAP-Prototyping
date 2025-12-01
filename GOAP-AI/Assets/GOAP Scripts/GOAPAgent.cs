using NUnit.Framework;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.AI;
using TMPro;

public class GOAPAgent : MonoBehaviour
{
    public string goal;

    GOAPPlanner planner; 

    public List<GOAPAction> action = new List<GOAPAction>();
    
    Queue<GOAPAction> actionQueue = null;

    public GOAPAction currentAction;

    bool running = false;

    public NavMeshAgent navMesh;

    public GOAPWorldState[] agentGoals; 
    
    GOAPWorldStates goalStates = new GOAPWorldStates();

    [SerializeField] TextMeshProUGUI plantext; 
    [SerializeField] TextMeshProUGUI worldStateText; 
    [SerializeField] TextMeshProUGUI actionEffectText; 

    private void Awake()
    {
        foreach (GOAPWorldState w in agentGoals)
        {
            goalStates.states.Add(w.key, w.value);
        }
        action = GetComponents<GOAPAction>().ToList();

        plantext.text = "Action Queue: ";
        worldStateText.text = "World States: ";
        actionEffectText.text = "After Effect: ";


    }
    private void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !running)
        {
            actionQueue = null;
            running = true;
            GetPlan();
            

            foreach (var state in GOAPWorld.GetWorld().states)
            {
                worldStateText.text += "" + state.Key.ToString() + ", ";
            }
        }

        if (running == true)
        {
            ExecutePlan(actionQueue); 
        }


    }
    void GetPlan()
    {
        if(actionQueue == null && action.Count > 0)
        {
            planner = new GOAPPlanner();

            print("Requesting Plan");
            actionQueue = planner.Plan(action, goalStates);
        }
    }

    void RePlan()
    {
        actionQueue.Clear();
        currentAction = null; 

        actionQueue = planner.Plan(action, goalStates);


    }

    void ExecutePlan(Queue<GOAPAction> actionQueue)
    {
        if (currentAction == null)
        {
            if (actionQueue.Count > 0 && actionQueue != null)
            {
                running = true; 
                currentAction = actionQueue.Dequeue();
                plantext.text += "" + currentAction.actionName + ", ";
                currentAction.navMesh = navMesh; 
                currentAction.PrePerform(); 
                print($"Executed: " + currentAction.actionName);
            }
            else { running = false; }
            
        }
        if (currentAction.isCompleted)
        {
            print("post performed");
            currentAction.PostPerform();
            currentAction = null; 
        }

        else if (!currentAction.isCompleted)
        {
            if (currentAction.Achievable)
            {

                currentAction.ExecuteAction();

            }
            else
            {
                currentAction = null;
                RePlan();
            }
        }

    }


}
