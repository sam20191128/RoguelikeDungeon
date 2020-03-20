using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    public int itemHeld;//整形变量，持有数量
    [TextArea]
    public string itemInfo;//物品属性描述

    public bool equip;
}
