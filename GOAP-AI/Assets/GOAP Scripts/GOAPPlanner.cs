
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Loading;
using UnityEditor.Search;
using UnityEngine;

public class GOAPPlanner : MonoBehaviour
{
    public string goal; 

    public Queue<GOAPAction> Plan(List<GOAPAction> actions, string goal, WorldState states)
    {
        Queue<GOAPAction> queue = new Queue<GOAPAction>();

        List<GOAPAction> usableActions = new List<GOAPAction>();
        
        List<GOAPAction> startingActions = new List<GOAPAction>();

        foreach (GOAPAction useAction in actions)
        {
            //Searching through actions to find if any complete the goal state
            if (useAction.HasGoal(goal, useAction.effects))
            {
                startingActions.Add(useAction);
                usableActions.Remove(useAction);
                
            }
            else { print("NO ACTION WITH GOAL"); queue = null; break; }
        }
        print($"Number of starting actions: " + startingActions.Count);

        //Looping through every starting action
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
        tempQueue.Enqueue(start);
        print(tempQueue.Count);

        return tempQueue;

        List<GOAPAction> remainingActions = usableActions; 

        GOAPAction lastAction = start;

        if (remainingActions.Count > 0)
        {
            foreach (GOAPAction action in remainingActions)
            {
                if (action.IsAchievable(action.effects, lastAction.conditions))
                {
                    continue;
                }

                Permute(remainingActions, lastAction);
            }

            if (tempQueue != null) { return tempQueue; }
        }

        return tempQueue;

    }

    void Permute(List<GOAPAction> usableActions, GOAPAction previous)
    {

        GOAPAction nextAction = null; 

        foreach (GOAPAction action in usableActions)
        {
            //Skipping if the action alread
            if (tempQueue.Contains(previous)) { continue; }

            if (action.IsAchievable(action.effects, previous.conditions))
            {
                nextAction = action;
                tempQueue.Enqueue(nextAction);

                if(action.conditions != null)
                {
                    Permute(usableActions, action);
                }
            }
            else { continue; }
        }

    }

}
