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

    [SerializeField] protected float walkSpeed;
    [SerializeField] protected float climbTime;
    [SerializeField] protected float minAttackTimer;
    [SerializeField] protected float maxAttackTimer;

    protected Animator animator;
    protected float attackTimer;

    private EnemyState enemyState;
    private const string WALKING_STATE = "Walking";
    private Rigidbody[] rigidbodies;

    private bool isClimbing;

    private Coroutine climbRoutine;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
    }

    private void Start()
    {
        animator.SetBool(WALKING_STATE, true);
        attackTimer = Random.Range(minAttackTimer, maxAttackTimer);
    }

    private void Update()
    {
        if(enemyState == EnemyState.Walking)
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, Target.transform.position, walkSpeed * Time.deltaTime);
            pos.y = Terrain.activeTerrain.SampleHeight(pos);
            transform.position = pos;
            transform.LookAt(Target);
            if (Vector3.Distance(transform.position, Target.position) < 1f)
                enemyState = EnemyState.Climbing;
        }
        else if(enemyState == EnemyState.Climbing)
        {
            if (isClimbing == false)
                climbRoutine = StartCoroutine(EnemyClimb());
        }
        else if(enemyState == EnemyState.Attacking)
        {
            if (attackTimer > 0f)
                attackTimer -= Time.deltaTime;
            else
            {
                attackTimer = Random.Range(minAttackTimer, maxAttackTimer);
                animator.SetTrigger("Attack");
            }
        }
    }

    IEnumerator EnemyClimb()
    {
        isClimbing = true;
        enemyState = EnemyState.Climbing;

        foreach (Rigidbody rb in rigidbodies)
            rb.useGravity = false;

        float rotationOffset = 20;

        transform.rotation = Quaternion.Euler(transform.rotation.x + rotationOffset, transform.rotation.y, transform.rotation.z);

        animator.SetBool(WALKING_STATE, false);
        animator.SetTrigger("Climbing");

        Transform NewTarget = Target.GetChild(0);

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
            rb.useGravity = false;
        animator.enabled = false;
        Destroy(this.gameObject, 2);
    }
}
