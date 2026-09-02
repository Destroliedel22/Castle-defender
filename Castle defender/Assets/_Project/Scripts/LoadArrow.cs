using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class LoadArrow : MonoBehaviour
{
    private bool ArrowLoaded;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Arrow") && !ArrowLoaded)
        {
            XRGrabInteractable grabbable = other.GetComponentInParent<XRGrabInteractable>();
            if (grabbable != null && grabbable.isSelected)
            {
                ArrowLoaded = true;
                grabbable.interactionManager.SelectExit(grabbable.interactorsSelecting[0], grabbable);
                grabbable.GetComponent<Rigidbody>().useGravity = false;
                grabbable.GetComponent<Rigidbody>().isKinematic = true;
                grabbable.GetComponent<Collider>().isTrigger = true;
                grabbable.transform.position = transform.position;
                grabbable.transform.rotation = transform.rotation;
                grabbable.transform.parent = transform;
            }
        }
    }
}
