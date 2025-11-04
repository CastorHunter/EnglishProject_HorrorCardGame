using UnityEngine;

public class Card_Scarecrow : Card
{
    private int _crows;
        
    public override int Attack()
    {
        _crows += 1;
        return ScaredCrows();
    }

    public override void TakeDamage(int damages)
    {
        health -= MasterOfCrows(damages);
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
