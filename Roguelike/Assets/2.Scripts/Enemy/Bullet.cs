using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public bool isMelee;


    private void OnTriggerEnter(Collider other)
    {
        if (!isMelee && other.CompareTag("Player"))
        {
            print("초아아랄");
            Destroy(gameObject);
        }
        else if(!isMelee)
        {
            Destroy(gameObject, 3f);
        }
    }
}
