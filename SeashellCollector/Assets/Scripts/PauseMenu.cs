using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private EventSystem mainEventSytem;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void TogglePauseMenu(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            this.animator.SetTrigger("Toggle");
            this.mainEventSytem.SetSelectedGameObject(null); // Make sure the button does not be select when pause menu is away.
        }
    }
}
