using UnityEngine;

public class ShieldEnemy : Enemy
{
    [SerializeField] private int blockChance;
    [SerializeField] private float minBlockTime;
    [SerializeField] private float maxBlockTime;

    private float blockTime;
    private bool blocking;

    private const string BLOCKING_STATE = "Block";

    private void HandleBlockState()
    {
        int random = Random.Range(0, 100);
        if (random < blockChance)
        {
            blocking = true;

            animator.SetBool(WALKING_STATE, false);
            animator.SetBool(BLOCKING_STATE, true);

            blockTime = Random.Range(minBlockTime, maxBlockTime);

            if (blockTime > 0f)
                blockTime -= Time.deltaTime;
            else
            {
                blocking = false;

                animator.SetBool(BLOCKING_STATE, false);
                animator.SetBool(WALKING_STATE, true);
            }
        }
    }

    protected override void HandleWalkState()
    {
        HandleBlockState();
        if (!blocking)
            base.HandleWalkState();
    }
}
