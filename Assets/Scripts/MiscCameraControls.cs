using Unity.Cinemachine;
using UnityEngine;

public class MiscCameraControls : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    CinemachineBrain brain;
    [SerializeField]
    CinemachineBlenderSettings settings;
    void Start()
    {
        brain = GetComponent<CinemachineBrain>();
    }

    public void AssignHardCut() 
    {
        brain.CustomBlends = settings;
    }
}
