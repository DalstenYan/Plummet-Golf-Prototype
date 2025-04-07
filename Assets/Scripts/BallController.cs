using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class BallController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Rigidbody rb;
    [SerializeField]
    private float maxHorizontalLaunchForce, maxVerticalLaunchForce;
    [SerializeField]
    CinemachineInputAxisController lookController;
    private double pauseInputTime;
    private PlayerInput playerInput;

    private SaveLastLocation saveLastLocation;


    private Vector2 deltaVector, startDragPosition, endDragPosition;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        saveLastLocation = GetComponent<SaveLastLocation>();

    }

    /// <summary>
    /// Registers the drag input from users
    /// </summary>
    /// <param name="context"></param>
    public void OnTapOrDragInput(InputAction.CallbackContext context) 
    {
        if (context.interaction is SlowTapInteraction) 
        {
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

    public void OnPauseInput(InputAction.CallbackContext context)
    {

        if (context.performed && (context.startTime != pauseInputTime))
        {
            pauseInputTime = context.startTime;
            //TogglePauseControls();
            PauseMenu.pauseMenu.OnPause();
        }
    }

    public void OnMouseDelta(InputAction.CallbackContext context) 
    {
        deltaVector += context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Shows the UI of the power and angle of where the ball will be headed
    /// </summary>
    private void ShowLaunchingUI() 
    {
        //TODO
        lookController.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        deltaVector = Vector2.zero;
        startDragPosition = deltaVector;
    }

    private void LaunchBall() 
    {
        //TODO
        saveLastLocation.newLastLocation();
        endDragPosition = deltaVector / 10.00f;
        Vector2 dragDifference = startDragPosition - endDragPosition;
        Debug.Log($"Start: {startDragPosition} - End: {endDragPosition} is: {dragDifference}");

        //Multiply by camera rotation
        Vector3 force = Camera.main.transform.rotation * ConstrainForce(dragDifference);
        Debug.Log("Final Force: " + force);
        rb.AddForce(force, ForceMode.VelocityChange);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        lookController.enabled = true;
    }

    private Vector3 ConstrainForce(Vector2 original) 
    {
        float x = original.x < 0 ? 
            Mathf.Max(original.x, -maxVerticalLaunchForce) : 
            Mathf.Min(original.x, maxVerticalLaunchForce);
        float y = original.y < 0 ?
            Mathf.Max(original.y, -maxHorizontalLaunchForce) :
            Mathf.Min(original.y, maxHorizontalLaunchForce);
        return new Vector3(x, y, y);
    }
    public void TogglePauseControls()
    {

        playerInput.SwitchCurrentActionMap(playerInput.currentActionMap.name == "Player" ? "UI" : "Player");
        Debug.Log("Action Map Changed to: " + playerInput.currentActionMap);
    }
    public void onLastLocationInput(InputAction.CallbackContext context)
    {
        if (context.performed==true)
        {
            saveLastLocation.backToLastLocation();
        }
    }
    public void ResetForce()
    {
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = Vector3.zero;
    }
}
