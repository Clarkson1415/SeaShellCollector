using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private int shellNumber = 0;
    private Rigidbody2D rb;
    public int moveSpeed = 2;
    private Vector2 movementInput;
    [SerializeField] private TMP_Text textScore;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Pickup>(out var p))
        {
            shellNumber++;
            Debug.Log($"Shell number {shellNumber}");
            Destroy(collision.gameObject);
            textScore.text = shellNumber.ToString();
        }
    }

    private void FixedUpdate()
    {
        Vector2 move = moveSpeed * Time.fixedDeltaTime * movementInput;
        rb.MovePosition(rb.position + move);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        this.movementInput = context.ReadValue<Vector2>();
    }
}
