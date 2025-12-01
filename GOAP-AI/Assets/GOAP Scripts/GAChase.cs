using TMPro;
using UnityEngine;

public class GAChase : GOAPAction
{
    [SerializeField] TextMeshProUGUI textBox;
    public float timer;
    public float maxTime;

    public float stoppingDistance; 

    Transform target; 

    public override void PrePerform()
    {

        if (WorldAssignerTest.player.transform != null)
        {
            target = WorldAssignerTest.player.transform;
            navMesh.destination = target.position;
        }
        else { Achievable = false; }
    }
    public override void ExecuteAction()
    {
        if(navMesh.pathPending)
        {
            return; 
        }
        print(navMesh.remainingDistance + "  " + stoppingDistance);
        if (navMesh.remainingDistance <= stoppingDistance)
        {
            print("reached target");
            navMesh.isStopped = true;
            isCompleted = true; 
        }
    }

    public override void PostPerform()
    {
        navMesh = null;
        textBox.text += " buildings destroyed";
        GOAPWorld.GetWorld().RemoveState("buildingsAlive");
        GOAPWorld.GetWorld().AddState("buildingsDestroyed", 0);
    }
}
