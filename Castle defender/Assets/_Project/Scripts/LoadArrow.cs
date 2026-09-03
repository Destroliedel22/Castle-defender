using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class LoadArrow : MonoBehaviour
{
    public GameObject arrowObject;
    public bool arrowLoaded;

    [SerializeField] private BowSettings settings;

    private Arrow arrowScript;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Arrow") && !arrowLoaded)
        {
            XRGrabInteractable grabbable = other.GetComponentInParent<XRGrabInteractable>();
            if (grabbable != null && grabbable.isSelected)
            {
                arrowLoaded = true;
                arrowObject = grabbable.gameObject;

                //Forces the hand to let go of the arrow
                grabbable.interactionManager.SelectExit(grabbable.interactorsSelecting[0], grabbable);

                Load();
            }
        }
    }

    private void Load()
    {
        arrowScript = arrowObject.GetComponent<Arrow>();
        arrowScript.SwitchSettings();
        arrowObject.transform.position = transform.position;
        arrowObject.transform.rotation = transform.rotation;
        arrowObject.transform.parent = transform;
    }

    public void Shoot()
    {
        arrowScript.SwitchSettings();
        arrowObject.transform.parent = null;

    }
}
