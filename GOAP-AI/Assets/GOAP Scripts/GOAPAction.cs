using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class GOAPAction : MonoBehaviour
{
    public string actionName; 

    public Dictionary<string, int> conditions;
    public Dictionary<string, int> effects;

    public GOAPWorldState[] preConditions;
    public GOAPWorldState[] afterEffects;

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
            if (goals.ContainsKey(kvp.Key))
            {
                return true;
            }

        }

        return false;
    }

    public bool HasGoal(string goal, Dictionary<string, int> action)
    {
        if (action.ContainsKey(goal))
        {
            //print("has key");
            return true;
        }
        else { return false; }

    }

    public abstract void ExecuteAction();
    
}
