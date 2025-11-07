using UnityEngine;
using TMPro;

public class Player : Entity
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
            SetState(State.Normal);
            return 0;
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

    public void ApplyFear(int fearLevel)
    {
        attack -= fearLevel;
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
        print("Player is Feared, damages and speed reduced");
    }

    public void ClearState()
    {
        SetState(State.Normal);
        attack = _baseAttack;
        speed = _baseSpeed;
    }
}

