using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class BallController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Rigidbody rb;
    [SerializeField]
    private float launchForce;

    private Vector2 deltaVector, startDragPosition, endDragPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Registers the drag input from users
    /// </summary>
    /// <param name="context"></param>
    public void OnTapOrDragInput(InputAction.CallbackContext context) 
    {
        if (context.interaction is SlowTapInteraction) 
        {
            Debug.Log("Slow Tap");
            if (context.started)
            {
                ShowLaunchingUI();
            }
            else if (context.canceled || context.performed) 
            {
                LaunchBall();
            }
        }
        //Debug.Log(context.phase + " | " + context.interaction);
        
    }

    public void OnMouseDelta(InputAction.CallbackContext context) 
    {
        deltaVector += context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        //Debug.Log(dragDirection);
    }

    /// <summary>
    /// Shows the UI of the power and angle of where the ball will be headed
    /// </summary>
    private void ShowLaunchingUI() 
    {
        //TODO
        deltaVector = Vector2.zero;
        startDragPosition = deltaVector;
    }

    private void LaunchBall() 
    {
        //TODO
        endDragPosition = deltaVector / 10.00f;
        Vector2 dragDifference = startDragPosition - endDragPosition;
        Debug.Log($"Start: {startDragPosition} - End: {endDragPosition} is: {dragDifference}");
        Vector3 force = new Vector3(dragDifference.x, dragDifference.y, dragDifference.y);
        Debug.Log(force);
        rb.AddForce(force, ForceMode.Impulse);
    }


}
