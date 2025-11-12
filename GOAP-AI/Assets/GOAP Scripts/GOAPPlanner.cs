
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Loading;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

public class GOAPPlanner : MonoBehaviour
{
    public Queue<GOAPAction> Plan(List<GOAPAction> actions, string goal, WorldState states)
    {
        Queue<GOAPAction> queue = new Queue<GOAPAction>();

        List<GOAPAction> usableActions = new List<GOAPAction>();
        
        
        List<GOAPAction> startingActions = new List<GOAPAction>();

        foreach (GOAPAction useAction in actions)
        {
            //Sorting actions into starting nodes and regular actions
            if (useAction.HasGoal(goal, useAction.effects))
            {
                startingActions.Add(useAction);
            }
            else
            {
                usableActions.Add(useAction);
            }
        }
        print($"Starting actions: " + startingActions.Count + ". Usable actions: " + usableActions.Count);

        //Looping through every starting action and returning the first path
        foreach (GOAPAction startAction in startingActions)
        {
            queue = PlanActions(usableActions, startAction);

            if(queue.Count > 0) { print("successful plan"); break; }

            else { continue; }
        }
        if(queue.Count > 0)
        {
            print($"Returning queue with " + queue.Count + " actions");
        }

        
        return queue; 
    }

    Queue<GOAPAction> tempQueue = new Queue<GOAPAction> (); 
    Queue<GOAPAction> PlanActions(List<GOAPAction> usableActions, GOAPAction start)
    {
        List<GOAPAction> remainingActions = usableActions; 

        if (remainingActions.Count > 0)
        {
            //tempQueue.Enqueue(start);

            foreach (GOAPAction action in remainingActions)
            {
               Permute(remainingActions, start, start);
            }
            if (tempQueue.Count > 0) { return tempQueue; }
        }

        return tempQueue;

    }

    void Permute(List<GOAPAction> usableActions, GOAPAction start, GOAPAction previous)
    {
        if (!tempQueue.Contains(start))
        {
            tempQueue.Enqueue(start);
        }

        foreach (GOAPAction action in usableActions)
        {

            if (action.IsAchievable(action.effects, previous.conditions))
            {
                tempQueue.Enqueue(action);

                if(action.conditions != null)
                {
                    Permute(usableActions, start, action);
                }
            }
            else { continue; }
        }

        

    }

}
