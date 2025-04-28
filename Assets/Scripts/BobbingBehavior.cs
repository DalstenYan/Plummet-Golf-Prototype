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
        rotatedZ = 45f;
        bottomPos = transform.localPosition;
    }
    void OnEnable()
    {
        Debug.Log("World Space: " + transform.position + " || Local Space: " + transform.localPosition);
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

    private void FixedUpdate()
    {
        //transform.rotation = new Quaternion(transform.rotation.x, Camera.main.transform.rotation.y, transform.rotation.z, Camera.main.transform.rotation.w); 
        //transform.RotateAround(Vector3.zero, Camera.main.transform.up, );
        //var rot = Camera.main.transform.rotation;
        //rot.z = 45;
        //rot.y = 0;
        //rot.x = 0;
        //rot.w = 0;
        //transform.rotation = rot;
        Vector3 rot = Quaternion.LookRotation(Camera.main.transform.position - transform.position).eulerAngles;
        rot.x = 0;
        rot.z = rotatedZ;
        transform.rotation = Quaternion.Euler(rot);

    }

}
