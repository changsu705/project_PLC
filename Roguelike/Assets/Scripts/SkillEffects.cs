using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffects : MonoBehaviour
{
    public static SkillEffects Instance { get; private set; }

    [SerializeField] private Transform[] effectsObjects;

    private readonly Dictionary<FX, (Transform, ParticleSystem)> effects = new Dictionary<FX, (Transform, ParticleSystem)>();

    private void Awake()
    {
        Instance = this;

        int effectsCnt = effectsObjects.Length;
        for (int i = 0; i < effectsCnt; i++)
        {
            effects.Add((FX)i, (effectsObjects[i], effectsObjects[i].GetComponentInChildren<ParticleSystem>()));
        }
    }

    public void PlayEffect(FX fx, Vector3 position, Quaternion rotation)
    {
        (Transform tr, ParticleSystem effect) = effects[fx];
        tr.SetPositionAndRotation(position, rotation);
        effect.Play(true);
    }

    public enum FX
    {
        Basic,
    }
}
