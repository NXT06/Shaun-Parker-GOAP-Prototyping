using UnityEngine;

public class GAPatrol : GOAPAction
{

    public float patrolRadius;
    public float patrolSpeed;
    public float waitTime;

    public float alertRadius;
    public LayerMask enemyLayer;

    public string enemiesDeadState; 

    float timer; 

    public override void ExecuteAction()
    {
        MoveAtInterval();

        if(DetectEnemy(enemyLayer, alertRadius))
        {
            isCompleted = true;
        }
        
    }

    public override void PostPerform()
    {
        timer = 0;
    }

    public override void PrePerform()
    {
       // if (GOAPWorld.GetWorld().states.ContainsKey(enemiesDeadState))
        //{
        //    Achievable = false; 
        //}

        navMesh.speed = patrolSpeed; 
        navMesh.destination = RandomPositionInCircle(patrolRadius, transform.position);
    }

    public void MoveAtInterval()
    {
        if(navMesh.remainingDistance < 0.1)
        {
            timer += Time.deltaTime;
        }
        if(timer > waitTime)
        {
            navMesh.destination = RandomPositionInCircle(patrolRadius, transform.position);
            timer = 0;
        }

        
    }

    public Vector3 RandomPositionInCircle(float radius, Vector3 position)
    {
        Vector3 pos;

        float randX = Random.Range(-radius, radius) + position.x;
        float randZ = Random.Range(-radius, radius) + position.z;

        pos = new Vector3(randX, 0, randZ); 

        return pos;
    }

    public bool DetectEnemy(LayerMask enemyLayer, float radius)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, enemyLayer);

        if(hits.Length > 0)
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
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, patrolRadius);

        Gizmos.color = Color.red; 

        Gizmos.DrawWireSphere(transform.position, alertRadius);
    }
}
