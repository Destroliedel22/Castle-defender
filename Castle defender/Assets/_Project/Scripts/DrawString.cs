using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DrawString : MonoBehaviour
{
    [HideInInspector] public float drawDistance;

    [SerializeField] private float minDrawDistance;
    [SerializeField] private float maxDrawDistance;
    [SerializeField] private float minArrowSpeed;
    [SerializeField] private float maxArrowSpeed;
    [SerializeField] private Transform stringRestPoint;
    [SerializeField] private LoadArrow loadArrow;

    private Rigidbody rb;
    private Transform grabbedHand;
    private Vector3 startPos;
    private Vector3 drawAxis;
    private float clampDistance;
    private float grabOffset;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        StringMovement();
    }

    private void StringMovement()
    {
        startPos = stringRestPoint.position;
        drawAxis = stringRestPoint.up;

        if (grabbedHand)
        {
            Vector3 handDirection = grabbedHand.position - startPos;
            drawDistance = Vector3.Dot(handDirection, drawAxis) - grabOffset;
            clampDistance = Mathf.Clamp(drawDistance, minDrawDistance, maxDrawDistance);
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
        ShootArrow();
    }

    private void ShootArrow()
    {
        loadArrow.Shoot();
        Rigidbody rb = loadArrow.arrowObject.GetComponent<Rigidbody>();
        float releaseSpeed = Mathf.Lerp(minArrowSpeed, maxArrowSpeed, clampDistance / maxDrawDistance);
        rb.AddForce(drawAxis * -releaseSpeed, ForceMode.VelocityChange);
        loadArrow.arrowObject = null;
        loadArrow.arrowLoaded = false;
    }
}