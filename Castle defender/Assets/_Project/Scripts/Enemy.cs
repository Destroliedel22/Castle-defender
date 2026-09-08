using System.Collections;
using UnityEngine;

public enum EnemyState
{
    Walking,
    Climbing,
    Attacking
}

public class Enemy : MonoBehaviour
{
    public event System.Action<Enemy> OnDeath;

    [HideInInspector] public Transform Target;

    [SerializeField] protected float walkSpeed = 1;
    [SerializeField] protected float climbTime = 5;
    [SerializeField] protected float minAttackTimer = 1;
    [SerializeField] protected float maxAttackTimer = 5;

    [SerializeField] private float walkToClimbDelay = 1;
    [SerializeField] private float climbToWalkDelay = 2;

    protected Animator animator;
    protected float attackTimer;

    private EnemyState enemyState;
    private const string WALKING_STATE = "Walking";
    private const string CLIMBING_STATE = "Climbing";
    private const string ATTACKING_STATE = "Attack";

    private Rigidbody[] rigidbodies;

    private bool hasClimbed;
    private bool isClimbing;

    private Coroutine climbRoutine;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
    }

    private void Start()
    {
        attackTimer = Random.Range(minAttackTimer, maxAttackTimer);
    }

    private void Update()
    {
        switch (enemyState)
        {
            case EnemyState.Walking:
                Walk();
                if (Vector3.Distance(transform.position, Target.position) < 1f)
                {
                    if (!hasClimbed)
                    {
                        animator.SetBool(WALKING_STATE, false);
                        if (walkToClimbDelay > 0f)
                            walkToClimbDelay -= Time.deltaTime;
                        else
                            enemyState = EnemyState.Climbing;
                    }
                    else
                        enemyState = EnemyState.Attacking;
                }
                break;

            case EnemyState.Climbing:
                if (!isClimbing && !hasClimbed)
                    climbRoutine = StartCoroutine(EnemyClimb());

                if (Vector3.Distance(transform.position, Target.position) < 1f)
                {
                    if (isClimbing)
                    {
                        animator.SetBool(CLIMBING_STATE, false);
                        isClimbing = false;
                        hasClimbed = true;

                        foreach (Rigidbody rb in rigidbodies)
                            rb.useGravity = true;
                    }

                    if (climbToWalkDelay > 0f)
                        climbToWalkDelay -= Time.deltaTime;
                    else
                    {
                        Target = Camera.main.transform.root;
                        enemyState = EnemyState.Walking;
                    }
                }
                break;

            case EnemyState.Attacking:
                animator.SetBool(WALKING_STATE, false);
                Attack();
                break;

        }
    }

    private void Walk()
    {
        animator.SetBool(WALKING_STATE, true);
        Vector3 pos = Vector3.MoveTowards(transform.position, Target.transform.position, walkSpeed * Time.deltaTime);
        if (!hasClimbed)
            pos.y = Terrain.activeTerrain.SampleHeight(pos);
        transform.position = pos;
        transform.LookAt(Target);
    }

    private void Attack()
    {
        if (attackTimer > 0f)
            attackTimer -= Time.deltaTime;
        else
        {
            attackTimer = Random.Range(minAttackTimer, maxAttackTimer);
            animator.SetTrigger(ATTACKING_STATE);
        }
    }

    IEnumerator EnemyClimb()
    {
        isClimbing = true;

        foreach (Rigidbody rb in rigidbodies)
            rb.useGravity = false;

        float rotationOffset = 20;

        transform.rotation = Quaternion.Euler(transform.rotation.x + rotationOffset, transform.rotation.y, transform.rotation.z);

        animator.SetBool(WALKING_STATE, false);
        animator.SetBool(CLIMBING_STATE, true);

        Transform NewTarget = Target.GetChild(0);
        Target = NewTarget;

        Vector3 startPos = transform.position;
        Vector3 endPos = NewTarget.position;
        float time = 0;

        while (time < climbTime)
        {
            time += Time.deltaTime;
            float progress = time / climbTime;

            transform.position = Vector3.Lerp(startPos, endPos, progress);

            yield return null;
        }

        transform.position = endPos;

        transform.rotation = Quaternion.Euler(transform.rotation.x - rotationOffset, transform.rotation.y, transform.rotation.z);
    }

    public void Death()
    {
        if (climbRoutine != null)
            StopCoroutine(climbRoutine);

        OnDeath?.Invoke(this);
        foreach (Rigidbody rb in rigidbodies)
            rb.useGravity = true;
        animator.enabled = false;
        Destroy(this.gameObject, 2);
    }
}
