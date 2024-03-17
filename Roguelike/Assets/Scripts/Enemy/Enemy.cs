using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(Rigidbody))]
public abstract class Enemy : MonoBehaviour
{
    protected Rigidbody rb;
    protected NavMeshAgent nav;
     
    public EnemyStat enemyStat;
    public Transform target;
    
    
    private void Awake()
    {
        rb=GetComponent<Rigidbody>();
        nav=GetComponent<NavMeshAgent>();
    }
    
    private void Update()
    {
        nav.SetDestination(target.position);
    }
    
    private void FixedUpdate()
    {
        Targeting();
    }

    public  void Targeting()
    {
        float targetRadius = 1.5f;
        float targetRange = 3f;
       
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange,
            LayerMask.GetMask("Player"));

    }
    
    
    
    


}
