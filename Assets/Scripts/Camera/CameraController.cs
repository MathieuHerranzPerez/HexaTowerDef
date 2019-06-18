using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private CameraContainer cameraContainer = default;
    [SerializeField]
    private float dampingRotation = 5.0f;

    private Quaternion targetRotation;

    void Start()
    {
        transform.rotation = Quaternion.Euler(60f, 0f, 0f);
        targetRotation = transform.rotation;
    }

    void Update()
    {
        float rotX = (cameraContainer.curve.Evaluate(cameraContainer.transform.position.y) * 10f);

        targetRotation = Quaternion.Euler(rotX, transform.eulerAngles.y, transform.eulerAngles.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * dampingRotation);
    }

    public CameraContainer GetCameraContainer()
    {
        return cameraContainer;
    }

}
