using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

[System.Serializable]
public class GOAPWorldState
{
    public string key;
    public int value;
   
}
public class GOAPWorldStates
{
    //Code Obtained from Holistic3D's GOAP tutorial

    public Dictionary<string, int> states;
    public GOAPWorldStates()
    {
        states = new Dictionary<string, int>();
    }

    public bool HasState(string key)
    {
        return states.ContainsKey(key);
    }

    public void AddState(string key, int value)
    {
        states.Add(key, value);
    }

    public void ModifyState(string key, int value)
    {
        if (states.ContainsKey(key))
        {
            states[key] += value;
            if (states[key] <= 0)
            {
                RemoveState(key);
            }
            else
            {
                states.Add(key, value);
            }
        }
    }

    public void RemoveState(string key)
    {
        if (states.ContainsKey(key))
        {
            states.Remove(key);
        }
    }

    public void SetState(string key, int value)
    {
        if (states.ContainsKey(key))
        {
            states[key] = value;
        }
        else
        {
            states.Add(key, value);
        }
    }

    public Dictionary<string, int> GetStates()
    {
        return states;
    }


}
