using UnityEngine;

public class BuffPosition : MonoBehaviour
{
    private Transform PlayerPosition;

    private void Start()
    {
        transform.SetParent(null);
        PlayerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        transform.position = PlayerPosition.position;
    }
}
