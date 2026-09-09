using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class LoadArrow : MonoBehaviour
{
    public GameObject ArrowObject;
    public bool ArrowLoaded;

    [SerializeField] private PlayerSettings settings;

    private Arrow arrowScript;

    private void OnTriggerEnter(Collider other)
    {
        if (!ArrowLoaded && other.CompareTag("Arrow"))
        {
            XRGrabInteractable grabbable = other.GetComponentInParent<XRGrabInteractable>();
            if (grabbable != null && grabbable.isSelected)
            {
                ArrowLoaded = true;
                ArrowObject = grabbable.gameObject;

                //Forces the hand to let go of the arrow
                grabbable.interactionManager.SelectExit(grabbable.interactorsSelecting[0], grabbable);

                Load();
            }
        }
    }

    private void Load()
    {
        arrowScript = ArrowObject.GetComponent<Arrow>();
        arrowScript.SwitchSettings();
        ArrowObject.transform.position = transform.position;
        ArrowObject.transform.rotation = transform.rotation;
        ArrowObject.transform.parent = transform;
    }

    public void Shoot()
    {
        arrowScript.SwitchSettings();
        arrowScript.IsShot = true;
        ArrowObject.transform.parent = null;
    }
}
