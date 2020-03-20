using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemOnWorld : MonoBehaviour
{
    public Item thisItem;//告诉当前的物品是哪个数据库
    public Inventory playerInventory;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))//CompareTag,匹配标签
        {
            AddNewItem();//执行添加物品
            Destroy(gameObject);//销毁地图场景里的物品
        }
    }

    public void AddNewItem()//添加物品
    {
        if (!playerInventory.itemList.Contains(thisItem))//如果背包列表里没有这个物品
        {
            playerInventory.itemList.Add(thisItem);//则把这个物品添加进背包列表
            //InventoryManager.CreatNewItem(thisItem);//背包里生成物品
        }
        else//如果背包列表里有这个物品
        {
            thisItem.itemHeld += 1;//则物品数量+1
        }

        InventoryManager.RefreshItem();
    }
}
