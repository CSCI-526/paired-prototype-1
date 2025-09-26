using UnityEngine;

public class PlayerLightFollow : MonoBehaviour
{
    public RoleSwitcher switcher;    
    public Vector3 offset = new Vector3(0, 0, 0);

    void LateUpdate()
    {
        if (!switcher) switcher = FindObjectOfType<RoleSwitcher>();
        var t = switcher ? switcher.GetActiveTarget() : null;
        if (!t) return;
        transform.position = t.position + offset;
    }
}
