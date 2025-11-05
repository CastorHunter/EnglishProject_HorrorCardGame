using System.Collections.Generic;
using UnityEngine;
using System;

public class Card : MonoBehaviour
{
    public string cardName;
    public int health, attack, speed;
    public List<string> weaknessesList, strengthList;
    public Sprite cardImage;
    public GameManagerBehavior gameManagerBehavior;
    
    public virtual int Attack()
    {
        return attack;
    }
    
    public virtual int TakeDamage(int targetHealth, int damage)
    {
        targetHealth -= damage;
        return targetHealth;
    }
}
