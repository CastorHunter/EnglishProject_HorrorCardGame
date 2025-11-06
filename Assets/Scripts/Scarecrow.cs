using UnityEngine;

public class Scarecrow : Entity
{
    private int _crows, _timeBeforeFear = 2;
        
    public override int Attack()
    {
        print(_timeBeforeFear);
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
        /*health -= MasterOfCrows(damages);*/
        
        //variante en attente du debug
        targetHealth -= damages;
        
        return targetHealth;
    }

    private int MasterOfCrows(int damages)
    {
        for (int i = 0; i < _crows; i++)
        {
            if (i == _crows - 1)
            {
                return _crows;
            }
        }
        return damages;
    }

    private int ScaredCrows()
    {
        return _crows*attack;
    }
}
