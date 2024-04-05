using UnityEngine;

public class SkillControl : MonoBehaviour
{
    [SerializeField] private SkillContainer container;

    public SkillContainer Container => container;

    private void Awake()
    {
        container.SetTrigger(gameObject);
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
                if (container.Mode == SkillContainer.DisableMode.CollisionOrLifeTime)
                {
                    gameObject.SetActive(false);
                }
                //enemy.OnDamage(container.ATK);
            }
            else
            {
                Debug.LogError($"{other.name} have 'Enemy' tag. But it don't have Enemy Component");
            }
        }
    }
}
