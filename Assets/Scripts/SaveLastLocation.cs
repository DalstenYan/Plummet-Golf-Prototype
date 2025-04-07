using UnityEngine;

public class SaveLastLocation : MonoBehaviour
{
    private Vector3 lastLocation;
    private bool returned = true;

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
            returned = true;
        }
    }
}
