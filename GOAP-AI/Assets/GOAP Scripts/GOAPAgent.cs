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
    [SerializeField] TextMeshProUGUI goaltext; 
    [SerializeField] TextMeshProUGUI currentActionText; 

    private void Awake()
    {
        foreach (GOAPWorldState w in agentGoals)
        {
            goalStates.states.Add(w.key, w.value);
        }
        action = GetComponents<GOAPAction>().ToList();

        plantext.text = "";


    }
    private void LateUpdate()
    {
        if(actionQueue == null)
        {
            actionQueue = null;
            running = true;
            GetPlan();
            
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

            if(actionQueue.Count == 0)
            {
                actionQueue = null;
                print("No available plan");
                return; 
            }

            Queue<GOAPAction> printPlan = new Queue<GOAPAction>();

            printPlan = actionQueue; 

            foreach (GOAPAction action in printPlan)
            {
                plantext.text += "| " + action.actionName + " "; 
            }
            plantext.text += "|";

            goaltext.text = actionQueue.Last().afterEffects.ElementAt(0).key; 
        }
    }

    void RePlan()
    {
        print("replan");
        actionQueue = null; 
        currentAction = null; 

        actionQueue = planner.Plan(action, goalStates);


    }

    float actionTimer; 
    void ExecutePlan(Queue<GOAPAction> actionQueue)
    {
        if (currentAction == null && actionQueue != null)
        {
            if (actionQueue.Count > 0 && actionQueue != null)
            {
                running = true; 
                currentAction = actionQueue.Dequeue();
                currentActionText.text = currentAction.actionName;
                currentAction.navMesh = navMesh; 
                currentAction.PrePerform(); 
                print($"Executed: " + currentAction.actionName);

                
            }
            else { print("No Viable Plan"); plantext.text = "No Plan";  running = false;}
            
        }

        
        if (currentAction != null && currentAction.timeOut > 0)
        {
            actionTimer += Time.deltaTime;
            print("Time remaining for action: " + actionTimer +" / " + currentAction.timeOut); 

            if (actionTimer > currentAction.timeOut)
            {
                print("timer up"); 
                RePlan();
                actionTimer = 0;
            }
        }

        if(currentAction != null && !currentAction.Achievable)
        {
            RePlan();
            actionTimer = 0; 
        }

        if (currentAction != null && currentAction.isCompleted)
        {
            currentAction.isCompleted = false;
            print("post performed");
            currentAction.PostPerform();
            currentAction = null;

            if (actionQueue.Count <= 0)
            {
                RePlan(); 
            }
        }
        else if (currentAction != null) 
        {
            if (currentAction.Achievable)
            {

                currentAction.ExecuteAction();

            }
            else
            {
                currentAction = null;
                actionTimer = 0;
                RePlan();
            }
        }

        

    }


}
