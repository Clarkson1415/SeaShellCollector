using System;
using UnityEngine;

public class GlobalShadowController : MonoBehaviour
{
    [Range(0f, 24f)]
    [SerializeField] private float timeOfDay = 12f;

    [Tooltip("Max shadow length in units.")]
    [SerializeField] private float maxLength = 2f;

    [SerializeField] private float OffsetY = 8f;

    private float lastOffsetY = 8f;

    private float lastTimeOfDay = 0f;

    private float lastMaxLength = 2f;

    private ShadowLengthController[] shadowControllers = Array.Empty<ShadowLengthController>();

    private void Start()
    {
        shadowControllers = FindObjectsByType<ShadowLengthController>(FindObjectsSortMode.None);
    }

    // Update is called once per frame
    void Update()
    {
        if (lastTimeOfDay != timeOfDay)
        {
            lastTimeOfDay = timeOfDay;
            UpdateShadowTimeOfDay();
        }

        if (lastMaxLength != maxLength)
        {
            lastMaxLength = maxLength;
            UpdateShadowLength();
        }
    }

    private void UpdateShadowTimeOfDay()
    {
        foreach(var shadow in this.shadowControllers)
        {
            if (shadow != null)
            {
                shadow.timeOfDay = this.timeOfDay;
            }
        }
    }

    private void UpdateShadowLength()
    {
        foreach (var shadow in this.shadowControllers)
        {
            if (shadow != null)
            {
                shadow.maxLength = this.maxLength;
            }
        }
    }
}
