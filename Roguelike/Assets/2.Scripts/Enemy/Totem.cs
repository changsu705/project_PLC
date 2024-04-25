using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Totem : MonoBehaviour
{
    [Header("Stats")]
    public int MaxHpTetem = 3;
    public int CurHpTetem = 3;
    
    public float healInterval = 1f;
    public float healAmount = 10f;
    
    private float lastHealTime;

    private bool isDamaged;

    public GameObject BuffArea;
    
    [Space(10)]
    
    [Header("Dissolve")]
    public MeshRenderer[] meshRenderers;
    public Material dissolveMaterial;
    Dictionary<MeshRenderer,Material> originalColors = new Dictionary<MeshRenderer, Material>();


    private void Awake()
    {
        
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        dissolveMaterial = new Material(dissolveMaterial);
        
        foreach(MeshRenderer mesh in meshRenderers)
        {
            originalColors[mesh] = mesh.material;
        }
    }
    
    private void Start()
    {
        lastHealTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - lastHealTime > healInterval)
        {
            HealEnemies();
            lastHealTime = Time.time;
            
        }
    
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            UpgradeEnemies(other);
        }

        if (other.CompareTag("Skill")&& !isDamaged)
        {
            StartCoroutine(OnDamageTotem());

        }
    }
    
    

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
           DowngradeEnemies(other);
        }
    }
    

    private void HealEnemies()
    {
        
        Collider[] enemies = Physics.OverlapSphere(transform.position, 15f, LayerMask.GetMask("Enemy"));
        foreach (Collider collider in enemies)
        {
            if (collider.CompareTag("Enemy"))
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.currentHp += healAmount;
                }
                
            }
        }
    }

    private void DestroyTotem()
    {
        StartCoroutine(Dissolve());
        BuffArea.SetActive(false);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<Enemy>().isUpgraded)
            {
                enemy.GetComponentInChildren<Bullet>().damage /= 1.2f;
                enemy.GetComponent<Enemy>().isUpgraded = false;
            }
        }
        Destroy(gameObject, 1f);
    }

    private void UpgradeEnemies(Collider other)
    {
        if (other.GetComponent<Enemy>().isUpgraded != true)
        {
            other.GetComponentInChildren<Bullet>().damage *= 1.2f;
            other.GetComponent<Enemy>().isUpgraded = true;
        }
        
    }
    
    private void DowngradeEnemies(Collider other)
    {
        if (other.GetComponent<Enemy>().isUpgraded)
        {
            other.GetComponentInChildren<Bullet>().damage /= 1.2f;
            other.GetComponent<Enemy>().isUpgraded = false;
        }
        
    }

    private IEnumerator OnDamageTotem()
    {
        isDamaged = true;
        CurHpTetem--;
        foreach(MeshRenderer mesh in meshRenderers)
        {
            mesh.material.color = Color.red;
        }
        
        yield return new WaitForSeconds(0.1f);
        
        isDamaged = false;
        foreach (var pair in originalColors)
        {
            pair.Key.material = pair.Value;
        }
        
        
        if (CurHpTetem <= 0)
        {
            DestroyTotem();
        }
    }

    private IEnumerator Dissolve()
    {
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            Material[] materials = meshRenderer.materials;
        
            for(int index = 0; index < materials.Length; index++)
            {
                materials[index] = dissolveMaterial;
            }
        
            meshRenderer.materials = materials;
        }
        
        dissolveMaterial.DOFloat(1, "_Float", 2);
        
        yield return null;
    }

}


 

   
