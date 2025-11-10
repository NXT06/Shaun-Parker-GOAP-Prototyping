using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class GAction : MonoBehaviour
{
    public string actionName = "Action";
    [Tooltip("Used by the planner when finding the cheapest path to the goal")] 
    public float cost = 1f;
    [Tooltip("Location action will take place")] 
    public GameObject target;
    public string targetTag;
    [Tooltip("How long the agent should spent doing the action")]
    public float duration = 0;
    public WorldState[] preConditions;
    public WorldState[] afterEffects; 
    public NavMeshAgent agent;

    public Dictionary<string, int> conditions;
    public Dictionary<string, int> effects;

    public WorldStates agentBeliefs;

    public bool running = false; 

    public GAction()
    {
        conditions = new Dictionary<string, int>();
        effects = new Dictionary<string, int>();
    }

    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if(preConditions != null)
        {
            foreach(WorldState w in preConditions)
            {
                conditions.Add(w.key, w.value);
            }
        }
        if(afterEffects != null)
        {
            foreach(WorldState w in afterEffects)
            {
                effects.Add(w.key, w.value);
            }
        }
    }

    public bool isAchievable()
    {
        return true; 
    }

    public bool IsAchievableGiven(Dictionary<string, int> thisConditions)
    {
        foreach(KeyValuePair<string, int> p in conditions)
        {
            if (!thisConditions.ContainsKey(p.Key))
            {
                return false;
            }
        }
        return true; 
    }

    public abstract bool PrePerform();
    public abstract bool PostPerform();
}
