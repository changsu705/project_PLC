using UnityEngine;

public class BombFruit : MonoBehaviour
{
    [SerializeField] private float force;
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
            Debug.Log("BOOM!");
            if (explosionFlowerPrefab != null)
            {
                GameObject explosionFlower = Instantiate(explosionFlowerPrefab, transform.position, Quaternion.identity);
                audioManager.PlaySFX("explosion");
                Destroy(explosionFlower, 0.5f);
                Destroy(gameObject);
            }
        }
    }
}
