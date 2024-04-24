using UnityEngine;

public class SkillControl : MonoBehaviour
{
    [SerializeField] private SkillObject container;
    private new SphereCollider collider;

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

    private void Start()
    {
        collider = GetComponentInChildren<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);

        if (other.CompareTag("Enemy"))
        {
            SkillEffects.Instance.PlayEffect(SkillEffects.FX.BasicHit, other.transform.position, Quaternion.identity);
        }

        if (container.CurrentContainer.Mode == SkillContainer.DisableMode.CollisionOrLifeTime)
        {
            SkillEffects.Instance.PlayEffect(container.CurrentContainer.DestroyFx, collider.transform.position, Quaternion.identity);

            AudioManager.Instance.PlaySFX(container.CurrentContainer.EndClip);
            gameObject.SetActive(false);
        }
    }
}
