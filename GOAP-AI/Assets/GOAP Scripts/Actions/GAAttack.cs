using UnityEngine;

public class GAAttack : GOAPAction
{

    public float attackCD = 1;
    public LayerMask enemyLayer;
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
            print("enemy left range"); 
            isCompleted = true; 
        }
    }

    public override void PostPerform()
    {
        navMesh.isStopped = false;
    }

    public override void PrePerform()
    {

    }

    public bool DetectEnemy(LayerMask enemyLayer, float radius)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, enemyLayer);

        if (hits.Length > 0)
        {
            foreach (Collider c in hits)
            {
                if (c.gameObject == this.gameObject)
                {
                    continue;
                }
                return true;
            }
        }

        return false;


    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, attackRange); 
    }
}
