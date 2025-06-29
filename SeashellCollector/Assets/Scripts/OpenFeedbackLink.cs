using UnityEngine;

public class OpenFeedbackLink : MonoBehaviour
{

    public void OpenLinkInBrowser()
    {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSdT4LpxD-lWzLlDZGkLI6id90lVAFRPQgHT523PM5DIF0WZtQ/viewform");
    }
}
