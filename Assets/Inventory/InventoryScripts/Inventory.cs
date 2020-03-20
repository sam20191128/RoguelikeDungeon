using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New inventory",menuName = "Inventory/New Inventory")]
public class Inventory : ScriptableObject
{
    public List<Item> itemList = new List<Item>();
}
