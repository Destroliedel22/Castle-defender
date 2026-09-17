using UnityEngine;

public class LoadArrow : MonoBehaviour
{
    public GameObject ArrowObject;
    public bool ArrowLoaded;

    public Arrow ArrowScript;

    [SerializeField] private PlayerSettings settings;

    public void Load()
    {
        ArrowLoaded = true;
        ArrowScript = ArrowObject.GetComponent<Arrow>();
        ArrowObject.transform.SetParent(null);
        ArrowObject.transform.position = transform.position;
        ArrowObject.transform.rotation = transform.rotation;
        ArrowObject.transform.SetParent(transform);
    }

    public void Shoot()
    {
        ArrowScript.SwitchSettings();
        ArrowScript.IsShot = true;
        ArrowObject.transform.parent = null;
    }
}
