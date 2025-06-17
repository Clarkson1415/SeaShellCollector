using Assets.Scripts;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    public int moveSpeed = 2;
    private Vector2 movementInput;

    [SerializeField] RandomSoundPlayer pickupSound;
    [SerializeField] AudioSource buySound;
    [SerializeField] AudioSource cannotDoSound;

    public List<ShopItem> Items;
    [SerializeField] private TextWithFeedback feedBackText;
    [SerializeField] private PlayerTopUI playerTopUI;

    private ShopItemSpawner shopItemSpawner;

    [SerializeField] private int _shellNumberPrivate = 0; // only serialized for testing;
    private PauseMenu pauseMenu;

    private int ShellNumber
    { 
        get => this._shellNumberPrivate;
        set 
        {
            this._shellNumberPrivate = value;
            this.playerTopUI.playerBag.UpdateBagFill((float)this.ShellNumber / this.MaxCapacity);
        }
    }

    private int _maxCapacity = 20;
    public int MaxCapacity 
    { 
        get => _maxCapacity;
        set
        {
            this._maxCapacity = value;
            this.playerTopUI.playerBag.UpdateBagFill((float)this.ShellNumber / value);
        }
    }

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        shopItemSpawner = FindFirstObjectByType<ShopItemSpawner>();
        this.pauseMenu = FindFirstObjectByType<PauseMenu>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Pickup>(out var p))
        {
            if (ShellNumber == MaxCapacity)
            {
                cannotDoSound.Play();
                feedBackText.ColourThenFade("Capacity full", Color.red);
                return;
            }

            ShellNumber++;
            Debug.Log($"Shell number {ShellNumber}");
            
            Destroy(p.gameObject);
            pickupSound.PlayRandomSound();
            feedBackText.ColourThenFade("+1", Color.green);
        }

        if (collision.TryGetComponent<ShopItem>(out var item))
        {
            if (this.ShellNumber < item.Cost)
            {
                cannotDoSound.Play();
                item.FlashTextRed();
                return;
            }

            this.playerTopUI.pickupList.AddToList(item);
            item.ApplyItemEffect(this);
            this.ShellNumber -= item.Cost;
            Items.Add(item);
            buySound.Play();
            feedBackText.ColourThenFade($"- {item.Cost}", Color.red);
            shopItemSpawner.RemoveItem(collision.gameObject);
        }
    }

    private void FixedUpdate()
    {
        var pauseMenuState = pauseMenu.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        if (pauseMenuState.IsName("SlideIn") || pauseMenuState.IsName("StayIn"))
        {
            return;
        }

        Vector2 move = moveSpeed * Time.fixedDeltaTime * movementInput;
        rb.MovePosition(rb.position + move);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        this.movementInput = context.ReadValue<Vector2>();
    }
}
