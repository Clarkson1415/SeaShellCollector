using System.Collections;
using UnityEngine;

public class AnimalAnimatorStateMachine : MonoBehaviour
{
    [SerializeField] private string LandTrigger;
    [SerializeField] private string TakeOffTrigger;
    private Animator animator;

    private enum BirdState { Flying, OnGround, Landing, TakingOff }
    private BirdState state = BirdState.Flying;
    private Vector3 target;

    public Vector3 areaCenter => this.GroundArea.bounds.center;
    public BoxCollider2D GroundArea; // Will fly above and land within this area.
    public float flySpeed = 5f;
    public float rotationSpeed = 2f;
    public float OnGroundDuration = 3f;
    public float chanceToLand = 0.2f; // 20% chance at each waypoint

    private void Awake()
    {
        animator = GetComponent<Animator>();
        PickNewTarget();
    }

    void Update()
    {
        switch (state)
        {
            case BirdState.Flying:
                FlyToTarget();
                break;
            case BirdState.OnGround:
                // Just idle
                break;
        }
    }

    void FlyToTarget()
    {
        Vector3 direction = (target - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, target, flySpeed * Time.deltaTime);

        if (direction.x > 0 && this.transform.localScale.x > 0)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x * -1, this.transform.localScale.y, this.transform.localScale.z);
        }

        if (Vector3.Distance(transform.position, target) < 0.2f)
        {
            if (Random.value < chanceToLand)
            {
                StartCoroutine(LandAndRest());
            }
            else
            {
                PickNewTarget();
            }
        }
    }

    void PickNewTarget()
    {
        this.animator.SetTrigger(TakeOffTrigger);
        Vector3 randomOffset = new Vector3(
            Random.Range(-GroundArea.bounds.size.x / 2, GroundArea.bounds.size.x / 2),
            Random.Range(2, GroundArea.bounds.size.y), // stay above ground when flying
            0 // Z is 0 in 2d
        );

        target = areaCenter + randomOffset;
        state = BirdState.Flying;
    }

    IEnumerator LandAndRest()
    {
        // Pick a ground target
        target = new Vector3(
            transform.position.x + Random.Range(-5, 5),
            0, // ground level
            transform.position.z + Random.Range(-5, 5)
        );

        state = BirdState.Landing;

        // Wait until we reach the ground
        while (Vector3.Distance(transform.position, target) > 0.2f)
        {
            Vector3 dir = (target - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, target, flySpeed * Time.deltaTime);
            yield return null;
        }

        this.animator.SetTrigger(LandTrigger);
        state = BirdState.OnGround;
        yield return new WaitForSeconds(OnGroundDuration);
        PickNewTarget();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(areaCenter, GroundArea.bounds.size);
    }
}
