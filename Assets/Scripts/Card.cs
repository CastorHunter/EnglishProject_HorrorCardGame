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
    
    public virtual void TakeDamage(int damage)
    {
        health -= damage;
    }
}
