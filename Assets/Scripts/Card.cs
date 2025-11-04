using System.Collections.Generic;
using UnityEngine;
using System;

public class Card : MonoBehaviour
{
    public int health, attack, speed;
    public List<string> weaknessesList, strenghList;
    public virtual int Attack()
    {
        return attack;
    }
    
    public virtual int TakeDamage(int targetHealth, int damage)
    {
        targetHealth -= damage;
        return health;
    }
}
