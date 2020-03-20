using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    Rigidbody2D rb;
    public BoxCollider2D boxCollider;
    public CircleCollider2D circleCollider;
    Animator anim;
    Vector2 movement;
    public float speed;
    public float speed2;

    //[Header("CD的UI组件")]
    //public Image cdImage;

    [Header("Dash参数")]//冲锋
    public float dashTime;//dash 时长
    private float dashTimeLeft;//冲锋剩余时间
    private float lastDash = -10f;//上一次dash时间点
    public float dashCoolDown;//Dash的CD时间
    public float dashSpeed;//冲锋速度

    private bool isDashing, Hurt;//布尔值变量,判断按键是否按下

    //public Joystick joystick;//操纵杆
    private float horizontalMove;

    public string scenePassword;

    private bool isDamaged;//可造成伤害

    public GameObject weapon_axe;
    public bool axeInHand;

    public GameObject myBag;
    public GameObject myEquip;
    bool isOpen_Bag;
    bool isOpen_Equip;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();
        weapon_axe = GameObject.FindGameObjectWithTag("Weapon_Axe");
        weapon_axe.SetActive(false);
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        //movement.x = joystick.Horizontal;
        //movement.y = joystick.Vertical;

        Weapon_Axe_Show();


        Attack();


        if (Input.GetMouseButtonDown(1))//冲锋
        {
            if (Time.time >= (lastDash + dashCoolDown))//当前时间超过了最后一次执行时间+CD冷却时间
            {
                ReadyToDash();//可以执行Dash
            }
        }

        OpenMyBag();

        OpenEquip();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

        Dash();//调用冲锋的方法

        Turnaround();//调用移动的方法

        SwitchAnim();//调用动画的方法
    }

    void Turnaround()//移动
    {
        //horizontalMove = joystick.Horizontal;//操纵杆控制移动

        if (movement.x != 0)
        {
            transform.localScale = new Vector3(movement.x, 1, 1);
        }
        if (movement.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        if (movement.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void SwitchAnim()//切换动画效果
    {
        anim.SetFloat("running", movement.magnitude);
    }
    void ReadyToDash()//准备冲锋
    {
        isDashing = true;//启用

        dashTimeLeft = dashTime;//冲锋剩余时间=设定的冲锋时间 

        lastDash = Time.time;//时间点=按下按键的时间点

        //cdImage.fillAmount = 1;//冲锋后CD开始冷却
    }

    public void Dash()//冲锋速度的方法
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)//冲锋剩余时间大于0
            {
                //rb.velocity = new Vector2(dashSpeed * horizontalMove, rb.velocity.y);//地面Dash
                rb.MovePosition(rb.position + dashSpeed * movement * speed * Time.fixedDeltaTime); ;//地面Dash

                dashTimeLeft -= Time.deltaTime;//跳出循环，因为要在FixedUpdate里调用，所以要减去这个时间的值Time.deltaTime

                ShadowPool.instance.GetFormPool();//拿出一个影子
            }
            if (dashTimeLeft <= 0)//冲锋剩余时间小于0
            {
                isDashing = false;//停止冲锋
            }
        }
    }
    private void Weapon_Axe_Show()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            axeInHand = !axeInHand;
        }
        if (axeInHand == true)
        {
            weapon_axe.SetActive(true);
        }
        else
        {
            weapon_axe.SetActive(false);
        }
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(2))
        {
            anim.SetBool("isAttack", true);
            isDamaged = true;
        }
        else if (Input.GetMouseButtonUp(2))
        {
            anim.SetBool("isAttack", false);
            isDamaged = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)//对敌人造成伤害
    {
        if (other.gameObject.tag == "Enemy" && isDamaged)
        {
            other.GetComponentInChildren<HealthBar>().hp -= 10;
            if (other.GetComponentInChildren<HealthBar>().hp <= 0)
            {
                //Destroy(other.gameObject);
                other.gameObject.SetActive(false);
            }
        }
    }

    void OpenMyBag()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            isOpen_Bag = myBag.activeSelf;
            isOpen_Bag = !isOpen_Bag;
            myBag.SetActive(isOpen_Bag);
        }
    }
    void OpenEquip()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isOpen_Equip = myEquip.activeSelf;
            isOpen_Equip = !isOpen_Equip;
            myEquip.SetActive(isOpen_Equip);
        }
    }
}
