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

        UpdateShadowTimeOfDay();
        UpdateShadowLength();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateShadowsIfChanged();
    }

    private void UpdateShadowsIfChanged()
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

    public void UpdateSingleShadow(ShadowLengthController shadow)
    {
        shadow.timeOfDay = this.timeOfDay;
        shadow.maxLength = this.maxLength;
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
