using UnityEngine;
using TMPro;

public class Card_Player : Card
{
    private State _playerState;
    private int _baseSpeed, _baseAttack;
    [SerializeField]
    private TextMeshProUGUI _playerStateText;

    private void Start()
    {
        SetState(State.Normal);
    }
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
        _playerStateText.text = ("State : " + _playerState.ToString());
    }

    public State GetState()
    {
        return _playerState;
    }

    public void Fear(int fearLevel)
    {
        attack -= fearLevel*5;
        if (attack < 0)
        {
            attack = 0;
        }
        speed -= fearLevel*5;
        if (speed < 0)
        {
            speed = 0;
        }
        SetState(State.Feared);
    }

    public void ClearState()
    {
        SetState(State.Normal);
        attack = _baseAttack;
    }
}

