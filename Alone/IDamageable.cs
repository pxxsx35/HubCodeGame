public interface IDamageable
{
    void TakeDamage(float damageAmount);
    void ApplyPoison(float totalDamage, float duration);
}