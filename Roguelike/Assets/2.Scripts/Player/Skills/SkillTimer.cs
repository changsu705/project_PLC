using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class SkillTimer : MonoBehaviour
{
    
    public GameObject[] hideSkillButtons;
    public GameObject[] textPros;
    public TextMeshProUGUI[] hideSkillTimeTexts;
    public Image[] hideSkillmages;
    private bool[] isHideSkills= {false,false,false,false,false,false,false,false,false,false};
    [FormerlySerializedAs("skiiTimes")] public float[] skillTimes = {10,10};
    private float[] getSkillTimes = {0,0,0,0,0,0,0,0,0,0};
    

    void Start()
    {
        for (int i = 0; i < textPros.Length; i++)
        {
          

            hideSkillTimeTexts[i] = textPros[i].GetComponent<TextMeshProUGUI>();
            hideSkillButtons[i].SetActive(false);
            
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)&& !isHideSkills[0])
        {
            HideSkillSetting(0);
        }
        
        if (Input.GetKeyDown(KeyCode.Space)&& !isHideSkills[1])
        {
            HideSkillSetting(1);
        }
        
        HideSkillChk();
    }


    
    public void HideSkillSetting(int skillNum)
    {
        hideSkillButtons[skillNum].SetActive(true);
        getSkillTimes[skillNum] = skillTimes[skillNum];
        isHideSkills[skillNum] = true;
    }

    private void HideSkillChk()
    {
        if (isHideSkills[0]) 
        {
            
            StartCoroutine(SkillTimeChk(0));
        }
        if(isHideSkills[1])
        {
            
            StartCoroutine(SkillTimeChk(1));
        }
        if(isHideSkills[2])
        {
            
            StartCoroutine(SkillTimeChk(2));
        }
        if(isHideSkills[3])
        {
            
            StartCoroutine(SkillTimeChk(3));
        }
        if(isHideSkills[4])
        {
            
            StartCoroutine(SkillTimeChk(4));
        }
    }

    IEnumerator SkillTimeChk(int skillNum)
    {
        yield return null;

        if(getSkillTimes[skillNum]>0)
        {
            getSkillTimes[skillNum] -= Time.deltaTime;

            if(getSkillTimes[skillNum]<0)
            {
                getSkillTimes[skillNum] = 0;
                isHideSkills[skillNum] = false;
                hideSkillButtons[skillNum].SetActive(false);

            }
            hideSkillTimeTexts[skillNum].text = getSkillTimes[skillNum].ToString("00");


            float time = getSkillTimes[skillNum] / skillTimes[skillNum];
            hideSkillmages[skillNum].fillAmount = time;
            

        }
    }
}