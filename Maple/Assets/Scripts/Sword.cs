using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public int SwordCode; //Sword 고유의 번호

    public GameObject User; //Sword.gameObject 를 사용하는 주체(Player, NPC, Monster)
    public GameObject S_Effect1; 
    public GameObject S_Effect2; //검기, 사거리,
    public GameObject S_Skill_Effect3; //스킬이펙트
    public string Name; //이 스크립트를 필요로하는 검의 이름

    public string SwordEffect_Name;

    public int SwordDamage; //검 자체 공격력.

    public float SwordSpeed; //검기발사속도
    public float SwordTerm; //공격 텀(공격속도)
    public float Dist; //사거리

    public bool isBrandish; //true : 공격가능
    public bool isLongBrandish; //true : 공격가능
    public bool isContinuingBrandish; //true : 공격가능
    public bool isUse; //플레이어가 검을 사용하지않을때와 사용할때를 구분
                       //true 일때만 사용가능.
    public Vector3 CorrectionValue; //검기가 발사되는 부분 보정
    void Start()
    {
        this.gameObject.SetActive(false);
        isBrandish = true;
        isLongBrandish = true;
        isContinuingBrandish = true;

        isUse = false;
    }

    // Update is called once per frame
    void Update()
    {
        UseState();

        if (isUse == true)
        {
            this.gameObject.SetActive(true);
        }
        if (isUse == false)
        {
            this.gameObject.SetActive(false);
        }

        if (User != null)
        {
            int R = Random.Range(1, 3);
            if (R == 1)
            {
                SwordEffect_Name = S_Effect1.name;
            }
            if (R == 2)
            {
                SwordEffect_Name = S_Effect2.name;
            }
        }
    }

    public void UseState()
    {
        PlayerEquip Equip = User.gameObject.GetComponent<PlayerEquip>();
        if (Equip.Weapon_Sword == this.gameObject)
        {
            isUse = true;
        }
        else
        {
            isUse = false;
        }
    }

    public void Brandish() //기본공격
    {
        if (isBrandish == true)
        {
            PlayerMove p = User.gameObject.GetComponent<PlayerMove>();
            if (p.isRight == true && p.isLeft == false)
            {
                StartCoroutine(ProcessR_Brandish());
            }
            if (p.isRight == false && p.isLeft == true)
            {
                StartCoroutine(ProcessL_Brandish());
            }
        }
    }
    public void LongBrandish()
    {
        if (isLongBrandish == true)
        {
            PlayerMove p = User.gameObject.GetComponent<PlayerMove>();
            if (p.isRight == true && p.isLeft == false)
            {
                StartCoroutine(ProcessR_LongBrandish());
                PlayerStatus s = User.GetComponent<PlayerStatus>();
                s.MP -= 5;
            }
            if (p.isRight == false && p.isLeft == true)
            {
                StartCoroutine(ProcessL_LongBrandish());
                PlayerStatus s = User.GetComponent<PlayerStatus>();
                s.MP -= 5;
            }
        }
    } //스킬공격1

    public void ContinuingBrandish() //스킬공격2
    { 
        if (isContinuingBrandish == true)
        {
            PlayerMove p = User.gameObject.GetComponent<PlayerMove>();
            if (p.isRight == true && p.isLeft == false)
            {
                StartCoroutine(ProcessR_ContinuingBrandish());
                PlayerStatus s = User.GetComponent<PlayerStatus>();
                s.MP -= 1;
            }
            if (p.isRight == false && p.isLeft == true)
            {
                StartCoroutine(ProcessL_ContinuingBrandish());
                PlayerStatus s = User.GetComponent<PlayerStatus>();
                s.MP -= 1;
            }
        }
    }

//-------------------------------------------------------------------------------기본공격

    public void PasteR_Bullet(Vector3 vDir) //SwordEffect 복제
    {
        GameObject PrefabSwordEffect = Resources.Load("Prefabs/" + SwordEffect_Name) as GameObject;
        GameObject FireSwordEffect = Instantiate(PrefabSwordEffect);
        FireSwordEffect.transform.position = this.transform.position;
        FireSwordEffect.transform.position += CorrectionValue;
        Rigidbody2D r = FireSwordEffect.GetComponent<Rigidbody2D>();
        r.AddForce(vDir * SwordSpeed);
        SwordEffect s = FireSwordEffect.gameObject.GetComponent<SwordEffect>();
        s.Dist = this.Dist;
        PlayerStatus p = User.GetComponent<PlayerStatus>();
        s.TotalDamage = p.T_STR;
    }
    public void PasteL_Bullet(Vector3 vDir) //SwordEffect 복제
    {
        GameObject PrefabSwordEffect = Resources.Load("Prefabs/" + SwordEffect_Name) as GameObject;
        GameObject FireSwordEffect = Instantiate(PrefabSwordEffect);
        FireSwordEffect.transform.position = this.transform.position;
        FireSwordEffect.transform.position += CorrectionValue;
        Transform t = FireSwordEffect.gameObject.GetComponent<Transform>();
        t.localScale = new Vector3(-1, 1, 1);
        Rigidbody2D r = FireSwordEffect.GetComponent<Rigidbody2D>();
        r.AddForce(vDir * SwordSpeed);
        SwordEffect s = FireSwordEffect.gameObject.GetComponent<SwordEffect>();
        s.Dist = this.Dist;
        PlayerStatus p = User.GetComponent<PlayerStatus>();
        s.TotalDamage = p.T_STR;
    }

    IEnumerator ProcessR_Brandish()
    {
        isBrandish = false;
        PasteR_Bullet(Vector3.right);
        yield return new WaitForSeconds(SwordTerm);
        isBrandish = true;
    }
    IEnumerator ProcessL_Brandish()
    {
        isBrandish = false;
        PasteL_Bullet(Vector3.left);
        yield return new WaitForSeconds(SwordTerm);
        isBrandish = true;
    }

