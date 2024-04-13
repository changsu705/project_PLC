using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "ComboContainer")]
public class ComboContainer : SkillObject
{
    [SerializeField] private SkillContainer[] comboSkills;
    [SerializeField] private float comboEndTime;

    private int comboIndex;

    public override SkillContainer CurrentContainer => comboSkills[comboIndex];

    public SkillContainer[] SkillContainers => comboSkills;

    public override void Init()
    {
        comboIndex = 0;
        attackCoolDown = true;
    }

    public override IEnumerator PlaySkill(PlayerController player)
    {
        attackCoolDown = false;
        
        player.StartCoroutine(comboSkills[comboIndex].PlaySkill(player));
        while (!comboSkills[comboIndex].AttackCoolDown)
        {
            yield return null;
        }
        attackCoolDown = true;

        comboIndex = (comboIndex + 1) % comboSkills.Length;

        yield return new WaitForSeconds(comboEndTime);
        if (attackCoolDown)
        {
            comboIndex = 0;
        }
    }
}