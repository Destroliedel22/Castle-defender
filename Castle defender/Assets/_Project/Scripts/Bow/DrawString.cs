using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DrawString : MonoBehaviour
{
    public PlayerSettings Settings;

    [HideInInspector] public float DrawDistance;

    [HideInInspector] public bool UseMinigun;
    [HideInInspector] public float MiniGunTimeBetween;
    [HideInInspector] public bool CanPenetrate;
    [HideInInspector] public bool UseTrajectory;
    [HideInInspector] public bool UseMultiShot;
    [HideInInspector] public int MultiShotAngle;
    [HideInInspector] public bool IsHoming;

    [SerializeField] private LineRenderer trajectoryLine;
    [SerializeField] private Transform stringRestPoint;
    [SerializeField] private LoadArrow loadArrow;

    private Rigidbody rb;
    private Animator animator;

    private Transform grabbedHand;
    private Vector3 startPos;
    private Vector3 drawAxis;
    private float clampDistance;
    private float normalizedDraw;

    private float miniGunTimeBetween;

    private GameObject lowerArrow;
    private GameObject upperArrow;

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
            clampDistance = Mathf.Clamp(DrawDistance, Settings.MinDrawDistance, Settings.MaxDrawDistance);

            //Calculates and sets the position of the string on the given axis according to where the hand is.
            Vector3 pos = startPos + drawAxis * clampDistance;
            transform.position = pos;

            //Gets a value between 0 and 1 to play the animation
            normalizedDraw = Mathf.InverseLerp(Settings.MinDrawDistance, Settings.MaxDrawDistance, clampDistance);
            animator.Play("Wooden Bow", 0, normalizedDraw);

            if (clampDistance == Settings.MaxDrawDistance && UseMinigun)
                Minigun();

            if (loadArrow.ArrowObject && UseTrajectory)
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
            transform.position = startPos;

            normalizedDraw -= 0.1f;
            animator.Play("Wooden Bow", 0, normalizedDraw);

            if (UseTrajectory)
                trajectoryLine.positionCount = 0;
        }

        rb.MoveRotation(stringRestPoint.rotation);
    }

    public void OnGrab(SelectEnterEventArgs args)
    {
        grabbedHand = args.interactorObject.transform;
        loadArrow.ArrowObject = grabbedHand.parent.GetComponentInChildren<Arrow>().gameObject;
        loadArrow.Load();
        loadArrow.ArrowScript.Bow = this.gameObject;

        loadArrow.ArrowScript.CanPenetrate = CanPenetrate;
        loadArrow.ArrowScript.IsHoming = IsHoming;
        if (UseMultiShot)
        {
            lowerArrow = Instantiate(loadArrow.ArrowObject, loadArrow.ArrowObject.transform.position, loadArrow.ArrowObject.transform.rotation * Quaternion.Euler(MultiShotAngle, 0, 0));
            lowerArrow.transform.SetParent(loadArrow.transform);
            lowerArrow.transform.localScale = loadArrow.ArrowObject.transform.localScale;

            upperArrow = Instantiate(loadArrow.ArrowObject, loadArrow.ArrowObject.transform.position, loadArrow.ArrowObject.transform.rotation * Quaternion.Euler(-MultiShotAngle, 0, 0));
            upperArrow.transform.SetParent(loadArrow.transform);
            upperArrow.transform.localScale = loadArrow.ArrowObject.transform.localScale;
        }
    }

    public void OnLetGo(SelectExitEventArgs args)
    {
        grabbedHand.parent.GetComponentInChildren<SpawnArrow>().Spawn();
        grabbedHand = null;
        ShootArrow();
        if (UseMultiShot)
            MultiShot();
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
        float arrowSpeed = Mathf.Lerp(Settings.MinArrowSpeed, Settings.MaxArrowSpeed, clampDistance / Settings.MaxDrawDistance);
        return drawAxis * -arrowSpeed;
    }

    private void Minigun()
    {
        if (miniGunTimeBetween > 0)
            miniGunTimeBetween -= Time.deltaTime;
        else
        {
            miniGunTimeBetween = MiniGunTimeBetween;

            GameObject arrow = loadArrow.ArrowObject;
            GameObject clone = Instantiate(arrow, arrow.transform.position, arrow.transform.rotation);

            Arrow arrowScript = clone.GetComponent<Arrow>();
            arrowScript.SwitchSettings();
            arrowScript.Bow = this.gameObject;
            arrowScript.IsShot = true;

            Rigidbody rb = clone.GetComponent<Rigidbody>();
            rb.AddForce(SpreadDirection() * -Settings.MaxArrowSpeed, ForceMode.VelocityChange);
        }
    }

    private Vector3 SpreadDirection()
    {
        float randomX = Random.Range(-7, 7);
        float randomY = Random.Range(-7, 7);

        return Quaternion.Euler(randomX, randomY, 0) * drawAxis;
    }

    private void MultiShot()
    {
        Arrow lowerArrowScript = lowerArrow.GetComponent<Arrow>();
        lowerArrowScript.SwitchSettings();
        lowerArrowScript.Bow = this.gameObject;
        lowerArrowScript.IsShot = true;
        lowerArrow.transform.parent = null;

        Rigidbody lrb = lowerArrow.GetComponent<Rigidbody>();
        lrb.AddForce(AngledArrowSpeed(MultiShotAngle), ForceMode.VelocityChange);

        Arrow upperArrowScript = upperArrow.GetComponent<Arrow>();
        upperArrowScript.SwitchSettings();
        upperArrowScript.Bow = this.gameObject;
        upperArrowScript.IsShot = true;
        upperArrow.transform.parent = null;

        Rigidbody urb = upperArrow.GetComponent<Rigidbody>();
        urb.AddForce(AngledArrowSpeed(-MultiShotAngle), ForceMode.VelocityChange);
    }

    private Vector3 AngledArrowSpeed(float angle)
    {
        float arrowSpeed = Mathf.Lerp(Settings.MinArrowSpeed, Settings.MaxArrowSpeed, clampDistance / Settings.MaxDrawDistance);
        Vector3 rotatedDirection = Quaternion.AngleAxis(angle, stringRestPoint.right) * drawAxis;
        return rotatedDirection * -arrowSpeed;
    }

    private Vector3[] CalculateTrajectoryPoint(Vector3 startPos, Vector3 startVelocity)
    {
        int steps = 250;
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