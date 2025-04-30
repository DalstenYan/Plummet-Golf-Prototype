using System.Collections;
using UnityEngine;

public class BobbingBehavior : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    float bobHeight;
    [SerializeField]
    float animationTime;
    Vector3 bottomPos;
    float rotatedZ;

    private void Awake()
    {
        Debug.Log("World Space Rotation: " + transform.rotation + "Local Space Rotation: " + transform.localRotation);
        rotatedZ = 45f;
        bottomPos = transform.localPosition;
    }
    void OnEnable()
    {
        transform.localPosition = bottomPos;
        StartCoroutine(BobUpAndDown());
    }

    IEnumerator BobUpAndDown() 
    {
        Vector3 goal = transform.localPosition == bottomPos ? transform.localPosition + (Vector3.up * bobHeight) : bottomPos;
        Vector3 lerpOriginPos = transform.localPosition;
        float elapsedTime = 0;
        while (elapsedTime < animationTime) 
        {
            transform.localPosition = Vector3.Lerp(lerpOriginPos, goal, (elapsedTime/animationTime));
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        transform.localPosition = goal;
        StartCoroutine(BobUpAndDown());
    }

}
