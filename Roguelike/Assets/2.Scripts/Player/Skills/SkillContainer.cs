using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(SkillContainer), true)]
class SkillContainerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        var mode = (SkillContainer.DisableMode)serializedObject.FindProperty("disableMode").intValue;
        if (mode == SkillContainer.DisableMode.LifeTime)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("disableTime"));
        }
        else if (mode == SkillContainer.DisableMode.CollisionOrLifeTime)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("disableTime"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("destroyFx"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif

[CreateAssetMenu(menuName = "Skill Container/Default")]
public class SkillContainer : SkillObject
{
    [Space]
    [SerializeField] protected SkillEffects.FX fx;
    [Space]
    [SerializeField] private AudioClip startClip;
    [SerializeField] private AudioClip destroyClip;
    [Space]
    [SerializeField] private int animationKey;

    [Header("Stat")]
    [SerializeField] private int atk = 10;
    [SerializeField] private float coolTime = 1f;

    [Header("Trigger")]
    [SerializeField] private Vector3 offset;
    [SerializeField] private float radius = 0.5f;
    [SerializeField] private DisableMode disableMode;
    [HideInInspector, SerializeField] private float disableTime;
    [HideInInspector, SerializeField] private SkillEffects.FX destroyFx;

    /// <summary>
    /// 공격력
    /// </summary>
    public int ATK => atk;

    /// <summary>
    /// 쿨타임
    /// </summary>
    public float CoolTime => coolTime;

    /// <summary>
    /// 오브젝트 비활성화 방식
    /// </summary>
    public DisableMode Mode => disableMode;

    /// <summary>
    /// 활성화 시간
    /// </summary>
    public float DisableTime => disableTime;

    /// <summary>
    /// 실행시킬 애니메이션 키
    /// </summary>
    public int AnimationKey => animationKey;

    /// <summary>
    /// 스킬 시작 오디오 클립
    /// </summary>
    public AudioClip StartClip => startClip;

    /// <summary>
    /// 스킬 소멸/파괴 오디오 클립
    /// </summary>
    public AudioClip EndClip => destroyClip;

    /// <summary>
    /// 소멸 이펙트
    /// </summary>
    public SkillEffects.FX DestroyFx => destroyFx;

    public override SkillContainer CurrentContainer => this;

    public void SetTrigger(GameObject skillObject)
    {
        trigger = skillObject;

        collider = skillObject.GetComponentInChildren<SphereCollider>();
        collider.isTrigger = true;
        collider.center = offset;
        collider.radius = radius;
    }

    public override IEnumerator PlaySkill(PlayerController player)
    {
        player.StartCoroutine(CoolDown());
        trigger.transform.SetPositionAndRotation(player.transform.position, player.transform.rotation);
        SkillEffects.Instance.PlayEffect(fx, player.transform.position, player.transform.rotation);
        trigger.SetActive(true);

        switch (disableMode)
        {
            case DisableMode.Blink:
                yield return new WaitForSeconds(0.1f);
                break;

            case DisableMode.LifeTime:
            case DisableMode.CollisionOrLifeTime:
                yield return new WaitForSeconds(disableTime);
                break;
        }

        player.Attack();
        trigger.SetActive(false);
    }

    protected IEnumerator CoolDown()
    {
        attackCoolDown = false;
        yield return new WaitForSeconds(coolTime);
        attackCoolDown = true;
    }

    /// <summary>
    /// 공격 콜라이더가 특정 액션에 비활성화 되게 하는 모드
    /// </summary>
    public enum DisableMode
    {
        /// <summary>
        /// 콜라이더가 0.1초동안 나타났다 사라집나다.
        /// </summary>
        Blink,

        /// <summary>
        /// 콜라이더가 일정 시간이 지나면 사라집니다.
        /// </summary>
        LifeTime,

        /// <summary>
        /// 콜라이더가 오브젝트에 부딪히거나 일정 시간이 지나면 사라집니다.
        /// </summary>
        CollisionOrLifeTime,
    }
}