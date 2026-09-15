using UnityEngine;

public class LoadArrow : MonoBehaviour
{
    public GameObject ArrowObject;
    public bool ArrowLoaded;

    [SerializeField] private PlayerSettings settings;

    private Arrow arrowScript;

    public void Load()
    {
        ArrowLoaded = true;
        arrowScript = ArrowObject.GetComponent<Arrow>();
        ArrowObject.transform.SetParent(null);
        ArrowObject.transform.position = transform.position;
        ArrowObject.transform.rotation = transform.rotation;
        ArrowObject.transform.SetParent(transform);
    }

    public void Shoot()
    {
        arrowScript.SwitchSettings();
        arrowScript.IsShot = true;
        ArrowObject.transform.parent = null;
    }
}
