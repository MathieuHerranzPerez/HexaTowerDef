using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContainer : MonoBehaviour
{
    // touche pour effectuer les rotations
    public KeyCode inputLeftCameraRot = KeyCode.A;
    public KeyCode inputRightCameraRot = KeyCode.E;

    public float dampTime = 0.2f;
    public float dampingRotation = 5.0f;

    // vitesse de la caméra pour la translation
    public float cameraSpeed = 2.5f;
    // pour le zoom
    public float cameraMouseScroll = 50.0f;

    // position minimum de la caméra selon x et z
    [SerializeField]
    private float minPosXZ = -15f;
    // position maximum de la caméra selon x et z
    [SerializeField]
    private float maxPosXZ = 80f;
    // hauteur min et max de la caméra selon y
    [SerializeField]
    private float minHeight = 2.0f;
    [SerializeField]
    private float maxHeight = 20.0f;

    private Vector3 moveVelocity;
    public Vector3 desiredPosition;
    public Quaternion targetRotation;

    public AnimationCurve curve;

    [SerializeField]
    private Transform cameraTopPosition = default;

    // ---- INTERN ----
    private Vector3 previousPos;
    private Quaternion previousRotation;
    private bool isZoomActive = true;
    private bool isGoingToTop = false;
    private bool isGoingToPrevious = false;

    void Awake()
    {
        Init();
    }

    void Update()
    {
        if (!isGoingToTop && !isGoingToPrevious)
            desiredPosition = transform.position;
        else
        {
            if(isGoingToPrevious && Vector3.Distance(transform.position, previousPos) <= 0.5f)
            {
                isGoingToPrevious = false;
            }
            else if(isGoingToTop && Vector3.Distance(transform.position, cameraTopPosition.position) <= 0.5f)
            {
                isGoingToTop = false;
            }
        }

        // translation
        if (!isGoingToTop && !isGoingToPrevious)
        {
            if (transform.position.x >= minPosXZ && transform.position.x <= maxPosXZ && transform.position.z >= minPosXZ && transform.position.z <= maxPosXZ)
            {
                desiredPosition += transform.right * (Input.GetAxis("Horizontal") * cameraSpeed) + (new Vector3(transform.up.x, 0, transform.up.z) * (Input.GetAxis("Vertical") * cameraSpeed));
            }

            if (isZoomActive)
            {
                if (transform.position.y >= minHeight && transform.position.y <= maxHeight)
                {

                    if (transform.position.x >= minPosXZ + 1 && transform.position.x <= maxPosXZ - 1 && transform.position.z >= minPosXZ + 1 && transform.position.z <= maxPosXZ - 1)
                    {
                        desiredPosition += (transform.forward * Input.GetAxis("Mouse ScrollWheel") * cameraMouseScroll);
                    }
                    else
                    {
                        desiredPosition += (new Vector3(0, transform.forward.y, 0) * Input.GetAxis("Mouse ScrollWheel") * cameraMouseScroll);
                    }
                    // check heigh
                    if (desiredPosition.y < minHeight)
                    {
                        desiredPosition.y = minHeight;
                    }
                    else if (desiredPosition.y > maxHeight)
                    {
                        desiredPosition.y = maxHeight;
                    }
                }
            }

            if (desiredPosition.x < minPosXZ)
            {
                desiredPosition.x = minPosXZ + 0.5f; ;
            }
            if (desiredPosition.x > maxPosXZ)
            {
                desiredPosition.x = maxPosXZ - 0.5f; ;
            }
            if (desiredPosition.z < minPosXZ)
            {
                desiredPosition.z = minPosXZ + 0.5f;
            }
            if (desiredPosition.z > maxPosXZ)
            {
                desiredPosition.z = maxPosXZ - 0.5f;
            }
        }
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref moveVelocity, dampTime);


        if (!isGoingToTop && !isGoingToPrevious)
        {
            // rotation
            float rotY = targetRotation.eulerAngles.y;

            if (isZoomActive)
            {
                if (Input.GetKeyDown(inputLeftCameraRot))
                //if (Input.GetButtonDown("RotLeft"))
                {
                    rotY = targetRotation.eulerAngles.y - (360 / 6);
                }
                if (Input.GetKeyDown(inputRightCameraRot))
                //if (Input.GetButtonDown("RotRight"))
                {
                    rotY = targetRotation.eulerAngles.y + (360 / 6);
                }
            }
            targetRotation = Quaternion.Euler(transform.eulerAngles.x, rotY, transform.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * dampingRotation);
        }







        // DEBUG PART
        Debug.DrawRay(transform.position, transform.forward * 10, Color.green);
        Debug.DrawRay(transform.position, transform.right * 10, Color.red);
        Debug.DrawRay(transform.position, transform.up * 10, Color.blue);
    }

    /*
    public void ResetCameraAtPos(Vector3 pos)
    {
        transform.position = new Vector3(pos.x, transform.position.y < 7.5 ? 7.5f : transform.position.y, pos.z - 7.5f);
        desiredPosition = new Vector3(pos.x, transform.position.y < 7.5 ? 7.5f : transform.position.y, pos.z - 5f);
        transform.rotation = Quaternion.Euler(60f, 0f, 0f);
        targetRotation = Quaternion.Euler(60, 0, 0);
    }

    public void ResetCamera()
    {
        transform.position = new Vector3(0, 10, -5.0f);
        targetRotation = Quaternion.Euler(60f, 0f, 0f);
    }*/

    public void GoTopCenter()
    {
        // disable the zoom
        //isZoomActive = false;

        isGoingToTop = true;

        //Camera.main.orthographic = true;
        //Camera.main.orthographicSize = 15;

        // store the current pos and rotation
        previousPos = transform.position;
        previousRotation = transform.rotation;

        desiredPosition = cameraTopPosition.position;
        targetRotation = Quaternion.Euler(90, 0, 0);
    }

    public void ReturnToPrevious()
    {
        // enable the zoom
        //isZoomActive = true;

        isGoingToPrevious = true;

        //Camera.main.orthographic = false;

        // store the current pos and rotation
        desiredPosition = previousPos;
        targetRotation = previousRotation;
    }

    private void Init()
    {
        transform.position = new Vector3(0, 10, -5.0f);
        transform.rotation = Quaternion.Euler(60f, 0f, 0f);
        targetRotation = transform.rotation;
    }
}
