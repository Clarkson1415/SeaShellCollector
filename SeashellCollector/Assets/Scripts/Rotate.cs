using System.Collections;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(RotateObject());
    }

    private IEnumerator RotateObject()
    {
        while (true)
        {
            this.transform.Rotate(0, 0, 1);
            yield return new WaitForSeconds(1f/90f); // Rotate at 90 degrees per second
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
