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

    
    
    public abstract void Targeting();
    


}
