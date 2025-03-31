using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform playerTransform;
    Vector3 cameraOffset;
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        cameraOffset = transform.position - playerTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerTransform.position + cameraOffset;
    }
}
