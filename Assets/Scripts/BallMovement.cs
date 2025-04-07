using UnityEngine;

public class BallMovement : MonoBehaviour
{

    [Header("Refrences")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private LineRenderer lr;

    [Header("Attributes")]
    [SerializeField] private float maxPower;
    [SerializeField] private float power;
    [SerializeField] private float maxGoalSpeed;

    private bool isDragging;
    private bool inHole;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
