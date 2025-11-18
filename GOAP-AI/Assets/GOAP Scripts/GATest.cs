using TMPro;
using UnityEngine;

public class GATest : GOAPAction
{
    [SerializeField] TextMeshProUGUI textBox; 
    public override void PrePerform()
    {
        throw new System.NotImplementedException();
    }
    public override void ExecuteAction()
    {
        textBox.text += " buildings destroyed";
        GOAPWorld.GetWorld().RemoveState("buildingsAlive");
        GOAPWorld.GetWorld().AddState("buildingsDestroyed", 0);
    }

    public override void PostPerform()
    {
        throw new System.NotImplementedException();
    }
}
