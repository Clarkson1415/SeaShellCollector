using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private int baseMoveSpeed = 2;
    [SerializeField] private float moveSpeedModifer = 1;

    private Vector2 movementInput;

    [SerializeField] RandomSoundPlayer pickupSound;
    [SerializeField] AudioSource buySound;
    [SerializeField] AudioSource cannotDoSound;

    private PauseMenu pauseMenu;
    public List<ShopItem> Items;
    [SerializeField] private TextWithFeedback feedBackText;
    [SerializeField] private PlayerTopUI playerTopUI;

    private int TotalShells
    {
        get
        {
            return this._pinkShellNumberPrivate;
        }
    }

    [SerializeField] private int _pinkShellNumberPrivate = 0; // only serialized for testing;
    private int PinkShellNumber
    {
        get => this._pinkShellNumberPrivate;
        set
        {
            if (value > 0)
            {
                pickupSound.PlayRandomSound();
            }
            else
            {
                buySound.Play();
            }

            feedBackText.ColourThenFade(value - this._pinkShellNumberPrivate);
            this._pinkShellNumberPrivate = value;
            this.playerTopUI.playerBag.UpdatePinkShellCounter(this._pinkShellNumberPrivate);
            this.playerTopUI.playerBag.UpdateTotalShellCounter(this.TotalShells); // ew I have to do this in every type of shell Setter.
        }
    }

    [SerializeField] private int maxCapModifer = 0; // only serialized for testing;

    [SerializeField] private int _maxCapacity = 20;

    private int MaxCapacity
    {
        get => _maxCapacity + maxCapModifer;
        set
        {
            this._maxCapacity = value;
            this.playerTopUI.playerBag.UpdateMaxCapacity(_maxCapacity + maxCapModifer);
        }
    }

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        this.pauseMenu = FindFirstObjectByType<PauseMenu>();

        this.playerTopUI.playerBag.UpdateMaxCapacity(_maxCapacity + maxCapModifer);
        this.playerTopUI.playerBag.UpdatePinkShellCounter(this._pinkShellNumberPrivate);
        this.playerTopUI.playerBag.UpdateTotalShellCounter(this.TotalShells); // ew I have to do this in every type of shell Setter.

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If sandcastle = pickup from it
        // if critter pickup from it.

        if (collision.TryGetComponent<Pickup>(out var p))
        {
            if (PinkShellNumber == MaxCapacity)
            {
                cannotDoSound.Play();
                feedBackText.ColourThenFade("Capacity full", Color.red);
                return;
            }

            PinkShellNumber++;
            Debug.Log($"Shell number {PinkShellNumber}");
            Destroy(p.gameObject);
        }
        else if (collision.TryGetComponent<ShopItem>(out var item))
        {
            if (this.PinkShellNumber < item.Cost)
            {
                cannotDoSound.Play();
                item.FlashTextRed();
                return;
            }

            Debug.Log("picked up item: " + item.name);
            this.PinkShellNumber -= item.Cost;
            item.ApplyItemEffects(this);
            
            if (item is not AutomationShopItem)
            {
                Items.Add(item);
                this.playerTopUI.pickupList.AddToList(item);
            }

            item.ShopBelongsTo.BoughtItem(item);

            if (item.Timeout > 0)
            {
                StartCoroutine(RemoveItemAfterTimeout(item));
            }
        }
        else if (collision.TryGetComponent<Sandcastle>(out var sandy))
        {
            if (sandy.PickupStore.Count > 0)
            {
                this.PinkShellNumber += sandy.PickupStore.Count;
            }

            Debug.Log("Just get total from sandcastle for now untill have specific types for player and critters to collect.");
        }
    }

    private IEnumerator RemoveItemAfterTimeout(ShopItem item)
    {
        Debug.Log("Removed item after timeout: " + item.name);
        yield return new WaitForSeconds(item.Timeout);
        item.RemoveEffects(this);
        this.playerTopUI.pickupList.Remove(item);
        this.Items.Remove(item);
    }

    private void FixedUpdate()
    {
        var pauseMenuState = pauseMenu.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        if (pauseMenuState.IsName("SlideIn") || pauseMenuState.IsName("StayIn"))
        {
            return;
        }

        Vector2 move = baseMoveSpeed * moveSpeedModifer * Time.fixedDeltaTime * movementInput;
        rb.MovePosition(rb.position + move);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        this.movementInput = context.ReadValue<Vector2>();
    }

    public void ModifySpeed(int value, float timeout = -1f)
    {
        this.moveSpeedModifer += value / 100f;
    }

    public void ModifyMaxCap(int value, float timeout = -1f)
    {
        this.maxCapModifer += value;
    }
}
