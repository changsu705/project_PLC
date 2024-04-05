using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill Container/Flash")]
public class Flash : SkillContainer
{
    [Header("Flash")]
    [SerializeField] private float flashForce;

    public override IEnumerator PlaySkill(PlayerController player)
    {
        float sqrtMin = float.MaxValue;
        Enemy target = null;
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            float sqrtDistance = (enemy.transform.position - player.transform.position).sqrMagnitude;
            if (sqrtMin > sqrtDistance)
            {
                sqrtMin = sqrtDistance;
            }
        }

        if (target != null)
        {
            player.transform.position = Vector3.Lerp(player.transform.position, target.transform.position, 0.8f);
            return base.PlaySkill(player);
        }

        return null;
    }
}
