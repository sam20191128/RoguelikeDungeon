using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target;
    [SerializeField] private float moveSpeed;

    private float lifeTimer;
    [SerializeField] private float maxLife = 2.0f;

    public GameObject destroyEffect;
    public GameObject attackEffect;


    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        lifeTimer += Time.deltaTime;
        if (lifeTimer >= maxLife)//2秒后销毁
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponentInChildren <HealthBar>().hp -= 25;//GetComponentInChildren在物体的子级里找到HealthBar

            Instantiate(attackEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }






}
