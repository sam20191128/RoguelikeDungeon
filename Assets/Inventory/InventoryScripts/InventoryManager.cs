using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    static InventoryManager instance;//单例模式

    public Inventory mybag;
    public GameObject slotGrid;//网格
    //public Slot slotPrefab;//网格
    public GameObject emptySlot;
    public Text itemInformation;

    public List<GameObject> slots = new List<GameObject>();

    private void Awake()
    {
        if (instance != null)

            Destroy(this);
        instance = this;
    }
    private void OnEnable()
    {
        RefreshItem();
        instance.itemInformation.text = "";//默认文本不显示
    }
    public static void UpdateItemInfo(string itemDescription)
    {
        instance.itemInformation.text = itemDescription;//itemDescription等于物品描述
    }


    //public static void CreatNewItem(Item item)//生成物品
    //{
    //    //item传到Slot，再传到InventoryManager来生成
    //    Slot newItem = Instantiate(instance.slotPrefab, instance.slotGrid.transform.position, Quaternion.identity);//在Grid网格里生成prefab
    //    newItem.gameObject.transform.SetParent(instance.slotGrid.transform);//挂在Grid的子集里
    //    newItem.slotItem = item;
    //    newItem.slotImage.sprite = item.itemImage;
    //    newItem.slotNum.text = item.itemHeld.ToString();
    //}

    public static void RefreshItem()//销毁
    {
        for (int i = 0; i < instance.slotGrid.transform.childCount; i++)//instance.slotGrid.transform.childCount有多少个子集
        {
            if (instance.slotGrid.transform.childCount == 0)//如果有0个子集
                break;//跳过方法不执行
            Destroy(instance.slotGrid.transform.GetChild(i).gameObject);//否则有几个子集销毁几个子集
        }

        for (int i = 0; i < instance.mybag.itemList.Count; i++)//背包列表里有有几个物品
        {
            //CreatNewItem(instance.mybag.itemList[i]);//重新创建
            instance.slots.Add(Instantiate(instance.emptySlot));//生成同时添加进slots的list列表里
            instance.slots[i].transform.SetParent(instance.slotGrid.transform);//生成格
        }
    }
}
