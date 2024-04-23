using UnityEngine;

public class BombFruit : MonoBehaviour
{
    [SerializeField] private float force;
    private Rigidbody rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
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
        }
    }
}
