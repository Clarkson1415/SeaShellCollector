using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(ButtonSelectorOnControlChange))]
    public abstract class GameMenu : MonoBehaviour
    {
        /// <summary>
        /// If menu buttons can be selected by controller or keyboard instead of mouse.
        /// </summary>
        public abstract bool IsMenuOpen { get; }

        /// <summary>
        /// But to be selected when the controls change from mouse to keyboard or controller.
        /// </summary>
        public GameObject FirstButtonToSelect;
    }
}
