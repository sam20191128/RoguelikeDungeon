using UnityEngine;

public class Buff : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private bool isRotating;
    [SerializeField] private bool isMove;
    [SerializeField] private float moveSpeed;
    private Vector3 targetPos;
    private Transform PlayerPosition;
    private bool isonPlayer;
    private bool isDamaged;//可造成伤害

    private void Start()
    {
        PlayerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        targetPos = new Vector3(PlayerPosition.position.x - 5f, PlayerPosition.position.y + 0.6f, PlayerPosition.position.z);

        BuffMove();
    }

    private void BuffMove()
    {
        if (isonPlayer == true)
        {

            if (Vector2.Distance(transform.position, PlayerPosition.position) < 2f)
            {
                isMove = true;
                transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            }
            else
            {
                isMove = false;
                isRotating = true;
                BuffRotation();
            }
        }
    }

    private void BuffRotation()//旋转
    {
        if (isRotating)
        {
            transform.RotateAround(new Vector3(PlayerPosition.position.x, PlayerPosition.position.y, PlayerPosition.position.z),
                                   new Vector3(0, 0, 1), rotateSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isonPlayer = true;
            isDamaged = true;
            transform.SetParent(PlayerPosition);
            transform.position = PlayerPosition.position;
        }

        if (other.gameObject.tag == "Enemy" && isonPlayer && isDamaged)
        {
            other.GetComponentInChildren<HealthBar>().hp -= 5;
            if (other.GetComponentInChildren<HealthBar>().hp <= 0)
            {
                //Destroy(other.gameObject);
                other.gameObject.SetActive(false);
            }
        }
    }


}
