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
    [HideInInspector] public int waveSpawned;
    [HideInInspector] public bool isDead;

    [SerializeField] private int dmg;

    [SerializeField] private float walkSpeed = 1;
    [SerializeField] private float climbTime = 5;
    [SerializeField] private float minAttackTimer = 1;
    [SerializeField] private float maxAttackTimer = 5;

    [SerializeField] private float walkToClimbDelay = 1;
    [SerializeField] private float climbToWalkDelay = 2;

    [SerializeField] private ShootPowerup shootPowerup;

    [SerializeField] private AudioClip deathClip;

    protected Animator animator;

    protected const string WALKING_STATE = "Walking";

    private EnemyState enemyState;
    private const string CLIMBING_STATE = "Climbing";
    private const string ATTACKING_STATE = "Attack";

    private Rigidbody[] rigidbodies;

    private AudioSource audioSource;

    private float attackTimer;

    private bool walkedForward;
    private bool hasClimbed;
    private bool isClimbing;
    private bool powerUpShot;

    private Coroutine climbRoutine;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Start()
    {
        attackTimer = Random.Range(minAttackTimer, maxAttackTimer);

        foreach (Rigidbody rb in rigidbodies)
            rb.isKinematic = true;
    }

    protected void Update()
    {
        if (!isDead)
        {
            switch (enemyState)
            {
                case EnemyState.Walking:
                    HandleWalkState();
                    break;

                case EnemyState.Climbing:
                    HandleClimbState();
                    break;

                case EnemyState.Attacking:
                    HandleAttackState();
                    break;
            }
        }
    }

    protected bool ArrivedAtTarget()
    {
        return Vector3.Distance(transform.position, Target.position) < 1f;
    }

    protected virtual void HandleWalkState()
    {
        animator.SetBool(WALKING_STATE, true);
        Vector3 pos = Vector3.MoveTowards(transform.position, Target.transform.position, walkSpeed * Time.deltaTime);
        if (!hasClimbed)
            pos.y = Terrain.activeTerrain.SampleHeight(pos);
        else
            pos.y = transform.position.y;

        transform.position = pos;

        Vector3 direction = Target.position - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);

        if (ArrivedAtTarget())
        {
            if (!hasClimbed)
            {
                animator.SetBool(WALKING_STATE, false);
                if (walkToClimbDelay > 0f)
                    walkToClimbDelay -= Time.deltaTime;
                else
                    enemyState = EnemyState.Climbing;
            }
            else if (!walkedForward)
            {
                Target = Camera.main.transform.root;
                walkedForward = true;
            }
            else
                enemyState = EnemyState.Attacking;
        }
    }

    protected void HandleClimbState()
    {
        if (!isClimbing && !hasClimbed)
            climbRoutine = StartCoroutine(EnemyClimb());

        if (ArrivedAtTarget())
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
                Target = Target.GetChild(0);
                enemyState = EnemyState.Walking;
            }
        }
    }

    protected void HandleAttackState()
    {
        animator.SetBool(WALKING_STATE, false);

        if (attackTimer > 0f)
            attackTimer -= Time.deltaTime;
        else
        {
            attackTimer = Random.Range(minAttackTimer, maxAttackTimer);
            animator.SetTrigger(ATTACKING_STATE);
            Target.GetComponent<Player>().Health -= dmg;
        }

        if (!ArrivedAtTarget())
        {
            enemyState = EnemyState.Walking;
        }
    }

    protected IEnumerator EnemyClimb()
    {
        isClimbing = true;

        foreach (Rigidbody rb in rigidbodies)
            rb.useGravity = false;

        float rotationOffset = 20;

        transform.rotation = Quaternion.Euler(transform.eulerAngles.x + rotationOffset, transform.eulerAngles.y, transform.eulerAngles.z);

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

        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.Euler(transform.eulerAngles.x - rotationOffset, transform.eulerAngles.y, transform.eulerAngles.z);
        time = 0;

        while (time < 1f)
        {
            time += Time.deltaTime;
            float progress = time / 1f;

            transform.rotation = Quaternion.Slerp(startRot, endRot, progress);

            yield return null;
        }

        transform.position = endPos;
    }

    public void Death()
    {
        if(!isDead)
        {
            if (climbRoutine != null)
                StopCoroutine(climbRoutine);

            if (!powerUpShot)
            {
                shootPowerup.LaunchPowerUp(transform);
                powerUpShot = true;
            }

            OnDeath?.Invoke(this);
            foreach (Rigidbody rb in rigidbodies)
            {
                rb.useGravity = true;
                rb.isKinematic = false;
            }
            animator.enabled = false;
            audioSource.PlayOneShot(deathClip);
            isDead = true;
            Destroy(this.gameObject, 2);
        }
    }
}
