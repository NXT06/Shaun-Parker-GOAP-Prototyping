using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class WorldAssignerTest : MonoBehaviour
{
    public string[] worldStates;

    public static GameObject player;
    
    public GameObject thisPlayer; 

    [SerializeField] TextMeshProUGUI worldstateText; 

    GOAPWorldStates currentWorld; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GOAPWorld.GetWorld().states.Add("playerAlive", 0); 
        GOAPWorld.GetWorld().states.Add("buildingsAlive", 0); 
        
        

        foreach (var state in GOAPWorld.GetWorld().states)
        {
            worldstateText.text += "" + state.Key + ", ";
        }
    }

    // Update is called once per frame
    void Update()
    {
        player = thisPlayer; 
        worldStates = GOAPWorld.GetWorld().states.Keys.ToArray<string>();
    }
}
