using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMap : MonoBehaviour
{
    GameObject mapSprite;

    private void OnEnable()//优先于start启动
    {
        mapSprite = transform.parent.GetChild(0).gameObject;//获得一开始边框的图片

        mapSprite.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            mapSprite.SetActive(true);
        }
    }
}
