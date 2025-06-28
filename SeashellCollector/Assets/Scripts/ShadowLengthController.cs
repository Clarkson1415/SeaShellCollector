using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

/// <summary>
/// Fakes shadow length changes in 2D by scaling a shadow sprite
/// based on a timeOfDay value (0 = noon, 1 = sunrise/sunset).
/// Attach this to the shadow GameObject (child of the object casting the shadow).
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class ShadowLengthController : MonoBehaviour
{
    [Range(0f, 24f)]
    public float timeOfDay = 12f;

    [Tooltip("Max shadow length in units.")]
    public float maxLength = 2f;

    private Vector3 initialLocalScale;
    private Vector3 initialLocalPosition;

    private SpriteRenderer shadowSpriteRenderer;
    private GameObject shadowSpriteObject;

    private void Start()
    {
        initialLocalPosition = transform.localPosition;
        initialLocalScale = transform.localScale;

        shadowSpriteObject = new GameObject("ShadowSprite");
        shadowSpriteObject.transform.position = new Vector3(0f, 0f, 0f); // Position at origin
        shadowSpriteObject.transform.SetParent(transform, true); // Set as child of this GameObject
        this.shadowSpriteRenderer = shadowSpriteObject.GetComponent<SpriteRenderer>();
        var colourSpriteRender = this.GetComponent<SpriteRenderer>();
        
        if (this.shadowSpriteRenderer == null || colourSpriteRender == null)
        {
            Debug.LogError("ShadowLengthController requires a SpriteRenderer on both the shadow and its parent.");
            return;
        }

        this.shadowSpriteRenderer.sprite = colourSpriteRender.sprite; // Use parent's sprite
        this.shadowSpriteRenderer.color = Color.black; // Match parent's color
        this.shadowSpriteRenderer.color = new Color(this.shadowSpriteRenderer.color.r, this.shadowSpriteRenderer.color.g, this.shadowSpriteRenderer.color.b, 0.5f); // Semi-transparent shadow
        this.shadowSpriteRenderer.sortingOrder = colourSpriteRender.sortingOrder - 1; // Ensure shadow is drawn behind parent
    }
    void Update()
    {
        // Calculate how far from noon we are (0 at noon, 1 at sunrise/sunset)
        float t = Mathf.Abs(timeOfDay - 12f) / 12f;

        // Calculate current length based on t
        float length = Mathf.Lerp(0f, maxLength, t);

        // Determine if shadow is below (morning) or above (evening)
        bool isMorning = timeOfDay < 12f;

        // Set shadow scale Y (always positive, no flip)
        shadowSpriteObject.transform.localScale = new Vector3(
            initialLocalScale.x,
            length,
            initialLocalScale.z
        );

        // Move shadow position: below object for morning, above for evening
        float yOffset = isMorning ? -length / YOffsetModifier : length / YOffsetModifier;

        // Position relative to the object (assuming pivot at bottom of shadow sprite)
        shadowSpriteObject.transform.localPosition = initialLocalPosition + new Vector3(0f, yOffset, 0f);
    }

    public float YOffsetModifier = 2f;
}
