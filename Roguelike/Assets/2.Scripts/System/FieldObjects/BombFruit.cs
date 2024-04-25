using UnityEngine;

public class BombFruit : MonoBehaviour
{
    [SerializeField] private float force;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float damage;
    [SerializeField] private GameObject explosionFlowerPrefab;
    private Rigidbody rigid;
    private AudioManager audioManager;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        audioManager = AudioManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Skill"))
        {
            rigid.useGravity = true;
            rigid.isKinematic = false;
            rigid.AddForce((other.transform.up + other.transform.forward) * force, ForceMode.VelocityChange);
            rigid.AddTorque(-other.transform.right * 5f, ForceMode.VelocityChange);
        }
        else if (rigid.useGravity)
        {
            BOOM();
        }
    }

    private void BOOM()
    {
        Debug.Log("BOOM!");
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hitCollider in colliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                Debug.Log("Hit");
            }
        }

        if (explosionFlowerPrefab != null)
        {
            GameObject explosionFlower = Instantiate(explosionFlowerPrefab, transform.position, Quaternion.identity);
            audioManager.PlaySFX("explosion");
            Destroy(explosionFlower, 0.5f);
        }

        Destroy(gameObject);
    }
}
