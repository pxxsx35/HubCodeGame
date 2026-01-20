using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    public Animator animator;

    private static readonly int RunHash = Animator.StringToHash("Run");
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int AttackIndexHash = Animator.StringToHash("AttackIndex");

    public void SetMove(bool value)
    {
        animator.SetBool(RunHash, value);
    }

    public void PlayAttack(int comboIndex)
    {
        animator.SetInteger(AttackIndexHash, comboIndex);
        animator.SetTrigger(AttackHash);
    }

}
