using System.Diagnostics;
using TMPro;
using UnityEngine;

public class BallinHole : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private GameObject winCheck;
    [SerializeField] private GameObject WinScreen;
    private Stopwatch stopwatchCounter = new Stopwatch();
    [SerializeField] private BallController ballController;
    [SerializeField] private TMP_Text strokeText;



    // Update is called once per frame
    void Update()
    {
        if (NearGoal())
        {
           stopwatchCounter.Start();
        }
        else
        {
            stopwatchCounter.Stop();
            stopwatchCounter.Reset();
        }

        if(stopwatchCounter.ElapsedMilliseconds > 2500)
        {
            strokeText.text = "    You finished the level in \r\n\t     " + ballController.strokes + " strokes!";
            WinScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private bool NearGoal()
    {
        return Physics.CheckSphere(winCheck.transform.position, .75f, _layerMask);
    }
}
