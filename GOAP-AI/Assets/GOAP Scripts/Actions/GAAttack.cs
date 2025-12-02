using UnityEngine;

public class GAAttack : GOAPAction
{

    public float attackCD = 1;
    LayerMask enemyLayer;
    public float attackRange = 2; 

    float timer; 

    public override void ExecuteAction()
    {
        timer += Time.deltaTime; 

        if (timer > attackCD)
        {
            print("attack");
            timer = 0; 
        }

        if(!DetectEnemy(enemyLayer, attackRange))
        {
            isCompleted = true; 
        }
    }

    public override void PostPerform()
    {

    }

    public override void PrePerform()
    {

    }

    public bool DetectEnemy(LayerMask enemyLayer, float radius)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, enemyLayer);

        if (hits.Length > 0)
        {
            return true;
        }

        return false;


    }
}
