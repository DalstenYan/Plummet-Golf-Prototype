using Unity.Cinemachine;
using UnityEngine;

public class MiscCameraControls : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    CinemachineBrain brain;
    BallController playerBC;
    [SerializeField]
    CinemachineBlenderSettings settings;
    void Start()
    {
        brain = Camera.main.GetComponent<CinemachineBrain>();
        playerBC = GameObject.FindGameObjectWithTag("Player").GetComponent<BallController>();
    }

    public void AssignHardCut() 
    {
        brain.CustomBlends = settings;
        SelfDisable();
    }

    public void SelfDisable() 
    {
        playerBC.inCutscene = false;
        gameObject.SetActive(false);
    }
}
