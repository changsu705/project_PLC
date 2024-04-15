using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillControl : MonoBehaviour
{
    [SerializeField] private SkillObject container;

    public SkillContainer Container => container.CurrentContainer;
    

    private void Awake()
    {
        if (container is SkillContainer skill)
        {
            skill.SetTrigger(gameObject);
        }
        else if (container is ComboContainer combo)
        {
            foreach (var item in combo.SkillContainers)
            {
                item.Init();
                item.SetTrigger(gameObject);
            }
        }
        
        container.Init();
        gameObject.SetActive(false);
    }

   

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                Debug.Log(other.name);
                SkillEffects.Instance.PlayEffect(SkillEffects.FX.BasicHit, other.transform.position, Quaternion.identity);
                if (container.CurrentContainer.Mode == SkillContainer.DisableMode.CollisionOrLifeTime)
                {
                    gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.LogError($"{other.name} have 'Enemy' tag. But it don't have Enemy Component");
            }
        }
    }
}
