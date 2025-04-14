using System.Diagnostics;
using UnityEngine;

public class BallinHole : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private GameObject winCheck;
    [SerializeField] private GameObject WinScreen;
    private Stopwatch stopwatchCounter = new Stopwatch();



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
            WinScreen.SetActive(true);
        }
    }

    private bool NearGoal()
    {
        return Physics.CheckSphere(winCheck.transform.position, .75f, _layerMask);
    }
}
