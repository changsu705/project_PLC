using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill Container/Meteo")]
public class Meteo : SkillContainer
{
    [Header("Meteo")]
    [SerializeField] private Vector3 meteoOffset;

    public override IEnumerator PlaySkill(PlayerController player)
    {
        player.StartCoroutine(CoolDown());
        trigger.transform.SetPositionAndRotation(
            player.transform.position + player.transform.rotation * meteoOffset,
            player.transform.rotation);

        SkillEffects.Instance.PlayEffect(fx, player.transform.position, player.transform.rotation);
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

        player.Attack();
        trigger.SetActive(false);
    }
}
