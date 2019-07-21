using UnityEngine;

public class FrontShield : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField]
    private ParticleSystem absorbtionEffect = default;
    [SerializeField]
    private LayerMask bulletLayerMask = default;

    // ---- INTERN ----
    private int layer;

    void Start()
    {
        layer = Mathf.RoundToInt(Mathf.Log(bulletLayerMask.value, 2));
    }

    //void OnTriggerEnter(Collider collider)
    //{
        
    //}

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Check collision : " + collision);
        Debug.Log("collision.gameObject.layer : " + collision.gameObject.layer);
        Debug.Log("bulletLayerMask : " + layer);
        if (collision.gameObject.layer == layer)
        {
            Destroy(collision.gameObject);
            Debug.Log("Absorbe");
            // todo effect
            GameObject effectGO = (GameObject)Instantiate(absorbtionEffect.gameObject, collision.transform.position, Quaternion.Euler(transform.forward), transform);
            Destroy(effectGO, 2f);
        }
    }
}
