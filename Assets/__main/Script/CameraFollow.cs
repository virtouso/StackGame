using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public static CameraFollow _instance;

    public Transform goal;

    public float Offset;


    private void Awake()
    {
        _instance = this;
    }



    private void LateUpdate()
    {
        Follow();
    }



    public void Follow()
    {
        if (goal == null)
        {
            return;
        }

        transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, goal.position.y + Offset, 0.01f), transform.position.z);
    }


}
