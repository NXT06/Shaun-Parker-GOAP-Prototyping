using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GOAPPlanner : MonoBehaviour
{
    public Queue<GOAPAction> Plan(List<GOAPAction> actions, GOAPWorldStates goals)
    {
        Queue<GOAPAction> queue = new Queue<GOAPAction>();

        List<GOAPAction> usableActions = new List<GOAPAction>();

        List<GOAPAction> startingActions = new List<GOAPAction>();

        print("starting"); 

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

        //Ordering the Actions by priority, this allows permute test through them correctly
        Stack<GOAPAction> startActionsOrdered = CheckForPriority(startingActions);
        

        print($"Starting actions: " + startingActions.Count + ". Usable actions: " + usableActions.Count);
        print(usableActions.Last().actionName); 

        int startCount = startActionsOrdered.Count;

        //Looping through every starting action based of priority and returning the first path
        for (int i = 0; i < startCount; i++)
        {
            queue = OrderActions(usableActions, startActionsOrdered.Pop());

            if (queue != null && queue.Count > 0) { break; } //breaking on first successful path

            else { continue; }
        }
        if (queue != null && queue.Count > 0)
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
        Stack<GOAPAction> usableActionsOrdered = CheckForPriority(usableActions);

        if (usableActions.Count > 0)
        {
            //Attempting to permute 
            for (int i = 0; i < usableActions.Count; i++)
            {
                tempQueue.Clear();

                Permute(usableActionsOrdered, start, start);

                if (tempQueue.Count > 0 && tempQueue.Last().conditions.Count == 0)
                {return  tempQueue;}
                else
                {
                    print("last action had conditions"); 
                }
            }
        }

        return null;

    }

    void Permute(Stack<GOAPAction> usableActions, GOAPAction start, GOAPAction previous)
    {
        Stack<GOAPAction> newActions = new Stack<GOAPAction>(new Stack<GOAPAction>(usableActions));

        print("premuted using: " + previous.actionName); 

        if (!tempQueue.Contains(start))
        {
            tempQueue.Enqueue(start);
        }

        if(previous.conditions.Count == 0)
        {
            return; 
        }

        int count = usableActions.Count;
        print(usableActions.Count); 
        
        for (int i = 0; i < count; i++)
        {
            //Removing the highest priority action and testing it
            GOAPAction action = newActions.Pop();

            //First checking to ensure that the action is viable
            if (action.IsAchievable(action.effects, previous.conditions))
            {
                tempQueue.Enqueue(action);


                //Scanning through every current world state to see if this is a condition for the action
                foreach (KeyValuePair<string, int> kvp in GOAPWorld.GetWorld().states)
                {
                    if (action.conditions.ContainsKey(kvp.Key))
                    {
                        print("contains world state");
                        return; //returns when the action contains a condition that matches world state
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
            else { print(action.actionName + " is not achievable.");  continue; }
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
