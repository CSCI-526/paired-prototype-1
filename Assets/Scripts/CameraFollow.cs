using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smooth = 8f;
    public Vector3 offset = new Vector3(0,0,-10);

    void LateUpdate()
    {
        if (!target)
        {
            var go = GameObject.FindWithTag("Player");
            if (go) target = go.transform; else return;
        }
        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * smooth);
    }
}