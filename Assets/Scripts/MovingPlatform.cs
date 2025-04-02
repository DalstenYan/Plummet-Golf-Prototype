using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private List<Transform> _movePoints = new List<Transform>();
    [SerializeField] private float _movementInterval;
    [SerializeField] private float _movementSpeed;

    private int currentIndex;
    private Transform currentTarget;
    private bool isPointChanged;
    private bool isChangingPoint;

    void Awake()
    {
        if (_movePoints[0] != null)
        {
            currentTarget = _movePoints[0];
        }
    }

    private void FixedUpdate()
    {

        if (_movePoints[currentIndex] != null && isChangingPoint == false)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, currentTarget.transform.position, _movementSpeed * Time.deltaTime);
        }
        if (gameObject.transform.position == currentTarget.transform.position && isChangingPoint == false)
        {
            isChangingPoint = true;
            StartCoroutine(NewPoint());
        }
    }

    IEnumerator NewPoint()
    {
        if (currentIndex != _movePoints.Count - 1 && isPointChanged == false)
        {
            isPointChanged = true;
            yield return new WaitForSeconds(_movementInterval);
            currentIndex += 1;
            currentTarget = _movePoints[currentIndex];
            EndCoroutines(NewPoint());
        }
        else if (currentIndex == _movePoints.Count - 1 && isPointChanged == false)
        {
            isPointChanged = true;
            yield return new WaitForSeconds(_movementInterval);
            currentIndex = 0;
            currentTarget = _movePoints[currentIndex];
            EndCoroutines(NewPoint());
        }
    }

    private void EndCoroutines(IEnumerator routine)
    {
        StopCoroutine(routine);
        isPointChanged = false;
        isChangingPoint = false;
    }
}

