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

    private ShopItemSpawner shopItemSpawner;

    [SerializeField] private int _totalShellNumberPriv = 0; // only serialized for testing;

    private int TotalShells
    {
        get
        {
            return this._totalShellNumberPriv;
        }
        set
        {
            _totalShellNumberPriv = value;
            this.playerTopUI.playerBag.UpdateTotalShellCounter(this._totalShellNumberPriv);
        }
    }

    [SerializeField] private int _pinkShellNumberPrivate = 0; // only serialized for testing;
    private int PinkShellNumber
    {
        get => this._pinkShellNumberPrivate;
        set
        {
            this._pinkShellNumberPrivate = value;
            this.playerTopUI.playerBag.UpdatePinkShellCounter(this._pinkShellNumberPrivate);
            this.TotalShells += value;
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
        shopItemSpawner = FindFirstObjectByType<ShopItemSpawner>();
        this.pauseMenu = FindFirstObjectByType<PauseMenu>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
            pickupSound.PlayRandomSound();
            feedBackText.ColourThenFade("+1", Color.green);
        }

        if (collision.TryGetComponent<ShopItem>(out var item))
        {
            if (this.PinkShellNumber < item.Cost)
            {
                cannotDoSound.Play();
                item.FlashTextRed();
                return;
            }

            this.playerTopUI.pickupList.AddToList(item);
            item.ApplyItemEffect(this);
            this.PinkShellNumber -= item.Cost;
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

        if (timeout > 0)
        {
            StartCoroutine(RemoveModAfterTimeout(timeout, () => this.moveSpeedModifer -= value / 100f));
        }
    }

    public void ModifyMaxCap(int value, float timeout = -1f)
    {
        this.maxCapModifer += value;

        if (timeout > 0)
        {
            StartCoroutine(RemoveModAfterTimeout(timeout, () => this.maxCapModifer -= value));
        }
    }

    private IEnumerator RemoveModAfterTimeout(float timeout, System.Action removeAction)
    {
        yield return new WaitForSeconds(timeout);
        removeAction?.Invoke();
    }
}
