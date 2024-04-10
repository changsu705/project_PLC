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
                target = enemy;
            }
        }

        if (target != null)
        {
            player.StartCoroutine(CoolDown());

            Vector3 playerPos = player.transform.position;
            Vector3 targetPos = target.transform.position;
            playerPos.y = targetPos.y = 0f;

            Quaternion look = Quaternion.LookRotation(targetPos - playerPos);
            SkillEffects.Instance.PlayEffect(fx, player.transform.position, look);

            player.transform.position = Vector3.Lerp(player.transform.position, target.transform.position, 0.8f);
            trigger.transform.SetPositionAndRotation(player.transform.position, player.transform.rotation);

            trigger.SetActive(true);

            switch (Mode)
            {
                case DisableMode.Blink:
                    yield return new WaitForSeconds(0.1f);
                    break;

                case DisableMode.LifeTime:
                case DisableMode.CollisionOrLifeTime:
                    yield return new WaitForSeconds(DisableTime);
                    break;
            }

            trigger.SetActive(false);
        }
    }
}
