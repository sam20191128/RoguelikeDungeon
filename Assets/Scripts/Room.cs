using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    public GameObject doorLeft, doorRight, doorTop, doorDown;

    public bool roomLeft, roomRight, roomUp, roomDown;

    public Text text;

    public int doorNumber;

    public int stepTostart;

    void Start()
    {
        doorLeft.SetActive(roomLeft);
        doorRight.SetActive(roomRight);
        doorTop.SetActive(roomUp);
        doorDown.SetActive(roomDown);
    }

    public void UpdateRoom(float x0ffset, float y0ffset)
    {
        stepTostart = (int)(Mathf.Abs(transform.position.x / x0ffset) + Mathf.Abs(transform.position.y / y0ffset));

        text.text = stepTostart.ToString();

        if (roomUp)
            doorNumber++;
        if (roomDown)
            doorNumber++;
        if (roomLeft)
            doorNumber++;
        if (roomRight)
            doorNumber++;
    }

    private void OnTriggerEnter2D(Collider2D  other)//碰撞房间
    {
        if (other.CompareTag("Player"))//如果碰撞体是角色
        {
            CameraController.instance.ChangeTarget(transform);//当前房间的坐标传进去
        }
    }


}
