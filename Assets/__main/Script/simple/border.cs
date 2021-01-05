using UnityEngine;

public class border : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "border");
    }
}
