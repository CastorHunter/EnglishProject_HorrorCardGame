using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Player : Entity
{
    private List<State> _playerStates = new List<State>();
    private int _baseSpeed, _baseAttack, _baseHealth;
    public TextMeshProUGUI _playerStatesText;

    private void Start()
    {
        _baseAttack = attack;
        _baseSpeed = speed;
        _baseHealth = health;
        AddState(State.Normal);
    }
    public override int Attack()
    {
        if (_playerStates.Contains(State.Surprised))
        {
            RemoveState(State.Surprised);
            return 0;
        }
        return attack;
    }
    
    public override int TakeDamage(int currentHealth, int damage)
    {
        currentHealth -= damage;
        return currentHealth;
    }
    
    public int TakeHeal(int currentHealth, int heal)
    {
        currentHealth += heal;
        if (currentHealth > _baseHealth)
        {
            currentHealth = _baseHealth;
        }
        return currentHealth;
    }

    public void AddState(State newState)
    {
        if (_playerStates.Contains(newState) != true)
        {
            _playerStates.Add(newState);
            if (_playerStates.Contains(State.Normal) && newState != State.Normal)
            {
                RemoveState(State.Normal);
            }
            SetPlayerStatesText();
        }
    }

    public void RemoveState(State newState)
    {
        if (_playerStates.Contains(newState))
        {
            _playerStates.Remove(newState);
        }
        SetPlayerStatesText();
    }

    public List<State> GetStates()
    {
        return _playerStates;
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
        AddState(State.Feared);
        print("Player is Feared, damages and speed reduced");
    }

    public void ClearStates()
    {
        _playerStates.Clear();
        AddState(State.Normal);
        attack = _baseAttack;
        speed = _baseSpeed;
        SetPlayerStatesText();
    }

    private void SetPlayerStatesText()
    {
        _playerStatesText.text = ("State : ");
        foreach (var state in _playerStates)
        {
            _playerStatesText.text += state.ToString() + " ";
        }
    }
}

