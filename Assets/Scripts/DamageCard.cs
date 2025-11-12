using UnityEngine;

public class DamageCard : Card
{
    [SerializeField]
    private int _damageMultiplier = 1, _damageToAdd = 0;
    public override void CardAction()
    {
        gameManagerBehavior.StartCoroutine(gameManagerBehavior.AttackAnimation());
        gameManagerBehavior.currentEnemyHealth=gameManagerBehavior.enemyEntity.TakeDamage(gameManagerBehavior.currentEnemyHealth, gameManagerBehavior.player.Attack()*_damageMultiplier+_damageToAdd);
        if (gameManagerBehavior.currentEnemyHealth < 0)
        {
            gameManagerBehavior.currentEnemyHealth = 0;
        }
    }
}
