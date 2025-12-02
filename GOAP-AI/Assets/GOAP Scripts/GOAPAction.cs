using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;



public abstract class GOAPAction : MonoBehaviour
{
    public string actionName; 

    public Dictionary<string, int> conditions;
    public Dictionary<string, int> effects;

    public GOAPWorldState[] preConditions;
    public GOAPWorldState[] afterEffects;

    public GOAPWorldStates agentBeliefs;

    public NavMeshAgent navMesh;

    public bool isCompleted;

    public float timeOut; 

    public bool Achievable = true; 

    public GOAPAction()
    {
        conditions = new Dictionary<string, int>();
        effects = new Dictionary<string, int>();

        
    }

    public void Awake()
    {

        if (preConditions != null)
        {
            foreach (GOAPWorldState w in preConditions)
            {
                conditions.Add(w.key, w.value);
            }
        }
        if (afterEffects != null)
        {
            foreach (GOAPWorldState w in afterEffects)
            {
                effects.Add(w.key, w.value);
            }
        }
    }

    public bool IsAchievable(Dictionary<string, int> currentAction, Dictionary<string, int> goals)
    {
        foreach (KeyValuePair<string, int> kvp in currentAction)
        {
            print(kvp.Key +  ": " + goals.Keys.First());
            if (goals.ContainsKey(kvp.Key))
            {
                return true;
            }

        }

        return false;
    }

    public bool HasGoal(GOAPWorldStates agentGoals, Dictionary<string, int> action)
    {

        foreach (KeyValuePair<string, int> kvp in action)
        {
            if (agentGoals.states.ContainsKey(kvp.Key))
            {
                return true;
            }
        }
        { return false; }

    }

    public int PriorityCondition()
    {
        int maxValue;
        if(conditions.Values.Count > 0)
        {
             maxValue = conditions.Values.Max();
        }
        else
        {
            return 0; 
        }
            List<int> highestKVPs = new List<int>();

        foreach (KeyValuePair<string, int> kvp in conditions)
        {
            if (kvp.Value == maxValue)
            {
                highestKVPs.Add(kvp.Value); 
            }
        }

        if (highestKVPs.Count == 1)
        {
            return highestKVPs[0];
        }
        else
        {
            int choice = Random.Range(0, highestKVPs.Count);

            return highestKVPs[choice];
        }
    }
    
    //Execuction functions for the action
    public abstract void PrePerform();
    public abstract void ExecuteAction();
    public abstract void PostPerform();
    
}
