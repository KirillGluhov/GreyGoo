using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public Vector3 offset;
    public float sensitivity = 3; // ���������������� �����
    public float limit = 80; // ����������� �������� �� Y
    public float zoom = 0.25f; // ���������������� ��� ����������, ��������� �����
    public float zoomMax = 10; // ����. ����������
    public float zoomMin = 0.5f; // ���. ����������
    private float X, Y;

    void Start()
    {
        limit = Mathf.Abs(limit);
        if (limit > 90) limit = 90;
        offset = new Vector3(offset.x, offset.y, -Mathf.Abs(zoomMin));
        transform.position = target.position + offset;
    }

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0) offset.z += zoom;
        else if (Input.GetAxis("Mouse ScrollWheel") < 0) offset.z -= zoom;
        offset.z = Mathf.Clamp(offset.z, -Mathf.Abs(zoomMax), -Mathf.Abs(zoomMin));

        X = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;
        Y += Input.GetAxis("Mouse Y") * sensitivity;
        Y = Mathf.Clamp(Y, -limit, limit);

        if (Y > 0)
        {
            Y = 0;
        }

        transform.localEulerAngles = new Vector3(-Y, X, 0);
        transform.position = transform.localRotation * offset + target.position;
    }
}
