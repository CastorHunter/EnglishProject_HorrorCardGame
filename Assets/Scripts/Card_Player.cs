using UnityEngine;

public class Card_Player : Card
{
    public override int Attack()
    {
        return attack;
    }
    
    public override int TakeDamage(int health, int damage)
    {
        health -= damage;
        return health;
    }
}


