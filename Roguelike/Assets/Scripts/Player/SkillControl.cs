using UnityEngine;

public class SkillControl : MonoBehaviour
{
    [SerializeField] private SkillContainer container;

    private void Awake()
    {
        container.SetTrigger(gameObject);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                Debug.Log(other.name);
                //enemy.OnDamage(container.ATK);
            }
            else
            {
                Debug.LogError($"{other.name} have 'Enemy' tag. But it don't have Enemy Component");
            }
        }
    }
}
