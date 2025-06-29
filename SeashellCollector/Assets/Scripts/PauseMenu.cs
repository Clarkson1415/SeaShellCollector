using Assets.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseMenu : GameMenu
{
    public override bool IsMenuOpen => this.animator.GetCurrentAnimatorStateInfo(0).IsName("StayIn");

    private Animator animator;
    [SerializeField] private EventSystem mainEventSytem;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void TogglePauseMenu(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        // If menu is not open yet, then we are toggling it on. So pause time.
        if (!IsMenuOpen)
        {
            Time.timeScale = 0;
        }

        this.animator.SetTrigger("Toggle");
        this.mainEventSytem.SetSelectedGameObject(null); // Make sure the button does not be select when pause menu is away.
    }
}
