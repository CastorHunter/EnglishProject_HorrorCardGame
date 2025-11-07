using UnityEngine;

public class Mermaid : Entity
{
    private int _turnsBeforeWisdom = 2;
    public override int Attack()
    {
        if (gameManagerBehavior.player.GetState() != State.AbyssalEnd)
        {
            gameManagerBehavior.player.SetState(State.AbyssalEnd);
        }
        else
        {
            if (_turnsBeforeWisdom <= 0)
            {
                _turnsBeforeWisdom = 2;
                print("Mermaid is using Wisdom");
            }
            print("Mermaid is singing");
        }
        _turnsBeforeWisdom -= 1;
        return 0;
    }
}
