using UnityEngine;

public class Scarecrow : Entity
{
    private int _crows, _timeBeforeFear = 2;
        
    public override int Attack()
    {
        if (_timeBeforeFear <= 0)
        {
            if (gameManagerBehavior != null)
            {
                gameManagerBehavior.player.ApplyFear(_crows);
                _timeBeforeFear = 2;
            }
            return 0;
        }
        else
        {
            _timeBeforeFear -= 1;
            _crows += 1;
            return ScaredCrows();
        }
    }

    public override int TakeDamage(int targetHealth, int damages)
    {
        targetHealth -= MasterOfCrows(damages, targetHealth);
        return targetHealth;
    }

    private int MasterOfCrows(int damages, int healthToCompare, int shieldCrows = 0)
    {
        if (healthToCompare <= damages)
        {
            for (int i = 0; i < _crows; i++)
            {
                if (healthToCompare+2*i >= damages)
                {
                    shieldCrows = (i+1);
                    _crows -= (i+1);
                    print("The Scarecrow sacrificed " + shieldCrows + " crow(s) to prevent from death !");
                    return damages - shieldCrows*2;
                }
            }
        }
        return damages;
    }

    private int ScaredCrows()
    {
        return _crows*attack;
    }
}
