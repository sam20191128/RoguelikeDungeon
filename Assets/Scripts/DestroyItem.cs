using UnityEngine;

public class DestroyItem : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private CameraController cameraController;
    private BoxCollider2D boxCollider;

    private bool isDead;

    public GameObject slashEffect;

    private int randNum;//整形随机数值
    public GameObject[] dropItems;//掉落物品

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cameraController = FindObjectOfType<CameraController>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Weapon_Axe" && !isDead)//飞出的斧子碰撞到
        {
            isDead = true;
            boxCollider.enabled = false;//关闭碰撞体

            anim.SetTrigger("isDestroyed");//执行销毁动画

            cameraController.isShaked = true;//相机震动
            cameraController.CameraShake(0.2f);//震动量

            Instantiate(slashEffect, transform.position, Quaternion.identity);//实例化摧毁特效
        }

        if (other.gameObject.tag == "Weapon" && !isDead)//普通武器碰撞到
        {
            isDead = true;
            boxCollider.enabled = false;//关闭碰撞体

            anim.SetTrigger("isDestroyed");//执行销毁动画

            cameraController.isShaked = true;//相机震动
            cameraController.CameraShake(0.2f);//震动量

            Instantiate(slashEffect, transform.position, Quaternion.identity);//实例化摧毁特效
        }

        if (other.gameObject.tag == "Buff" && !isDead)
        {
            isDead = true;
            boxCollider.enabled = false;//关闭碰撞体

            anim.SetTrigger("isDestroyed");//执行销毁动画

            cameraController.isShaked = true;//相机震动
            cameraController.CameraShake(0.2f);//震动量

            Instantiate(slashEffect, transform.position, Quaternion.identity);//实例化摧毁特效
        }
    }

    //标记此方法将在“isDestroyed”动画的结束帧上触发
    public void DropRandomItem()//随机掉落物品
    {
        randNum = Random.Range(0, 2); //返回一个介于最小值和最大值之间的随机整数
        Instantiate(dropItems[randNum], transform.position, Quaternion.identity);//实例化掉落物品
    }
}
