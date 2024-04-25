using System.Collections;
using UnityEngine;

public class BombFruit : MonoBehaviour
{
    [SerializeField] private float force;
    [SerializeField] private float explosionRadius;
    public float damage;
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
            //StartCoroutine(nameof(Explosion));
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
                hitCollider.GetComponent<Enemy>().currentHp -= damage;
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

    IEnumerator Explosion()
    {
        RaycastHit [] rayHits =Physics.SphereCastAll(transform.position,15,Vector3.up,0f,LayerMask.GetMask("Enemy"));
        GameObject explosionFlower = Instantiate(explosionFlowerPrefab, transform.position, Quaternion.identity);
        audioManager.PlaySFX("explosion");
        
        foreach (RaycastHit hitObj in rayHits)
        {
            hitObj.transform.GetComponent<Enemy>().currentHp -= damage;
        }

        Destroy(gameObject);
        yield break;
    }
}
