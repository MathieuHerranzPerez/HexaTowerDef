using UnityEngine;

public class TileGroupeContainer : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField]
    private LayerMask mapLayer = default;
    [SerializeField]
    private TileGroup tileGroup = default;

    // ---- INTERN ----
    private Camera cam;
    private bool isActivated = false;
    private float yPos;
    private int nbRotationLeft = 0;

    // mouvement
    private Quaternion targetRotation;
    private Vector3 desiredPosition;
    private Vector3 moveVelocity;

    void Start()
    {
        cam = Camera.main;
        yPos = transform.position.y;
    }

    void Update()
    {
        if (isActivated)
        {
            // translation
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mapLayer))
            {
                Transform objectHit = hit.transform;

                desiredPosition = new Vector3(objectHit.position.x, yPos, objectHit.position.z);
                transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref moveVelocity, 0.08f);

                if(Input.GetButtonDown("Fire1"))
                {
                    GameObject emptyGO = new GameObject();
                    Transform newTransform = emptyGO.transform;
                    newTransform.position += new Vector3(objectHit.position.x, objectHit.position.y + 1f, objectHit.position.z);
                    bool isBuiltASuccess = tileGroup.TryToBuild(newTransform, nbRotationLeft);

                    Destroy(emptyGO);

                    Debug.Log("Success ? : " + isBuiltASuccess);
                    if(isBuiltASuccess)
                    {
                        tileGroup.NotifyBuilt();
                    }
                }
            }

            // rotation
            float rotY = targetRotation.eulerAngles.y;

            if (Input.GetButtonDown("RotateLeft"))
            {
                rotY = targetRotation.eulerAngles.y - (360 / 6);
                ++nbRotationLeft;
            }
            else if(Input.GetButtonDown("RotateRight"))
            {
                rotY = targetRotation.eulerAngles.y + (360 / 6);
                --nbRotationLeft;
            }

            targetRotation = Quaternion.Euler(transform.eulerAngles.x, rotY, transform.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    public void Activate()
    {
        isActivated = true;
    }

    public void Desactivate()
    {
        isActivated = false;
    }
}
