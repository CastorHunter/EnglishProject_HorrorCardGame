using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Data/CardData")]
public class CardData : ScriptableObject
{
    public int health, attack, speed;
    public List<string> weaknessesList, strenghList;
    public Card cardOrigin;
}
