using UnityEngine;

public class HealCard : Card
{
    [SerializeField]
    private int _heal;
    
    public override void CardAction()
    {
        gameManagerBehavior.currentPlayerHealth=gameManagerBehavior.player.TakeHeal(gameManagerBehavior.currentPlayerHealth, _heal);
    }
}
