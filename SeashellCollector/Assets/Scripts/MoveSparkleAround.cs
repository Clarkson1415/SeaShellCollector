using System.Collections;
using UnityEngine;

public class MoveSparkleAround : MonoBehaviour
{
    private BoxCollider2D parentsBoxCol;
    private Animator animator;

    private void Start()
    {
        parentsBoxCol = this.GetComponentInParent<BoxCollider2D>();
        animator = this.GetComponent<Animator>();

        StartCoroutine(SparkleMovement());
    }

    private IEnumerator SparkleMovement()
    {
        while (true)
        {
            while (animator.GetCurrentAnimatorStateInfo(0).IsName("Sparkle"))
            {
                yield return null;
            }

            MoveToRandomPosition();
            this.animator.SetTrigger("Sparkle");
            yield return new WaitForEndOfFrame(); // Wait for Sparkle anim to start
        }
    }

    /// <summary>
    /// Move to random position in box collider bounds.
    /// </summary>
    private void MoveToRandomPosition()
    {
        var newX = Random.Range(parentsBoxCol.bounds.min.x, parentsBoxCol.bounds.max.x);
        var newY = Random.Range(parentsBoxCol.bounds.min.y, parentsBoxCol.bounds.max.y);
        float randomAngle = Random.Range(0f, 360f);
        Quaternion newRotation = Quaternion.Euler(0f, 0f, randomAngle);
        this.transform.SetPositionAndRotation(new Vector3(newX, newY), newRotation);
    }
}
