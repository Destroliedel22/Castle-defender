using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DrawString : MonoBehaviour
{
    [HideInInspector] public float DrawDistance;

    [SerializeField] private LineRenderer trajectoryLine;

    [SerializeField] private PlayerSettings settings;
    [SerializeField] private Transform stringRestPoint;
    [SerializeField] private LoadArrow loadArrow;

    private Rigidbody rb;
    private Animator animator;

    private Transform grabbedHand;
    private Vector3 startPos;
    private Vector3 drawAxis;
    private float clampDistance;
    private float normalizedDraw;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInParent<Animator>();
        animator.speed = 0;
    }

    private void Update()
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

            //Where the hand is on the given axis
            DrawDistance = Vector3.Dot(handDirection, drawAxis);

            //Distance keeps between the min and max
            clampDistance = Mathf.Clamp(DrawDistance, settings.MinDrawDistance, settings.MaxDrawDistance);

            //Calculates and sets the position of the string on the given axis according to where the hand is.
            Vector3 pos = startPos + drawAxis * clampDistance;
            rb.MovePosition(pos);

            //Gets a value between 0 and 1 to play the animation
            normalizedDraw = Mathf.InverseLerp(settings.MinDrawDistance, settings.MaxDrawDistance, clampDistance);
            animator.Play("Wooden Bow", 0, normalizedDraw);

            if (loadArrow.ArrowObject)
            {
                //Trajectory
                Vector3 trajectoryStartPos = loadArrow.ArrowObject.transform.position;
                Vector3[] points = CalculateTrajectoryPoint(trajectoryStartPos, ArrowSpeed());
                trajectoryLine.positionCount = points.Length;
                trajectoryLine.SetPositions(points);
            }
        }
        else
        {
            rb.MovePosition(startPos);

            normalizedDraw -= 0.1f;
            animator.Play("Wooden Bow", 0, normalizedDraw);

            trajectoryLine.positionCount = 0;
        }

        rb.MoveRotation(stringRestPoint.rotation);
    }

    public void OnGrab(SelectEnterEventArgs args)
    {
        grabbedHand = args.interactorObject.transform;
    }

    public void OnLetGo(SelectExitEventArgs args)
    {
        grabbedHand = null;
        ShootArrow();
    }

    private void ShootArrow()
    {
        if (loadArrow.ArrowObject)
        {
            loadArrow.Shoot();
            Rigidbody rb = loadArrow.ArrowObject.GetComponent<Rigidbody>();

            rb.AddForce(ArrowSpeed(), ForceMode.VelocityChange);

            loadArrow.ArrowObject = null;
            loadArrow.ArrowLoaded = false;
        }
    }

    private Vector3 ArrowSpeed()
    {
        //Calculates how far the string is drawn for more force on the arrow
        float arrowSpeed = Mathf.Lerp(settings.MinArrowSpeed, settings.MaxArrowSpeed, clampDistance / settings.MaxDrawDistance);
        return drawAxis * -arrowSpeed;
    }

    private Vector3[] CalculateTrajectoryPoint(Vector3 startPos, Vector3 startVelocity)
    {
        int steps = 30;
        float duration = 2f;
        Vector3[] points = new Vector3[steps];

        for (int i = 0; i < steps; i++)
        {
            float t = (i / (float)(steps - 1)) * duration;

            Vector3 point = startPos + startVelocity * t + 0.5f * Physics.gravity * t * t;
            points[i] = point;
        }

        return points;
    }
}