using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class LoadArrow : MonoBehaviour
{
    public GameObject arrowObject;
    public bool arrowLoaded;

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
        //Put the next 3 inside a function inside arrow script and call it here
        arrowObject.GetComponent<Rigidbody>().useGravity = false;
        arrowObject.GetComponent<Rigidbody>().isKinematic = true;
        arrowObject.GetComponent<Collider>().isTrigger = true;

        //Change to moving towards instead of teleport
        arrowObject.transform.position = transform.position;
        arrowObject.transform.rotation = transform.rotation;
        arrowObject.transform.parent = transform;
    }

    public void Shoot()
    {
        //Put the next 3 inside a function inside arrow script and call it here
        arrowObject.GetComponent<Rigidbody>().useGravity = true;
        arrowObject.GetComponent<Rigidbody>().isKinematic = false;
        arrowObject.GetComponent<Collider>().isTrigger = false;

        arrowObject.transform.parent = null;
    }
}
