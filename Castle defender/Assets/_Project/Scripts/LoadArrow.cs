using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class LoadArrow : MonoBehaviour
{
    public GameObject arrowObject;
    public bool arrowLoaded;

    private bool moveArrow;

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

    private void Update()
    {
        if(moveArrow)
        {
            float speed = 1;
            Vector3 newPosition = Vector3.MoveTowards(arrowObject.transform.position, transform.position, speed * Time.deltaTime);
            arrowObject.transform.position = newPosition;
            Vector3 newRotation = Vector3.RotateTowards(arrowObject.transform.forward, transform.position - arrowObject.transform.position, speed * Time.deltaTime, 0.0f);
            arrowObject.transform.rotation = Quaternion.LookRotation(newRotation);

            if (Vector3.Distance(arrowObject.transform.position, transform.position) < 0.1f)
            {
                arrowObject.transform.position = transform.position;
                arrowObject.transform.rotation = transform.rotation;
                moveArrow = false;
            }
        }
    }

    private void Load()
    {
        //Put the next 3 inside a function inside arrow script and call it here
        arrowObject.GetComponent<Rigidbody>().useGravity = false;
        arrowObject.GetComponent<Rigidbody>().isKinematic = true;
        arrowObject.GetComponent<Collider>().isTrigger = true;

        moveArrow = true;

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
