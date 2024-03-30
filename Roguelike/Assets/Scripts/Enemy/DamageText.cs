using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private float moveSpeed;
    private float alphaSpeed;
    private float destroyTime;
    
    private TextMeshPro text;
    private Color alpha;
    public int damage; // 임시 코드 변경할 예정
    
    void Start()
    {
        Init();
    }

    private void Init()
    {
        moveSpeed=2.0f;
        alphaSpeed=2.0f;
        destroyTime=2.0f;
        
        text = GetComponent<TextMeshPro>();
        alpha = text.color;
        text.text = damage.ToString();
        
        Invoke(nameof(DestoryObject), destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0)); // 텍스트 위치
        
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed); // 텍스트 알파값
        text.color = alpha;
    }

    private void DestoryObject()
    {
        Destroy(gameObject);
    }
    
}
