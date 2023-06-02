using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject _object;
    [SerializeField] private Vector3 _distanceFromObject;
    private void LateUpdate()
    {
        Vector3 positionToGo = _object.transform.position + _distanceFromObject;
        Vector3 smoothPosition = Vector3.Lerp(a: transform.position, b: positionToGo, t: 0.125F);
        transform.position = smoothPosition;
        transform.LookAt(_object.transform.position);
    }
}
