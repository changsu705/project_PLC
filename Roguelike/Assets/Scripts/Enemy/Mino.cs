using System.Collections;
using UnityEngine;

public class Mino : Enemy
{

    private void Awake()
    {
        StartCoroutine(Think());
    }
    
    private void Update()
    {
        if (isDead)
        {
            StopAllCoroutines();
            return;
        }
        
        
        
    }
    
    private IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);

        int ranAction = Random.Range(0, 2);

        switch (ranAction)
        {
            case 0:
                // 휘두르다
                StartCoroutine(Swing());
                break;
            case 1:
                // 돌진
                StartCoroutine(Charging());
                break;
            case 2:
                // 충격파
                StartCoroutine(ShockWave());
                break;
        }

    }

    private IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(Think());
    }
    
    private IEnumerator Charging()
    {
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(Think());
    }
    
    private IEnumerator ShockWave()
    {
        yield return new WaitForSeconds(0.1f);
        
        StartCoroutine(Think());
    }
    
    
    
    public override IEnumerator Attack()
    {
        yield break;
    }
}

