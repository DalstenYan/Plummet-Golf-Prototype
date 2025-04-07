using TMPro;
using UnityEngine;

public class FlagRaise : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private GameObject flagPole;
    [SerializeField] private float flagMaxHeight = 2f;
    [SerializeField] private float flagMinHeight = 1f;
    [SerializeField] private float flagXPosition;
    [SerializeField] private float flagZPosition;
    [SerializeField] private float flagMoveSpeed = 2f;

    void Start()
    {
        flagXPosition = flagPole.transform.position.x;
        flagZPosition = flagPole.transform.position.z;
    }

    void Update()
    {
        if (NearGoal())
        {
            flagPole.GetComponent<Rigidbody>().linearVelocity = new Vector3(0f, flagMoveSpeed, 0f);
            if (flagPole.transform.position.y >= flagMaxHeight)
            {
                flagPole.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                flagPole.transform.position = new Vector3(flagXPosition, flagMaxHeight, flagZPosition);
            }
        }
        else
        {
            flagPole.GetComponent<Rigidbody>().linearVelocity = new Vector3(0f, flagMoveSpeed * -1, 0f);
            if (flagPole.transform.position.y <= flagMinHeight)
            {
                flagPole.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                flagPole.transform.position = new Vector3(flagXPosition, flagMinHeight, flagZPosition);
            }
        }
    }

    private bool NearGoal()
    {
        return Physics.CheckSphere(flagPole.transform.position, 7.5f, _layerMask);
    }
}
