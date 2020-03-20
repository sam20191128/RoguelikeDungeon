using UnityEngine;

public class Enemy_LongRangeAttack : MonoBehaviour
{
    public Transform wayPoint01, wayPoint02;
    private Transform wayPointTarget;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float attackRange;
    private bool Faceright = true;
    private Transform target;//目标的Transform

    private SpriteRenderer sp;
    private Material defaultMat;
    [SerializeField] private Material hurtMat;

    public bool isDead, isAttack;
    [HideInInspector]
    public float dazuiguaiPositionX, dazuiguaiPositionY;

    private CameraController cameraController;
    public GameObject slashEffect;

    private Animator anim;
    public GameObject Projectile;
    public Transform firePoint;



    private void Start()
    {
        anim = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        wayPointTarget = wayPoint01;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        defaultMat = GetComponent<SpriteRenderer>().material;
        cameraController = FindObjectOfType<CameraController>();
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, target.transform.position) >= attackRange && !isDead)
        {
            anim.SetBool("isAttack", false);
            Patrol();
        }
        else
        {
            anim.SetBool("isAttack", true);
        }

    }


    private void Patrol()//巡逻
    {
        transform.position = Vector2.MoveTowards(transform.position, wayPointTarget.position, moveSpeed * Time.deltaTime);


        if (Vector2.Distance(transform.position, wayPoint01.position) <= 0.01f)
        {
            wayPointTarget = wayPoint02;
            Vector3 localTemp = transform.localScale;
            localTemp.x *= -1;
            transform.localScale = localTemp;
        }

        if (Vector2.Distance(transform.position, wayPoint02.position) <= 0.01f)
        {
            wayPointTarget = wayPoint01;
            Vector3 localTemp = transform.localScale;
            localTemp.x *= -1;
            transform.localScale = localTemp;
        }
    }

    public void Shot()
    {
        Instantiate(Projectile, firePoint.position, Quaternion.identity);
    }


    void SwitchAnim()//切换动画效果
    {
        anim.SetFloat("running", Mathf.Abs(transform.position.magnitude));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Weapon_Axe")
        {
            cameraController.isShaked = true;//相机震动
            cameraController.CameraShake(0.2f);//震动量0.5

            Instantiate(slashEffect, transform.position, Quaternion.identity);
        }

        if (other.gameObject.tag == "Weapon" && !isDead)//普通武器碰撞到
        {
            cameraController.isShaked = true;//相机震动
            cameraController.CameraShake(0.2f);//震动量0.5

            Instantiate(slashEffect, transform.position, Quaternion.identity);
        }

        if (other.gameObject.tag == "Buff" && !isDead)
        {
            cameraController.isShaked = true;//相机震动
            cameraController.CameraShake(0.2f);//震动量0.5

            Instantiate(slashEffect, transform.position, Quaternion.identity);
        }
    }
}
