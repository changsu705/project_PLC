using System.Collections;
using UnityEngine;

public abstract class SkillObject : ScriptableObject
{
    protected bool attackCoolDown = true;

    protected GameObject trigger;
    
    /// <summary>
    /// 현재 실행중인 스킬
    /// </summary>
    public abstract SkillContainer CurrentContainer { get; }

    /// <summary>
    /// 쿨타임 체크
    /// </summary>
    public bool AttackCoolDown => attackCoolDown;

    /// <summary>
    /// 데이터 초기화
    /// </summary>
    public abstract void Init();
    
    /// <summary>
    /// 플레이어 스킬 시작
    /// </summary>
    /// <param name="player">코루틴 돌리거나 기타 행동을 시킬 플레이어</param>
    public abstract IEnumerator PlaySkill(PlayerController player);
}