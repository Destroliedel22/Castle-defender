using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class String : MonoBehaviour
{
    [SerializeField] private float minDrawDistance;
    [SerializeField] private float maxDrawDistance;
    [SerializeField] private Transform stringRestPoint;

    private Rigidbody rb;
    private Transform grabbedHand;
    private Vector3 startPos;
    private Vector3 drawAxis;
    private float drawDistance;
    private float grabOffset;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        startPos = stringRestPoint.position;
        drawAxis = stringRestPoint.up;

        if (grabbedHand)
        {
            Vector3 handDirection = grabbedHand.position - startPos;
            drawDistance = Vector3.Dot(handDirection, drawAxis) - grabOffset;
            float clampDistance = Mathf.Clamp(drawDistance, minDrawDistance, maxDrawDistance);
            Vector3 pos = startPos + drawAxis * clampDistance;
            rb.MovePosition(pos);
        }
        else
        {
            rb.MovePosition(startPos);
        }

        rb.MoveRotation(stringRestPoint.rotation);
    }

    public void OnGrab(SelectEnterEventArgs args)
    {
        grabbedHand = args.interactorObject.transform;

        Vector3 initialHandDirection = grabbedHand.position - startPos;
        grabOffset = Vector3.Dot(initialHandDirection, drawAxis);
    }

    public void OnLetGo(SelectExitEventArgs args)
    {
        grabbedHand = null;
    }
}