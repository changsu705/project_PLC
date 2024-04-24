using UnityEngine;

public class MaterialTransition : MonoBehaviour
{
    public Material targetMaterial; // 변환될 Material
    public float transitionDuration = 2f; // 전환 지속 시간 (초)

    private Material originalMaterial; // 초기 Material
    private Renderer[] childRenderers; // 자식 오브젝트의 Renderer 컴포넌트 배열

    private void Start()
    {
        // 자식 오브젝트의 Renderer 컴포넌트 가져오기
        childRenderers = GetComponentsInChildren<Renderer>();

        // 초기 Material 저장
        originalMaterial = childRenderers[0].material;
        Invoke("StartTransition", 1.5f);
    }

    public void StartTransition()
    {
        // Coroutine 시작
        StartCoroutine(TransitionCoroutine());
    }

    private System.Collections.IEnumerator TransitionCoroutine()
    {
        float elapsedTime = 0f;

        // 보간(interpolation) 시작
        while (elapsedTime < transitionDuration)
        {
            // 현재 시간에 대한 보간값 계산
            float t = elapsedTime / transitionDuration;

            // 모든 자식 오브젝트의 Material을 보간
            foreach (Renderer renderer in childRenderers)
            {
                // 해당 이름을 가진 오브젝트에 대해서만 Material 변경
                if (renderer.gameObject.name != "Cylinder002" && renderer.gameObject.name != "FX_Portal_Blue_Out")
                {
                    renderer.material.Lerp(originalMaterial, targetMaterial, t);
                }
            }

            // 경과 시간 업데이트
            elapsedTime += Time.deltaTime;

            // 다음 프레임까지 대기
            yield return null;
        }

        // 보간이 끝난 후, 해당 오브젝트들의 Material은 변경되지 않도록 설정
        foreach (Renderer renderer in childRenderers)
        {
            if (renderer.gameObject.name != "Cylinder002" && renderer.gameObject.name != "FX_Portal_Blue_Out")
            {
                renderer.material = targetMaterial;
            }
        }
    }
}