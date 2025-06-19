using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// This guy will auto select the button when the controls change to either wasd, arrows keys or controller.
/// </summary>
public class ButtonSelectorOnControlChange : MonoBehaviour, IPointerEnterHandler
{
    public GameObject buttonToSelect;
    public EventSystem mainEventSytem;
    private PauseMenu? pauseMenu;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer entered " + eventData.hovered);
        this.mainEventSytem.SetSelectedGameObject(null);
    }

    private void Awake()
    {
        this.pauseMenu = FindFirstObjectByType<PauseMenu>();
    }

    /// <summary>
    /// Called when controller stick or keyboard input is detected.
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (this.pauseMenu == null)
        {
            return;
        }

        var pauseMenuState = pauseMenu.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        if (!pauseMenuState.IsName("StayIn"))
        {
            Debug.Log("Not Stay in");
            return;
        }

        if (context.performed && this.mainEventSytem.currentSelectedGameObject == null)
        {
            Debug.Log("should do button things");
            this.mainEventSytem.SetSelectedGameObject(this.buttonToSelect); 
        }
    }
}
