using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomGenerator : MonoBehaviour
{
    public enum Direction { up, down, left, right };
    public Direction direction;


    [Header("房间信息")]
    public GameObject roomPrefab;
    public int roomNumber;
    //public Color startColor, endColor;
    private GameObject endRoom;

    [Header("位置控制")]
    public Transform generatorPoint;
    public float x0ffset;
    public float y0ffset;
    public LayerMask roomLayer;

    public int maxStep;//最大距离

    public List<Room> rooms = new List<Room>();//房间列表

    List<GameObject> farRooms = new List<GameObject>();//最远距离的房间
    List<GameObject> lessFarRooms = new List<GameObject>();//比最远稍微近一步的房间
    List<GameObject> oneWayRooms = new List<GameObject>();//只有一个入口的房间

    public WallType wallType;

    void Start()
    {
        for (int i = 0; i < roomNumber; i++)
        {
            rooms.Add(Instantiate(roomPrefab, generatorPoint.position, Quaternion.identity).GetComponent<Room>());

            //改变point位置
            ChangPointPos();
        }

        //rooms[0].GetComponent<SpriteRenderer>().color = startColor;

        endRoom = rooms[0].gameObject;

        //找到最后房间，变量room采集自rooms列表
        foreach (var room in rooms)
        {
            //if (room.transform.position.sqrMagnitude > endRoom.transform.position.sqrMagnitude)//如果距离最远。sqrMagnitude是变量值的x*x+y*y+z*z
            //{
            //    endRoom = room.gameObject;
            //}

            SetupRoom(room, room.transform.position);
        }

        FindEndRoom();

        //endRoom.GetComponent<SpriteRenderer>().color = endColor;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void ChangPointPos()//改变point位置
    {
        do
        {
            direction = (Direction)Random.Range(0, 4);//强制转换int整型数值为枚举数值

            switch (direction)//switch在前，先执行再检测
            {

                case Direction.up:
                    generatorPoint.position += new Vector3(0, y0ffset, 0);
                    break;
                case Direction.down:
                    generatorPoint.position += new Vector3(0, -y0ffset, 0);
                    break;
                case Direction.left:
                    generatorPoint.position += new Vector3(-x0ffset, 0, 0);
                    break;
                case Direction.right:
                    generatorPoint.position += new Vector3(x0ffset, 0, 0);
                    break;
            }
        } while (Physics2D.OverlapCircle(generatorPoint.position, 0.2f, roomLayer));//检测是否碰撞到已有的房间
    }

    public void SetupRoom(Room newRoom, Vector3 roomPosition)
    {
        newRoom.roomUp = Physics2D.OverlapCircle(roomPosition + new Vector3(0, y0ffset, 0), 0.2f, roomLayer);
        newRoom.roomDown = Physics2D.OverlapCircle(roomPosition + new Vector3(0, -y0ffset, 0), 0.2f, roomLayer);
        newRoom.roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-x0ffset, 0, 0), 0.2f, roomLayer);
        newRoom.roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(x0ffset, 0, 0), 0.2f, roomLayer);

        newRoom.UpdateRoom(x0ffset, y0ffset);

        switch (newRoom.doorNumber)
        {
            case 1:
                if (newRoom.roomUp)
                    Instantiate(wallType.singelUp, roomPosition, Quaternion.identity);
                if (newRoom.roomDown)
                    Instantiate(wallType.singelBottom, roomPosition, Quaternion.identity);
                if (newRoom.roomLeft)
                    Instantiate(wallType.singelLeft, roomPosition, Quaternion.identity);
                if (newRoom.roomRight)
                    Instantiate(wallType.singelRight, roomPosition, Quaternion.identity);
                break;
            case 2:
                if (newRoom.roomLeft && newRoom.roomUp)
                    Instantiate(wallType.doubleLU, roomPosition, Quaternion.identity);
                if (newRoom.roomLeft && newRoom.roomRight)
                    Instantiate(wallType.doubleLR, roomPosition, Quaternion.identity);
                if (newRoom.roomLeft && newRoom.roomDown)
                    Instantiate(wallType.doubleLB, roomPosition, Quaternion.identity);
                if (newRoom.roomUp && newRoom.roomRight)
                    Instantiate(wallType.doubleUR, roomPosition, Quaternion.identity);
                if (newRoom.roomUp && newRoom.roomDown)
                    Instantiate(wallType.doubleUB, roomPosition, Quaternion.identity);
                if (newRoom.roomRight && newRoom.roomDown)
                    Instantiate(wallType.doubleRB, roomPosition, Quaternion.identity);
                break;
            case 3:
                if (newRoom.roomLeft && newRoom.roomUp && newRoom.roomRight)
                    Instantiate(wallType.tripleLUR, roomPosition, Quaternion.identity);
                if (newRoom.roomLeft && newRoom.roomUp && newRoom.roomDown)
                    Instantiate(wallType.tripleLUB, roomPosition, Quaternion.identity);
                if (newRoom.roomLeft && newRoom.roomRight && newRoom.roomDown)
                    Instantiate(wallType.tripleLRB, roomPosition, Quaternion.identity);
                if (newRoom.roomUp && newRoom.roomRight && newRoom.roomDown)
                    Instantiate(wallType.tripleURB, roomPosition, Quaternion.identity);
                break;
            case 4:
                if (newRoom.roomLeft && newRoom.roomUp && newRoom.roomRight && newRoom.roomDown)
                    Instantiate(wallType.fourDoors, roomPosition, Quaternion.identity);
                break;
        }

    }


    public void FindEndRoom()//找到最终房间
    {
        //最大数值
        for (int i = 0; i < rooms.Count; i++)//for循环
        {
            if (rooms[i].stepTostart > maxStep)//如果最远距离大于maxStep
                maxStep = rooms[i].stepTostart; //maxStep等于最远距离
        }
        //获得最大值房间和次大值
        foreach (var room in rooms)
        {
            if (room.stepTostart == maxStep)//如果room的stepTostart等于最远距离
                farRooms.Add(room.gameObject);//那么添加room到最远的列表           
            if (room.stepTostart == maxStep - 1)//如果room的stepTostart等于最远距离-1
                lessFarRooms.Add(room.gameObject);//那么添加room到比最远稍近的列表
        }

        for (int i = 0; i < farRooms.Count; i++)
        {
            if (farRooms[i].GetComponent<Room>().doorNumber == 1)
                oneWayRooms.Add(farRooms[i]);
        }

        for (int i = 0; i < lessFarRooms.Count; i++)
        {
            if (lessFarRooms[i].GetComponent<Room>().doorNumber == 1)
                oneWayRooms.Add(lessFarRooms[i]);
        }

        if (oneWayRooms.Count != 0)
        {
            endRoom = oneWayRooms[Random.Range(0, oneWayRooms.Count)];
        }
        else
        {
            endRoom = farRooms[Random.Range(0, farRooms.Count)];
        }
    }
}


[System.Serializable]
public class WallType
{
    public GameObject singelLeft, singelRight, singelUp, singelBottom,
                      doubleLU, doubleLR, doubleLB, doubleUR, doubleUB, doubleRB,
                      tripleLUR, tripleLUB, tripleLRB, tripleURB,
                      fourDoors;
}
