using UnityEngine;

public class Krasue : Entity
{
    public override int Attack()
    {
        print("Krasue uses Wisdom after giving a poison card"); //ability 1
        gameManagerBehavior.player.AddState(State.Poisoned); //ability 2
        return attack;
    }
}
