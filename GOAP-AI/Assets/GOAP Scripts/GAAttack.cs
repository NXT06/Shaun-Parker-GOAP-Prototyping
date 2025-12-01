using UnityEngine;

public class GAAttack : GOAPAction
{
    public override void ExecuteAction()
    {
        print("attack");
        isCompleted = true; 
    }

    public override void PostPerform()
    {

    }

    public override void PrePerform()
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
