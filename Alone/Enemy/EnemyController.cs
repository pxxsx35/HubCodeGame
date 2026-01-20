using UnityEngine;

public class EnemyController : MonoBehaviour
{

    EnemyAnimation enemyAnimation;
    private EnemyBase enemy;

    private void Awake()
    {
        enemy = GetComponent<EnemyBase>();
    }

    private void Update()
    {
        if (enemy.CanAttackPlayer)
        {
            enemy.StopMove();
            enemy.TryAttack();
            Debug.Log("Attack Player"); 
        }
        else if (enemy.CanSeePlayer)
        {
            Debug.Log("See Player");
            enemy.MoveToPlayer();
        }
        else
        {
            Debug.Log("Stop anim");

            enemy.StopMove();
        }

    }
}