//--------------------------------------------------------------------------------Skill.2

    IEnumerator ProcessR_LongBrandish()
    {
        GameObject PrefabSwordEffect = Resources.Load("Prefabs/" + S_Skill_Effect3.name) as GameObject;
        GameObject FireSwordEffect = Instantiate(PrefabSwordEffect);
        FireSwordEffect.transform.position = this.transform.position;
        FireSwordEffect.transform.position += CorrectionValue;
        SwordEffect s = FireSwordEffect.gameObject.GetComponent<SwordEffect>();
        PlayerStatus p = User.GetComponent<PlayerStatus>();
        FireSwordEffect.transform.position += new Vector3(0.5f, 0, 0);
        s.TotalDamage = (int)(p.T_STR * 2f);
        isLongBrandish = false;
        User.transform.position += new Vector3(0.5f, 0, 0);
        PlayerMove m = User.GetComponent<PlayerMove>();
        m.isUseSkill = true;
        yield return new WaitForSeconds(1f);
        User.transform.position += new Vector3(-0.5f, 0, 0);
        m.isUseSkill = false;
        Destroy(FireSwordEffect);
        isLongBrandish = true;
    }
    IEnumerator ProcessL_LongBrandish()
    {
        GameObject PrefabSwordEffect = Resources.Load("Prefabs/" + S_Skill_Effect3.name) as GameObject;
        GameObject FireSwordEffect = Instantiate(PrefabSwordEffect);
        FireSwordEffect.transform.position = this.transform.position;
        FireSwordEffect.transform.position += CorrectionValue;
        Transform t = FireSwordEffect.gameObject.GetComponent<Transform>();
        t.localScale = new Vector3(-2, 1, 1);
        SwordEffect s = FireSwordEffect.gameObject.GetComponent<SwordEffect>();
        PlayerStatus p = User.GetComponent<PlayerStatus>();
        FireSwordEffect.transform.position += new Vector3(-0.5f, 0, 0);
        s.TotalDamage = (int)(p.T_STR * 2f);
        isLongBrandish = false;
        User.transform.position += new Vector3(-0.5f, 0, 0);
        PlayerMove m = User.GetComponent<PlayerMove>();
        m.isUseSkill = true;
        yield return new WaitForSeconds(1f);
        User.transform.position += new Vector3(0.5f, 0, 0);
        m.isUseSkill = false;
        Destroy(FireSwordEffect);
        isLongBrandish = true;
    }

//--------------------------------------------------------------------------------Skill.2

    IEnumerator ProcessR_ContinuingBrandish()
    {
        GameObject PrefabSwordEffect = Resources.Load("Prefabs/" + SwordEffect_Name) as GameObject;
        GameObject FireSwordEffect = Instantiate(PrefabSwordEffect);
        FireSwordEffect.transform.position = this.transform.position;
        FireSwordEffect.transform.position += CorrectionValue;
        Rigidbody2D r = FireSwordEffect.GetComponent<Rigidbody2D>();
        r.AddForce(Vector3.right * SwordSpeed);
        SwordEffect s = FireSwordEffect.gameObject.GetComponent<SwordEffect>();
        s.Dist = this.Dist;
        PlayerStatus p = User.GetComponent<PlayerStatus>();
        s.TotalDamage = (int)(p.T_STR * 1.1f);
        isContinuingBrandish = false;
        yield return new WaitForSeconds(0.05f);
        isContinuingBrandish = true;
    }

    IEnumerator ProcessL_ContinuingBrandish()
    {
        GameObject PrefabSwordEffect = Resources.Load("Prefabs/" + SwordEffect_Name) as GameObject;
        GameObject FireSwordEffect = Instantiate(PrefabSwordEffect);
        FireSwordEffect.transform.position = this.transform.position;
        FireSwordEffect.transform.position += CorrectionValue;
        Transform t = FireSwordEffect.gameObject.GetComponent<Transform>();
        t.localScale = new Vector3(-1, 1, 1);
        Rigidbody2D r = FireSwordEffect.GetComponent<Rigidbody2D>();
        r.AddForce(Vector3.left * SwordSpeed);
        SwordEffect s = FireSwordEffect.gameObject.GetComponent<SwordEffect>();
        s.Dist = this.Dist;
        PlayerStatus p = User.GetComponent<PlayerStatus>();
        s.TotalDamage = (int)(p.T_STR * 1.3f);
        isContinuingBrandish = false;
        yield return new WaitForSeconds(0.05f);
        isContinuingBrandish = true;
    }
}
