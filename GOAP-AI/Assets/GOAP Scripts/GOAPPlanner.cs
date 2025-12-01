using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GOAPPlanner : MonoBehaviour
{
    public Queue<GOAPAction> Plan(List<GOAPAction> actions, GOAPWorldStates goals)
    {
        Queue<GOAPAction> queue = new Queue<GOAPAction>();

        List<GOAPAction> usableActions = new List<GOAPAction>();

        List<GOAPAction> startingActions = new List<GOAPAction>();


        foreach (GOAPAction useAction in actions)
        {
            //Sorting actions into starting nodes and regular actions
            if (useAction.HasGoal(goals, useAction.effects))
            {
                startingActions.Add(useAction);
            }
            else
            {
                usableActions.Add(useAction);

            }
        }

        Stack<GOAPAction> startActionsOrdered = CheckForPriority(startingActions);

        print($"Starting actions: " + startingActions.Count + ". Usable actions: " + usableActions.Count);

        int startCount = startActionsOrdered.Count;

        //Looping through every starting action based of priority and returning the first path
        for (int i = 0; i < startCount; i++)
        {
            queue = OrderActions(usableActions, startActionsOrdered.Pop());

            if (queue.Count > 0) { print("successful plan"); break; } //breaking on first successful path

            else { continue; }
        }
        if (queue.Count > 0)
        {
            queue = new Queue<GOAPAction>(queue.Reverse());

            print($"Returning queue with " + queue.Count + " actions");
        }


        return queue;
    }

    Queue<GOAPAction> tempQueue = new Queue<GOAPAction>();
    Stack<Queue<GOAPAction>> potientialPaths;
    Queue<GOAPAction> OrderActions(List<GOAPAction> usableActions, GOAPAction start)
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

            //First checking to ensure that the action is viable
            if (action.IsAchievable(action.effects, previous.conditions))
            {
                tempQueue.Enqueue(action);


                //Scanning through every current world state to see if this goal 
                foreach (KeyValuePair<string, int> kvp in GOAPWorld.GetWorld().states)
                {
                    if (action.conditions.ContainsKey(kvp.Key))
                    {
                        return;
                    }
                }
                //Ensuring the action still has a condition (otherwise the plan is complete)
                if (action.conditions != null)
                {
                    Permute(usableActions, start, action);
                    break;


                }
                else { return; }
            }
            else { continue; }
        }



    }

    Stack<GOAPAction> CheckForPriority(List<GOAPAction> actions)
    {
        
        Stack<GOAPAction> actionPrios = new Stack<GOAPAction>();

        Dictionary<GOAPAction, int> actionDict = new Dictionary<GOAPAction, int>();


        foreach (GOAPAction action in actions)
        {
            actionDict.Add(action, action.PriorityCondition());
        }

        int count = actionDict.Count;

        for (int i = 0; i < count; i++)
        {
            actionPrios.Push(actionDict.OrderBy(i => i.Value).First().Key);
            actionDict.Remove(actionDict.OrderBy(i => i.Value).First().Key); 
        }

        if (actionPrios != null)
        {
            return actionPrios;
        }

        return null;


    }

}
