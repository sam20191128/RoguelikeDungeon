using UnityEngine;

public class Weapon_Sword : MonoBehaviour
{
    private Animator anim;
    private CameraController cameraController;

    private bool isDamaged;//可造成伤害

    private void Start()
    {
        anim = GetComponent<Animator>();
        cameraController = FindObjectOfType<CameraController>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            anim.SetBool("isAttack", true);
            isDamaged = true;
        }
        else if (Input.GetMouseButtonUp(1))
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
}
