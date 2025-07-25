using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bottle bobbing updown in the ocean.
/// </summary>
public class BobGameObjectUpDown : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] bool ApplyToChildren = false;
    public float amplitudeY = 0.5f; // How much to bob up and down
    public float amplitudeX = 0.2f; // Optional side-to-side sway
    public float speed = 1f;        // Speed of the bobbing motion
    public Vector2 phaseOffset = Vector2.zero; // Optional offset to make different objects bob uniquely

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * speed + phaseOffset.y) * amplitudeY;
        float xOffset = Mathf.Cos(Time.time * speed + phaseOffset.x) * amplitudeX;
        var offsetVector = new Vector3(xOffset, yOffset, 0f);

        //List<Transform> originalChildrenTransforms = new();
        //foreach(Transform child in transform)
        //{
        //    originalChildrenTransforms.Add(child);
        //}

        //transform.localPosition = startPos + offsetVector;

        //if (!ApplyToChildren)
        //{
        //    foreach (Transform child in transform)
        //    {
        //        child.localPosition -= // set to original before the move
        //    }
        //}
    }
}
