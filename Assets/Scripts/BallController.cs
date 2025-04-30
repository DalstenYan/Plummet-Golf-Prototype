using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class BallController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Rigidbody rb;
    [Header("Ball Settings")]
    [SerializeField]
    private Transform cameraFollowTransform;
    [SerializeField]
    private float leftRightPanSensitivity, upDownPanSensitivity;
    [SerializeField]
    private float maxLeftRightLaunchForce, maxForwardBackwardLaunchForce;
    [SerializeField]
    private float leftRightDragSensitivity, forwardBackwardDragSensitivity;

    public int strokes = 0;
    private SaveLastLocation saveLastLocation;
    private double pauseInputTime;
    private PlayerInput playerInput;
    private Vector2 deltaVector, cameraDelta;

    private LineRenderer lr;
    private bool linerendering;
    private bool canLaunchBall;
    private bool isPanning;
    public bool inCutscene;

    [Header("Audio")]
    [SerializeField] private AudioClip swingSound;
    [SerializeField] private AudioClip rollSound;

    [Header("Line Rendering")]
    [SerializeField] private List<Material> lineColors;
    private int breakpoint;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        inCutscene = true;
        isPanning = false;
        breakpoint = (int) maxForwardBackwardLaunchForce / lineColors.Count;
    }

    void Start()
    {
        //cinemachineCamera = GetComponentInChildren<CinemachineCamera>();
       // cinemachineCamera.Follow = GameObject.FindWithTag("BallFollower").transform;
        //lookController = GetComponentInChildren<CinemachineInputAxisController>();
        rb = GetComponent<Rigidbody>();
        saveLastLocation = GetComponent<SaveLastLocation>();
        lr = GetComponent<LineRenderer>();

        cameraDelta = cameraFollowTransform.eulerAngles;

        leftRightDragSensitivity /= 10f;
        forwardBackwardDragSensitivity /= 10f;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

    }


    private void FixedUpdate()
    {
        
        BallControl();
        CameraRotation();
        //Line Rendering
        if(linerendering)
        {
            lr.positionCount = 2;
            lr.SetPosition(0, transform.position);
            Vector3 temp = deltaVector;
            //temp /= 100f;
            temp.y *= forwardBackwardDragSensitivity;
            var camRot = Camera.main.transform.rotation;
            camRot.z = 0;
            Vector3 force = camRot * ConstrainForce(temp);
            
            force.y=0;
            lr.SetPosition(1, transform.position+force);
            //Line Color
            if (force.magnitude >= breakpoint)
            {
                int colIndex = Mathf.Min(Mathf.FloorToInt( force.magnitude / breakpoint), lineColors.Count - 1);
                lr.material = lineColors[colIndex];
            }
            else 
            {
                lr.material = lineColors[0];
            }
        }
        else
        {
            lr.positionCount = 0;
        }
        //Debug.Log("Linear: " + rb.linearVelocity + " || Angular: " + rb.angularVelocity);
    }
    #region InputActions
    /// <summary>
    /// Registers the drag input from users
    /// </summary>
    /// <param name="context"></param>
    public void OnTapOrDragInput(InputAction.CallbackContext context) 
    {
        //Catch player when they're trying to launch without ball slowed down
        if (!canLaunchBall) 
        {
            Debug.Log("CANNOT LAUNCH BALL YET");
            return;
        }

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
            isPanning = !context.canceled;
            Cursor.visible = !isPanning;
            Cursor.lockState = isPanning ? CursorLockMode.Locked : CursorLockMode.Confined;
            
        }
        else 
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
    public void OnLastLocationInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            
            saveLastLocation.backToLastLocation();
        }
    }
    public void SkipCutscene(InputAction.CallbackContext context) 
    {
        if (context.performed && inCutscene) 
        {
            GameObject.FindGameObjectWithTag("IntroCam").GetComponent<MiscCameraControls>().AssignHardCut();
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
        delta /= 100f;
        if (isPanning)
        {
            cameraDelta -= delta;
        }
        else 
        {
            deltaVector -= delta;
        }
        
    }
    #endregion
    private void BallControl() 
    {
        canLaunchBall = rb.linearVelocity.magnitude < 0.05f && 
                        rb.linearVelocity.y == 0 && 
                        !inCutscene;
        UIManager.Instance.EnableDisableArrow(canLaunchBall);
    }
    private void CameraRotation() 
    {
        if (!isPanning)
            return;
        float xFloat = cameraDelta.y * upDownPanSensitivity;
        float yFloat = cameraDelta.x * leftRightPanSensitivity;
        xFloat = Mathf.Clamp(xFloat, -50f, 70f);
        cameraFollowTransform.rotation = Quaternion.Euler(xFloat, yFloat, cameraFollowTransform.eulerAngles.z);
    }

    private void LaunchBall() 
    {
        //TODO
        saveLastLocation.newLastLocation();
        strokes += 1;
        AudioSource.PlayClipAtPoint(swingSound, transform.position);
        AudioSource.PlayClipAtPoint(rollSound, transform.position);
        UIManager.Instance.UpdateTallyStrokes();
        linerendering = false;

        //Divide large delta and apply sensitivity
        //deltaVector /= 100f;
        deltaVector.y *= forwardBackwardDragSensitivity;
        deltaVector.x *= leftRightDragSensitivity;

        //Multiply by camera rotation
        var camRot = Camera.main.transform.rotation;
        Vector3 force = camRot * ConstrainForce(deltaVector);
        force.y = 0;
        //Apply final force
        rb.AddForce(force, ForceMode.Impulse);
        Debug.Log($" Delta Input: {deltaVector} \tFinal Force: {force}\nCamera Rotation: {camRot} ");

        //Reset delta and cursor
        deltaVector = Vector2.zero;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private Vector3 ConstrainForce(Vector2 original) 
    {
        float x = original.x < 0 ?
            Mathf.Max(original.x, -maxLeftRightLaunchForce) :
            Mathf.Min(original.x, maxLeftRightLaunchForce);
        float y = original.y < 0 ?
            Mathf.Max(original.y, -maxForwardBackwardLaunchForce) :
            Mathf.Min(original.y, maxForwardBackwardLaunchForce);
        return new Vector3(x, 0, y);
    }
    public void TogglePauseControls()
    {

        playerInput.SwitchCurrentActionMap(playerInput.currentActionMap.name == "Player" ? "UI" : "Player");
        //Debug.Log("Action Map Changed to: " + playerInput.currentActionMap);
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
