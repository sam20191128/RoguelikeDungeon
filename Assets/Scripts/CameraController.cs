using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public float speed;

    public Transform target;//目标的Transform

    private float shakeAmplitude;//振幅
    private Vector3 shakeActive;

    public bool isShaked;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (target != null)
        {
            //MoveTowards(初始位置，目标位置，移动速度)
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), speed * Time.deltaTime);
        }
        if (shakeAmplitude>0)//振幅大于0
        {
            shakeActive=new Vector3(Random.Range(-shakeAmplitude, shakeAmplitude),Random.Range(-shakeAmplitude, shakeAmplitude),0);
            shakeAmplitude -= Time.deltaTime;
        }

        if (isShaked)
        {
            transform.position += shakeActive;
        }
    }

    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;//之前的相机目标变成新的相机目标
    }

    public void CameraShake(float _shakeAmount)
    {
        shakeAmplitude = _shakeAmount;
    }
}
