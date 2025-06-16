using Assets.Scripts;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private int shellNumber = 0;
    private Rigidbody2D rb;
    public int moveSpeed = 2;
    private Vector2 movementInput;
    [SerializeField] private TMP_Text textScore;
    [SerializeField] RandomSoundPlayer pickupSound;
    [SerializeField] AudioSource buySound;
    [SerializeField] AudioSource failToBuySound;

    public List<ShopItem> Items;
    [SerializeField] private TextWithFeedback feedBackText;

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
            pickupSound.PlayRandomSound();
            feedBackText.ColourThenFade("+1", Color.green);
        }

        if (collision.TryGetComponent<ShopItem>(out var item))
        {
            if (this.shellNumber < item.Cost)
            {
                failToBuySound.Play();
                item.FlashTextRed();
                return;
            }

            this.shellNumber -= item.Cost;
            Items.Add(item);
            buySound.Play();
            feedBackText.ColourThenFade($"- {item.Cost}", Color.red);
            Destroy(collision.gameObject);
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
