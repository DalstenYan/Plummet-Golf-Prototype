using UnityEngine;

public class SaveLastLocation : MonoBehaviour
{
    private Vector3 lastLocation;
    private bool returned = true;
    private BallController ballController;

    private void Start()
    {
        ballController = GetComponent<BallController>();
    }

    public void newLastLocation()
    {
        lastLocation = this.gameObject.transform.position;
        returned = false;
    }
    public void backToLastLocation()
    {
        if (returned == false)
        {
            this.transform.position = lastLocation;
            ballController.ResetForce();
            returned = true;
        }
    }
}
