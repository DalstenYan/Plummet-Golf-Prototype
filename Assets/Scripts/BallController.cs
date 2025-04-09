using TMPro;
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

    private int strokes=0;
    [SerializeField] private TMP_Text stroketext;

    private SaveLastLocation saveLastLocation;

    [SerializeField]
    private float horizontalDragSensitivity, verticalDragSensitivity;

    private Vector2 deltaVector;

    void Start()
    {
        lookController.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        horizontalDragSensitivity /= 10f;
        verticalDragSensitivity /= 10f;
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
        if (context.started)
        {
            ShowLaunchingUI();
        }

        if (context.interaction is SlowTapInteraction) 
        {
            if (!context.started) 
            {
                LaunchBall();
            }
        }
        //Debug.Log(context.phase + " | " + context.interaction);
        
    }
    public void OnRightClickHold(InputAction.CallbackContext context)
    {
        //Debug.Log("Interaction: " + context.interaction + "\nPhase: " + context.phase );
        if (context.interaction is HoldInteraction)
        {
            bool isPanning = !context.canceled;
            Cursor.visible = !isPanning;
            lookController.enabled = isPanning;
            Cursor.lockState = isPanning ? CursorLockMode.Locked : CursorLockMode.Confined;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
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
        var delta = context.ReadValue<Vector2>();
        deltaVector.x *= horizontalDragSensitivity;
        deltaVector -= delta;
    }

    /// <summary>
    /// Shows the UI of the power and angle of where the ball will be headed
    /// </summary>
    private void ShowLaunchingUI() 
    {
        //TODO
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private void LaunchBall() 
    {
        //TODO
        saveLastLocation.newLastLocation();
        strokes += 1;
        stroketext.text = "Strokes: " + strokes;
        deltaVector /= 100f;

        //Multiply by camera rotation
        deltaVector.y *= verticalDragSensitivity;
        var camRot = Camera.main.transform.rotation;
        camRot.z = 0;
        Vector3 force = camRot * ConstrainForce(deltaVector);
        Debug.Log($" Delta Input: {deltaVector} \tFinal Force: {force}\nCamera Rotation: {camRot} ");
        rb.AddForce(force, ForceMode.VelocityChange);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        deltaVector = Vector2.zero;
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
    public void OnLastLocationInput(InputAction.CallbackContext context)
    {
        if (context.performed)
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
