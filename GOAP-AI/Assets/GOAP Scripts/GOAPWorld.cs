using System.Collections.Generic;
using UnityEngine;

public sealed class GOAPWorld
{
    private static readonly GOAPWorld instance = new GOAPWorld();
    private static GOAPWorldStates world;
    
    static GOAPWorld()
    {
        world = new GOAPWorldStates();
        Debug.Log("assigning world"); 
    }

    private GOAPWorld()
    {

    }

    public static GOAPWorld Instance
    {
        get { return instance; }
    }

    public static GOAPWorldStates GetWorld()
    {
        return world;
    }
}
