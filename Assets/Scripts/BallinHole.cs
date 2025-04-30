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
    [SerializeField] private AudioClip ballInHoleSound;
    [SerializeField] private GameObject ball;
    [SerializeField] private float size = 0.75f;


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
            AudioSource.PlayClipAtPoint(ballInHoleSound, transform.position);
            WinScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            stopwatchCounter.Stop();
            stopwatchCounter.Reset();
            GameObject.FindWithTag("Player").GetComponent<BallController>().TogglePauseControls();
            ball.SetActive(false);
        }
    }

    private bool NearGoal()
    {
        return Physics.CheckSphere(winCheck.transform.position, size, _layerMask);
    }
}
