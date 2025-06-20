using EasyTransition;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private string sceneName = "Game";
    [SerializeField] private TransitionSettings transitionSettings;
    public string? testNullable;

    public void GotoScene()
    {
        // Transition 
        TransitionManager.Instance().Transition(this.sceneName, transitionSettings, 0f);
    }
}
