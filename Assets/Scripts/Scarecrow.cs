using UnityEngine;
using System.Collections;

public class Scarecrow : Entity
{
    private int _crows, _timeBeforeFear = 2;
    private int _shieldCrowsToText;
        
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
                    _shieldCrowsToText = shieldCrows;
                    StartCoroutine(DefendText());
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

    private IEnumerator DefendText()
    {
        gameManagerBehavior.scarecrowCrowsDefendText.text = ("The Scarecrow sacrificed " + _shieldCrowsToText + " crow(s) to prevent from death!");
        yield return new WaitForSeconds(3);
        gameManagerBehavior.scarecrowCrowsDefendText.text = "";
    }
}
