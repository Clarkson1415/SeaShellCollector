using UnityEngine;

public class ToolTip : MonoBehaviour
{
    public GameObject toolTip;

    private void Awake()
    {
        toolTip.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        toolTip.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        toolTip.SetActive(false);
    }
}
