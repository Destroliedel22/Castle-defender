using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DrawString : MonoBehaviour
{
    [HideInInspector] public float drawDistance;

    [SerializeField] private BowSettings settings;
    [SerializeField] private Transform stringRestPoint;
    [SerializeField] private LoadArrow loadArrow;

    private Rigidbody rb;
    private Animator animator;

    private Transform grabbedHand;
    private Vector3 startPos;
    private Vector3 drawAxis;
    private float clampDistance;
    private float normalizedDraw;
    private float grabOffset;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInParent<Animator>();
        animator.speed = 0;
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
            clampDistance = Mathf.Clamp(drawDistance, settings.minDrawDistance, settings.maxDrawDistance);
            Vector3 pos = startPos + drawAxis * clampDistance;
            rb.MovePosition(pos);

            //Gets a value between 0 and 1 to play the animation
            normalizedDraw = Mathf.InverseLerp(settings.minDrawDistance, settings.maxDrawDistance, clampDistance);
            animator.Play("Wooden Bow", 0, normalizedDraw);
        }
        else
        {
            rb.MovePosition(startPos);
            normalizedDraw -= 0.1f;
            animator.Play("Wooden Bow", 0, normalizedDraw);
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
        if(loadArrow.arrowObject)
        {
            loadArrow.Shoot();
            Rigidbody rb = loadArrow.arrowObject.GetComponent<Rigidbody>();
            float releaseSpeed = Mathf.Lerp(settings.minArrowSpeed, settings.maxArrowSpeed, clampDistance / settings.maxDrawDistance);
            rb.AddForce(drawAxis * -releaseSpeed, ForceMode.VelocityChange);
            loadArrow.arrowObject = null;
            loadArrow.arrowLoaded = false;
        }
    }
}