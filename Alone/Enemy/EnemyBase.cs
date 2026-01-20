using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected float damageAmount = 10f;
    [SerializeField] protected float detectRange = 6f;
    [SerializeField] protected float attackRange = 1.5f;

    protected Transform player;
    protected Rigidbody2D rig;
    protected EnemyAnimation anim;

    [SerializeField] private int maxCombo = 3;
    [SerializeField] private float comboResetTime = 1f;
    public int currentCombo;
    private float lastAttackTime;

    [SerializeField] private float attackCooldown = 0.4f;
    private float nextAttackTime;
    public bool CanSeePlayer { get; private set; }
    public bool CanAttackPlayer { get; private set; }
    public bool IsMoving { get; protected set; }

    protected virtual void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<EnemyAnimation>();
    }

    protected virtual void Update()
    {
        UpdatePerception();
    }

    protected void UpdatePerception()
    {
        float dist = Vector2.Distance(transform.position, player.position);

        CanSeePlayer = dist <= detectRange;
        CanAttackPlayer = dist <= attackRange;
    }

    public virtual void MoveToPlayer()
    {
        if (CanAttackPlayer) return;
        ResetCombo();
        float dir = player.position.x > transform.position.x ? 1 : -1;
        rig.velocity = new Vector2(dir * moveSpeed, rig.velocity.y);
        transform.localScale = new Vector3(dir, 1, 1) * 20;

        IsMoving = true;
        anim.SetMove(true);
    }

    public void StopMove()
    {
        rig.velocity = Vector2.zero;
        IsMoving = false;
        anim.SetMove(false);
    }

    public void TryAttack()
    {
        if (Time.time < nextAttackTime) return;
        if (!CanAttackPlayer) return;

        DoComboAttack();
        nextAttackTime = Time.time + attackCooldown;
    }

    protected void DoComboAttack()
    {
        if (Time.time - lastAttackTime > comboResetTime)
        {
            currentCombo = 0;
        }

        anim.PlayAttack(currentCombo);

        currentCombo = (currentCombo + 1) % maxCombo;
        lastAttackTime = Time.time;
       
    }

    public void ResetCombo()
    {
        currentCombo = 0;
    }
    protected abstract void PerformAttack();
}
