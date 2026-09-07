using UnityEngine;

public class Enemy : MonoBehaviour
{
    public event System.Action<Enemy> OnDeath;

    [HideInInspector] public Transform Target;

    [SerializeField] protected float walkSpeed;
    [SerializeField] protected float minAttackTimer;
    [SerializeField] protected float maxAttackTimer;

    protected Animator animator;
    protected float attackTimer;

    private const string WALKING_STATE = "Walking";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        animator.SetBool(WALKING_STATE, true);
        attackTimer = Random.Range(minAttackTimer, maxAttackTimer);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, Target.position) < 1f)
        {
            animator.SetBool(WALKING_STATE, false);
            if(attackTimer > 0f)
                attackTimer -= Time.deltaTime;
            else
            {
                animator.SetTrigger("Attack");
                attackTimer = Random.Range(minAttackTimer, maxAttackTimer);
            }
        }
        else
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, Target.transform.position, walkSpeed * Time.deltaTime);
            pos.y = Terrain.activeTerrain.SampleHeight(pos);
            transform.position = pos;
            transform.LookAt(Target);
        }
    }

    public void Death()
    {
        OnDeath?.Invoke(this);
        animator.enabled = false;
        Destroy(this.gameObject, 2);
    }
}
