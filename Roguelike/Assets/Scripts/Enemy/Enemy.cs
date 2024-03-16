using UnityEngine;
using UnityEngine.AI;


public abstract class Enemy : MonoBehaviour
{
    protected Rigidbody rb;
    protected NavMeshAgent nav;
     
    public EnemyStat enemyStat;
    public Transform target;
    

    
    
    public abstract void Targeting();
    


}
