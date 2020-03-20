using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item slotItem;//获得物品
    public Image slotImage;
    public Text slotNum;//物品数量



    public void ItemOnClicked()//点击物品
    {
        InventoryManager.UpdateItemInfo(slotItem.itemInfo);//显示物品描述
    }

    public void SetupSlot(Item item)
    {

    }

}
