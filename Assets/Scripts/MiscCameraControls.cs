using Unity.Cinemachine;
using UnityEngine;

public class MiscCameraControls : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    CinemachineBrain brain;
    BallController playerBC;
    GameObject skipCutsceneText;
    [SerializeField]
    CinemachineBlenderSettings settings;
    void Start()
    {
        brain = Camera.main.GetComponent<CinemachineBrain>();
        playerBC = GameObject.FindGameObjectWithTag("Player").GetComponent<BallController>();
        skipCutsceneText = GameObject.Find("SkipBox");
    }

    public void AssignHardCut() 
    {
        brain.CustomBlends = settings;
        SelfDisable();
    }

    public void SelfDisable() 
    {
        skipCutsceneText.SetActive(false);
        playerBC.inCutscene = false;
        gameObject.SetActive(false);
    }
}
