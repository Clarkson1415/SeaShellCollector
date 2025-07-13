using Assets.Scripts;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// This guy will auto select the button when the controls change to either wasd, arrows keys or controller.
/// </summary>
public class ButtonSelectorOnControlChange : MonoBehaviour, IPointerEnterHandler
{
    public EventSystem mainEventSytem;
    private GameMenu menu;

    private void Awake()
    {
        this.menu = this.GetComponent<GameMenu>();
        if (this.menu == null)
        {
            throw new ArgumentNullException($"NO MENU ON BUTTON SELECTOR {this.gameObject.name}");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer entered " + eventData.hovered);
        this.mainEventSytem.SetSelectedGameObject(null);
    }

    /// <summary>
    /// Called when controller stick or keyboard input is detected.
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (!this.menu.IsMenuOpen)
        {
            return;
        }

        if (context.performed && this.mainEventSytem.currentSelectedGameObject == null)
        {
            Debug.Log("should do button things");
            this.mainEventSytem.SetSelectedGameObject(this.menu.FirstButtonToSelect);
        }
    }
}
