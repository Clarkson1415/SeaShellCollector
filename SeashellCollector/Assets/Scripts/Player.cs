using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private int baseMoveSpeed = 2;
    [SerializeField] private float moveSpeedModifer = 1;
    private Vector2 movementInput;

    [Header("Sound")]
    [SerializeField] RandomSoundPlayer pickupSound;
    [SerializeField] AudioSource buySound;
    [SerializeField] AudioSource cannotDoSound;
    [SerializeField] AudioSource footstepsAudio;

    [Header("UI")]
    [SerializeField] private TextWithFeedback feedBackText;
    [SerializeField] private PlayerTopUI playerTopUI;
    [SerializeField] private MultiPickupFeedback multiPickupFeedback;
    private PauseMenu pauseMenu;

    private List<ShopItem> boughtItems = new();

    private List<Pickup> _totalPickupsPriv = new();
    private List<Pickup> TotalPickups
    {
        get
        {
            return this._totalPickupsPriv;
        }
        set
        {
            this._totalPickupsPriv = value;
            this.playerTopUI.playerBag.UpdateMoneyCounterUi(this.TotalPickups);
        }
    }

    public List<Pickup> GetCopyOfPickups()
    {
        return new List<Pickup>(this.TotalPickups);
    }

    private int PinkShellCount
    {
        get => this._totalPickupsPriv.Where(x => x.PickupType == PickupType.PinkShell).Count();
    }

    private int CoralCount
    {
        get => this._totalPickupsPriv.Where(x => x.PickupType == PickupType.Coral).Count();
    }

    private int PearlCount
    {
        get => this._totalPickupsPriv.Where(x => x.PickupType == PickupType.Pearl).Count();
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


    [Header("For Cheats")]
    [SerializeField] private Pickup pinkShellPickup;
    [SerializeField] private int pinkShellCheatNum;
    [SerializeField] private Pickup coralPickup;
    [SerializeField] private int coralCheatNum;
    [SerializeField] private Pickup pearlPickup;
    [SerializeField] private int pearlCheatNum;

    // Static collection to track all active collectors
    private static HashSet<Player> AllPlayers = new HashSet<Player>();
    public static int PlayersInScene => AllPlayers.Count;

    private void OnEnable()
    {
        AllPlayers.Add(this);
    }

    private void OnDisable()
    {
        AllPlayers.Remove(this);
    }

    private void OnDestroy()
    {
        AllPlayers.Remove(this);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
        this.pauseMenu = FindFirstObjectByType<PauseMenu>();

        this.CheckUpdateCheatValues();
    }

    private void CheckUpdateCheatValues()
    {
        if (this.TotalPickups.Count() >= (pinkShellCheatNum + coralCheatNum + pearlCheatNum))
        {
            return;
        }

        this.TotalPickups.Clear();

        List<Pickup> pickupShells = Enumerable.Repeat(pinkShellPickup, pinkShellCheatNum).ToList();
        this.TotalPickups = new List<Pickup>(this.TotalPickups.Concat(pickupShells));

        List<Pickup> pickupCoral = Enumerable.Repeat(coralPickup, coralCheatNum).ToList();
        this._totalPickupsPriv = new List<Pickup>(this.TotalPickups.Concat(pickupCoral));

        List<Pickup> pickupPearl = Enumerable.Repeat(pearlPickup, pearlCheatNum).ToList();
        this._totalPickupsPriv = new List<Pickup>(this.TotalPickups.Concat(pickupPearl));

        this.playerTopUI.playerBag.UpdateMaxCapacity(_maxCapacity + maxCapModifer);
        this.playerTopUI.playerBag.UpdateMoneyCounterUi(this.TotalPickups);
    }

    /// <summary>
    /// Must do this to trigger the setter.
    /// </summary>
    public void AddPickups(List<Pickup> picks)
    {
        this.multiPickupFeedback.ShowPickups(picks);//TODO test this

        pickupSound.PlayRandomSound();
        this.TotalPickups = new List<Pickup>(this.TotalPickups.Concat(picks));
    }

    private bool CanBuy(ShopItem item)
    {
        return this.PinkShellCount >= item.PinkShellCost && this.CoralCount >= item.CoralCost && this.PearlCount >= item.PearlCost;
    }

    /// <summary>
    /// Must do this to trigger the setter. Type used to pay, value to pay from those pickups.
    /// </summary>
    private void PayForItem(ShopItem item)
    {
        buySound.Play();

        var pinkShellsList = new List<Pickup>(this.TotalPickups.Where(x => x.PickupType == PickupType.PinkShell).ToList());
        pinkShellsList.RemoveRange(0, item.PinkShellCost);

        var coralList = new List<Pickup>(this.TotalPickups.Where(x => x.PickupType == PickupType.Coral).ToList());
        coralList.RemoveRange(0, item.CoralCost);

        var pearlList = new List<Pickup>(this.TotalPickups.Where(x => x.PickupType == PickupType.Pearl).ToList());
        pearlList.RemoveRange(0, item.PearlCost);

        this.TotalPickups = new List<Pickup>(pinkShellsList.Concat(coralList).Concat(pearlList));
        feedBackText.ColourThenFade(-(item.PinkShellCost + item.CoralCost + item.PearlCost));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If sandcastle -> pickup from it
        // if critter pickup from it.

        if (collision.TryGetComponent<Pickup>(out var p))
        {
            if (this.TotalPickups.Count + 1 > MaxCapacity)
            {
                cannotDoSound.Play();
                feedBackText.ColourThenFade("Capacity full", Color.red);
                return;
            }

            this.AddPickups(new List<Pickup> { p });
            p.gameObject.SetActive(false); // Return to object pool.
        }
        else if (collision.TryGetComponent<ShopItem>(out var item))
        {
            if (!CanBuy(item))
            {
                cannotDoSound.Play();
                item.FlashTextRed();
                feedBackText.ColourThenFade("Can't afford :(", Color.red);
                return;
            }

            Debug.Log("item: " + item.name);
            this.PayForItem(item);

            item.ApplyItemEffects(this);
            if (item is not AutomationShopItem)
            {
                boughtItems.Add(item);
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
            if (sandy.GetCopyOfPickups().Count == 0)
            {
                return;
            }
            else if (this.TotalPickups.Count + sandy.GetCopyOfPickups().Count > MaxCapacity)
            {
                cannotDoSound.Play();
                feedBackText.ColourThenFade("Capacity full", Color.red);
                return;
            }

            this.AddPickups(sandy.TakePickups());
        }
    }

    private IEnumerator RemoveItemAfterTimeout(ShopItem item)
    {
        Debug.Log("Removed item after timeout: " + item.name);
        yield return new WaitForSeconds(item.Timeout);
        item.RemoveEffects(this);

        MyLog.Log("Need to store index added at to remove not item.");

        this.playerTopUI.pickupList.Remove(item);
        this.boughtItems.Remove(item);
    }

    private void FixedUpdate()
    {
        CheckUpdateCheatValues();

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
        this.animator.SetFloat("velocityX", this.movementInput.x);
        this.animator.SetFloat("velocityY", this.movementInput.y);

        if (this.movementInput != Vector2.zero)
        {
            this.animator.SetFloat("lastVelocityX", this.movementInput.x);
            this.animator.SetFloat("lastVelocityY", this.movementInput.y);
        }

        if (context.started)
        {
            this.animator.SetTrigger("Walk");
            this.footstepsAudio.Play();
        }
        else if (context.canceled)
        {
            this.animator.SetTrigger("Idle");
            this.footstepsAudio.Stop();
        }
    }

    public void ModifySpeed(int value, float timeout = -1f)
    {
        this.moveSpeedModifer += value / 100f;
    }

    public void ModifyMaxCap(int value, float timeout = -1f)
    {
        this.maxCapModifer += value;
        this.playerTopUI.playerBag.UpdateMaxCapacity(_maxCapacity + maxCapModifer);
    }
}
