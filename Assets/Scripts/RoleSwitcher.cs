using UnityEngine;

public class RoleSwitcher : MonoBehaviour
{
    public GameObject fighter;
    public GameObject climber;

    GameObject active;
    void Start()
    {
        SetActive(fighter, true);
        SetActive(climber, false);
        active = fighter;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Vector3 pos = active.transform.position;
            if (active == fighter)
            {
                SetActive(fighter, false);
                SetActive(climber, true);
                climber.transform.position = pos;
                active = climber;
            }
            else
            {
                SetActive(climber, false);
                SetActive(fighter, true);
                fighter.transform.position = pos;
                active = fighter;
            }
        }
    }

    void SetActive(GameObject go, bool on)
    {
        go.SetActive(on);
    }

    public Transform GetActiveTarget()
    {
        return active != null ? active.transform : null;
    }
}
