using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CardData", menuName = "Data/CardData")]
public class EntityData : ScriptableObject
{
    public int health, attack, speed;
    public List<string> weaknessesList, strenghList;
    [FormerlySerializedAs("cardOrigin")] public Entity entityOrigin;
}
