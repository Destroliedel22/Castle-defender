using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float WalkSpeed;
    [SerializeField] protected float MinAttackTimer;
    [SerializeField] protected float MaxAttackTimer;
    [SerializeField] protected Transform Target;

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
        if (Vector3.Distance(transform.position, Target.position) < 1f)
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
            transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, WalkSpeed * Time.deltaTime);
            transform.LookAt(Target);
        }
    }

    public void Death()
    {
        animator.enabled = false;
    }
}
