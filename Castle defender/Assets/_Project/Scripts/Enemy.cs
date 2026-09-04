using UnityEngine;

public class Enemy : MonoBehaviour
{
    public event System.Action<Enemy> OnDeath;

    public Transform target;

    [SerializeField] protected float WalkSpeed;
    [SerializeField] protected float MinAttackTimer;
    [SerializeField] protected float MaxAttackTimer;

    protected Animator animator;
    protected float attackTimer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        animator.SetBool("Walking", true);
        attackTimer = Random.Range(MinAttackTimer, MaxAttackTimer);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, target.position) < 1f)
        {
            animator.SetBool("Walking", false);
            if(attackTimer > 0f)
                attackTimer -= Time.deltaTime;
            else
            {
                animator.SetTrigger("Attack");
                attackTimer = Random.Range(MinAttackTimer, MaxAttackTimer);
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, WalkSpeed * Time.deltaTime);
            transform.LookAt(target);
        }
    }

    public void Death()
    {
        OnDeath?.Invoke(this);
        animator.enabled = false;
        Destroy(this.gameObject, 2);
    }
}
