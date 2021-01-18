using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowlyRotate : MonoBehaviour
{
    [SerializeField] float rotSpeed = 1f;
    public bool cameraLocation = false;

    private Vector3 initialPosition;
    private Quaternion initialAngle;
    public Transform secondPosition;

    private void Start()
    {
        initialPosition = transform.position;
        initialAngle = transform.rotation;
    }

    void Update()
    {
        if (cameraLocation)
            transform.RotateAround(secondPosition.position, secondPosition.forward, rotSpeed);
        else
            transform.Rotate(new Vector3(0, 0, rotSpeed));

        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangePerspective();
        }
    }

    public void ChangePerspective()
    {
        cameraLocation = !cameraLocation;

        if (!cameraLocation)
        {
            transform.position = initialPosition;
            transform.rotation = initialAngle;
        }
        else
        {
            transform.position = new Vector3(-300f, secondPosition.position.y, initialPosition.z);
            transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            transform.Rotate(new Vector3(30f, 90f, 0));
        }
    }
}
