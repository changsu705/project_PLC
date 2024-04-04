using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill Container/Flash")]
public class Flash : SkillContainer
{
    [Header("Flash")]
    [SerializeField] private float flashForce;

    public override IEnumerator PlaySkill(PlayerController player)
    {
        if (player.Horizontal != 0f || player.Vertical != 0f)
        {
            player.transform.position +=
                PlayerController.quaterView *
                new Vector3(player.Horizontal, 0f, player.Vertical) *
                flashForce;

            player.StartCoroutine(base.PlaySkill(player));
        }

        yield return null;
    }
}
