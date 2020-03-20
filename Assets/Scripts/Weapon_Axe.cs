using System.Collections;
using UnityEngine;

public class Weapon_Axe : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;//斧子自身旋转速度
    [SerializeField] private bool isRotating;

    private BoxCollider2D boxCollider;

    //武器掷出
    [SerializeField] private float moveSpeed;//斧子掷出移动速度
    private Vector3 targetPos;
    private bool isClicked;//鼠标点击
    private bool isDamaged;//可造成伤害

    //武器召回
    private Transform WeaponPosition;
    private bool canCallBack;
    private bool returnWeapon;

    private CameraController cameraController;
    public GameObject slashEffect;
    public GameObject weaponReturnEffect;

    private TrailRenderer tr;//拖尾特效


    private void Start()
    {
        WeaponPosition = GameObject.FindGameObjectWithTag("WeaponPosition").GetComponent<Transform>();
        cameraController = FindObjectOfType<CameraController>();
        tr = GetComponentInChildren<TrailRenderer>();
        tr.enabled = false;//拖尾特效初始关闭
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;//关闭碰撞体
    }


    private void Update()
    {
        SelfRotation();
        if (Input.GetMouseButtonDown(0) && isClicked == false)//按下鼠标左键
        {
            isClicked = true;//点击
            //鼠标点击的坐标等于目标坐标
            targetPos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                                    Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
            boxCollider.enabled = true;
        }

        if (isClicked)//点击
        {
            ThrowWeapon();//掷出武器
        }

        ReachAtMousePosition();//武器投掷到目标点

        if (Input.GetMouseButtonDown(0) && canCallBack)//按下鼠标左键并且是可召回状态
        {
            isDamaged = true;//造成伤害开启
            returnWeapon = true;//召回武器开启
        }

        if (returnWeapon)//召回武器开启
        {
            BackWeapon();//执行召回武器
        }

        ReachAtPlayerPosition();//武器回到手中

        if (!isClicked && !returnWeapon && !canCallBack)
        {
            transform.position = WeaponPosition.position;
        }
    }

    private void ThrowWeapon()//掷出武器
    {
        isDamaged = true;//造成伤害开启
        isRotating = true;//旋转开启
        transform.SetParent(null);
        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);//移动到目标点
        tr.enabled = true;//拖尾特效开启
    }

    private void BackWeapon()//召回武器
    {
        transform.SetParent(WeaponPosition);
        isRotating = true;//旋转开启
        boxCollider.enabled = true;
        tr.enabled = true;//拖尾特效开启
        transform.position = Vector2.MoveTowards(transform.position, WeaponPosition.position, moveSpeed * 5 * Time.deltaTime);//移动到人物的点

        if (Vector2.Distance(transform.position, WeaponPosition.position) <= 0.01f)
        {
            StartCoroutine(CallBackEffect()); //开始协同程序
            Instantiate(weaponReturnEffect, transform.position, Quaternion.identity);//实例化武器返回特效
        }
    }

    private void ReachAtMousePosition()//武器投掷到目标点后
    {
        if (Vector2.Distance(transform.position, targetPos) <= 0.01f)
        {
            isRotating = false;//旋转关闭
            isDamaged = false;//造成伤害关闭
            tr.enabled = false;//拖尾特效关闭
            canCallBack = true;//可召回开启
            boxCollider.enabled = false;//关闭碰撞体
        }
    }

    private void ReachAtPlayerPosition()//武器回到手中后
    {
        if (Vector2.Distance(transform.position, WeaponPosition.position) <= 0.01f)
        {
            isRotating = false;//旋转关闭
            canCallBack = false;//可召回关闭
            isDamaged = false;//造成伤害关闭
            returnWeapon = false;//召回武器关闭
            tr.enabled = false;//拖尾特效关闭
            isClicked = false;//点击结束
            boxCollider.enabled = false;//关闭碰撞体

            transform.SetParent(WeaponPosition);
            transform.rotation = new Quaternion(0, 0, 0, 0);//位置摆正
        }
    }

    private void SelfRotation()//武器自身旋转
    {
        if (isRotating)
        {
            transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
        }
        else
        {
            transform.Rotate(0, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)//对敌人造成伤害
    {
        if (other.gameObject.tag == "Enemy" && isDamaged)
        {
            other.GetComponentInChildren<HealthBar>().hp -= 20;
            if (other.GetComponentInChildren<HealthBar>().hp <= 0)
            {
                //Destroy(other.gameObject);
                other.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator CallBackEffect()//协程
    {
        cameraController.isShaked = true;//相机震动
        cameraController.CameraShake(0.2f);//震动量
        yield return new WaitForSeconds(0.6f);//相机震动0.6秒
        cameraController.isShaked = false;//震动关闭
    }
}
