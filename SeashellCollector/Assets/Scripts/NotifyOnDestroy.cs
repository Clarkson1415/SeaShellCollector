using UnityEngine;

public class NotifyOnDestroy : MonoBehaviour
{
    public GameObject objectToNotify;

    public System.Action OnDestroyed;

    private void OnDestroy()
    {
        OnDestroyed?.Invoke();
    }
}
