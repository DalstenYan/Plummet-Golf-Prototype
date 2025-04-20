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
    CinemachineInputAxisController lookController;
    CinemachineCamera cinemachineCamera;
    private double pauseInputTime;
    private PlayerInput playerInput;

    public int strokes=0;
    [SerializeField] private TMP_Text stroketext;

    private SaveLastLocation saveLastLocation;

    [SerializeField]
    private float horizontalDragSensitivity, verticalDragSensitivity;

    private Vector2 deltaVector;

    private LineRenderer lr;
    private bool linerendering;

    [SerializeField] private AudioClip swingSound;
    [SerializeField] private AudioClip rollSound;

    void Start()
    {
        cinemachineCamera = GetComponentInChildren<CinemachineCamera>();
        cinemachineCamera.Follow = GameObject.FindWithTag("BallFollower").transform;
        lookController = GetComponentInChildren<CinemachineInputAxisController>();
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        saveLastLocation = GetComponent<SaveLastLocation>();
        lr = GetComponent<LineRenderer>();


        lookController.enabled = false;

        horizontalDragSensitivity /= 10f;
        verticalDragSensitivity /= 10f;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        
    }
    private void Update()
    {
        if(linerendering==true)
        {
            lr.positionCount = 2;
            lr.SetPosition(0, transform.position);
            Vector3 temp = deltaVector;
            temp /= 100f;
            temp.y *= verticalDragSensitivity;
            var camRot = Camera.main.transform.rotation;
            camRot.z = 0;
            Vector3 force = camRot * ConstrainForce(temp);
            
            force.y=0;
            lr.SetPosition(1, transform.position+force);

        }
        else
        {
            lr.positionCount = 0;
        }
    }

    /// <summary>
    /// Registers the drag input from users
    /// </summary>
    /// <param name="context"></param>
    public void OnTapOrDragInput(InputAction.CallbackContext context) 
    {
        //Debug.Log(context.interaction + ": " + context.phase);
        if (context.started && context.interaction is TapInteraction)
        {
            //Debug.Log("Force Reset");
            ResetForce();
            linerendering = true;
        }
        if (context.interaction is SlowTapInteraction) 
        {
            
            if(!context.started) 
            {
                LaunchBall();
                
            }
        }
    }

    public void OnRightClickHold(InputAction.CallbackContext context) 
    {
        
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

    private void LaunchBall() 
    {
        //TODO
        saveLastLocation.newLastLocation();
        strokes += 1;
        AudioSource.PlayClipAtPoint(swingSound, transform.position);
        AudioSource.PlayClipAtPoint(rollSound, transform.position);
        UIManager.Instance.UpdateTallyStrokes();
        //UpdateUI();
        deltaVector /= 100f;

        linerendering = false;
        //Multiply by camera rotation
        deltaVector.y *= verticalDragSensitivity;
        var camRot = Camera.main.transform.rotation;
        //camRot.y = 0;
        Vector3 force = camRot * ConstrainForce(deltaVector);
        Debug.Log($" Delta Input: {deltaVector} \tFinal Force: {force}\nCamera Rotation: {camRot} ");
       
        rb.AddForce(force, ForceMode.Impulse);
        deltaVector = Vector2.zero;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    private void UpdateUI() 
    {
        stroketext.text = "Strokes: " + strokes;
        
    }

    private Vector3 ConstrainForce(Vector2 original) 
    {
        float x = original.x < 0 ?
            Mathf.Max(original.x, -maxHorizontalLaunchForce) :
            Mathf.Min(original.x, maxHorizontalLaunchForce);
        float y = original.y < 0 ?
            Mathf.Max(original.y, -maxVerticalLaunchForce) :
            Mathf.Min(original.y, maxVerticalLaunchForce);
        return new Vector3(x, 0, y);
    }
    public void TogglePauseControls()
    {

        playerInput.SwitchCurrentActionMap(playerInput.currentActionMap.name == "Player" ? "UI" : "Player");
        //Debug.Log("Action Map Changed to: " + playerInput.currentActionMap);
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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        deltaVector = Vector2.zero;
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = Vector3.zero;
    }
}
