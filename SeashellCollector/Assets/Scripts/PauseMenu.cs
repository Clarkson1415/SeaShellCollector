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
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    public void TogglePauseMenu(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        this.animator.SetTrigger("Toggle");
        this.mainEventSytem.SetSelectedGameObject(null); // Make sure the button does not be select when pause menu is away.

        if (Time.timeScale == 1)
        {
            Time.timeScale = 0; // Pause the game.
        }
        else
        {
            Time.timeScale = 1; // Resume the game.
        }
    }
}
