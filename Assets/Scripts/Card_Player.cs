using UnityEngine;

public class Card_Player : Card
{
    private State _playerState;
    private int _baseSpeed, _baseAttack;
    public override int Attack()
    {
        if (_playerState == State.Surprised)
        {
            return 0;
            SetState(State.Normal);
        }
        return attack;
    }
    
    public override int TakeDamage(int currentHealth, int damage)
    {
        currentHealth -= damage;
        return currentHealth;
    }

    public void SetState(State newState)
    {
        _playerState = newState;
    }

    public State GetState()
    {
        return _playerState;
    }

    public void Fear(int fearLevel)
    {
        
    }

    public void ClearState()
    {
        SetState(State.Normal);
        attack = _baseAttack;
    }
}

