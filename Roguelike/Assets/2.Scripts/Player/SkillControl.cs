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
                SkillEffects.Instance.PlayEffect(SkillEffects.FX.BasicHit, other.transform.position, Quaternion.identity);
                if (container.Mode == SkillContainer.DisableMode.CollisionOrLifeTime)
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
