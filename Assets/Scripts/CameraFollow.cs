using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public RoleSwitcher switcher;
    public float smooth = 8f;
    public Vector3 offset = new Vector3(0, 0, -10);

    void LateUpdate()
    {
        if (!switcher) switcher = FindObjectOfType<RoleSwitcher>();
        var t = switcher ? switcher.GetActiveTarget() : null;
        if (!t) return;

        Vector3 target = t.position + offset;
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * smooth);
    }
}
