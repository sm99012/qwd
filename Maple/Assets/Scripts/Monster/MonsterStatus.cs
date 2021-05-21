using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterStatus : MonoBehaviour
{
    public GameObject Player; //적으로 인식하는 오브젝트(플레이어)

    public int M_HP;
    public int M_MP; //스탯 최대치

    public int HP;
    public int MP;
    public int Lv;
    public int EXP; //플레이어에게 주는 EXP
    public int STR;
    public int DEFENCE; //현재스탯
    public int AttackedCount; //몬스터가 맞은 횟수

    public float P_Back; //플레이어 넉백 값
    public float M_Back; //몬스터자체 넉백 값
    public float M_TotalBack; //몬스터자체 넉백 값 + 무기의 넉백 값
    public float RememberTime; //맞았을때 맞았다고 기억하는 시간

    public bool isPower; //몬스터 무적판단
    public bool isAttacked; //몬스터가 맞았을때
    public bool isDeath;

    public Canvas Canvas;
    public Image Max_HP;
    public Image Constant_HP;
    public Vector3 FixLocation; //HP바의 위치보정

    public bool isRight;
    public bool isLeft;

    public int idx; //이 스테이터스가 무슨 오브젝트에 지정되는지.

    void Start()
    {
        isPower = false;
        HP = M_HP;
        MP = M_MP;
        isAttacked = false;
        isDeath = false;
        AttackedCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        HPBar();
        //if (Max_HP != null)
        //{
        //    Max_HP.transform.position = this.gameObject.transform.position + new Vector3(0, 0.1f, 0);
        //}
        //if (Constant_HP != null)
        //{
        //    Constant_HP.transform.position = this.gameObject.transform.position + new Vector3(0, 0.1f, 0);
        //}
        //if (isRight)
        //{
        //    RectTransform r = Max_HP.GetComponent<RectTransform>();
        //    r.localScale = new Vector3(-r.localScale.x, r.localScale.y);
        //    RectTransform rr = Constant_HP.GetComponent<RectTransform>();
        //    rr.localScale = new Vector3(-rr.localScale.x, r.localScale.y);
        //}
        //else
        //{
        //    RectTransform r = Max_HP.GetComponent<RectTransform>();
        //    r.localScale = new Vector3(r.localScale.x, r.localScale.y);
        //    RectTransform rr = Constant_HP.GetComponent<RectTransform>();
        //    rr.localScale = new Vector3(rr.localScale.x, r.localScale.y);
        //}
        RectTransform r = Canvas.GetComponent<RectTransform>();
        if (isRight == true && isLeft == false)
        {
            if (r.localScale.x > 0)
            {
                r.localScale = new Vector3(-r.localScale.x, r.localScale.y, r.localScale.z);
            }
        }
        if (isRight == false && isLeft == true) //요게 반짝반짝거리는 문제가 생긴다...
        {
            if (r.localScale.x < 0)
            {
                r.localScale = new Vector3(-r.localScale.x, r.localScale.y, r.localScale.z);
            }
        }
    }

    public void FixedUpdate()
    {
        P_Hitted();
        Death();
    }

    public void HPBar()
    {
        //if (Max_HP == null)
        //{
        //    GameObject objM_HPBar = Resources.Load("Prefabs/M_HP") as GameObject;
        //    GameObject mhp = Instantiate(objM_HPBar);
        //    Image m_hp = mhp.GetComponent<Image>();
        //    Max_HP = m_hp;
        //}
        //if (Constant_HP == null)
        //{
        //    GameObject objC_HPBar = Resources.Load("Prefabs/HP") as GameObject;
        //    GameObject chp = Instantiate(objC_HPBar);
        //    Image c_hp = chp.GetComponent<Image>();
        //    Constant_HP = c_hp;
        //}
        //Max_HP.transform.position = this.gameObject.transform.position + FixLocation;
        //Constant_HP.transform.position = this.gameObject.transform.position + FixLocation;
        RectTransform HP_RectTransform = Constant_HP.GetComponent<RectTransform>();
        RectTransform M_HP_RectTransform = Max_HP.GetComponent<RectTransform>();
        Vector2 vSize = M_HP_RectTransform.sizeDelta;
        float M_HPBarWidth = vSize.x;
        float C_HP = M_HPBarWidth * HP / M_HP;
        HP_RectTransform.sizeDelta = new Vector2(C_HP, 0.1f);
    }

    public void Hit(PlayerStatus target) //target 을 때림
    {
        if (this.STR > target.DEFENCE)
        {
            int Damage = this.STR - target.DEFENCE;
            target.HP -= Damage;
        }
        if (this.STR <= target.DEFENCE)
        {
            int Damage = 1;
            target.HP -= Damage;
        }
    } //방여력 + 데미지

    public void P_Hitted() //플레이어가 맞을때(플레이어만을위한 함수).
    {
        Vector3 vPos = this.transform.position;
        int nLayer = 1 << LayerMask.NameToLayer("Player");

        CircleCollider2D ci = this.gameObject.GetComponent<CircleCollider2D>();
        vPos += new Vector3(ci.offset.x, ci.offset.y, 0);

        Collider2D c = Physics2D.OverlapCircle(vPos, ci.radius, nLayer);

        if (c)
        {
            PlayerSuper PlayerSuper = c.gameObject.GetComponent<PlayerSuper>(); //플레이어
            PlayerStatus PlayerStatus = c.gameObject.GetComponent<PlayerStatus>();
            PlayerEquip PlayerEquip = c.gameObject.GetComponent<PlayerEquip>();
            GameObject Weapon_Gun = PlayerEquip.Weapon_Gun;
            if (Weapon_Gun)
            {
                Gun g = Weapon_Gun.GetComponent<Gun>();
                PlayerSuper WeaponSuper = g.GetComponent<PlayerSuper>(); //총
                WeaponSuper.PowerTime = PlayerSuper.PowerTime;

                if (c.gameObject.tag == "Player" && PlayerSuper.isPower == false)
                {
                    Hit(PlayerStatus);
                    //Debug.Log(s.gameObject);
                    if (PlayerStatus.HP > 0) //플레이어가 죽었을때는 실행하면안되
                    {
                        P_KnockBack(c.gameObject, this.gameObject);
                        PlayerSuper.P_Power();
                        WeaponSuper.P_Power();
                    }
                    else return;
                }
            }
            GameObject Weapon_Sword = PlayerEquip.Weapon_Sword;
            if (Weapon_Sword)
            {
                Sword g = Weapon_Sword.GetComponent<Sword>();
                PlayerSuper WeaponSuper = g.GetComponent<PlayerSuper>(); //검
                WeaponSuper.PowerTime = PlayerSuper.PowerTime;

                if (c.gameObject.tag == "Player" && PlayerSuper.isPower == false)
                {
                    Hit(PlayerStatus);
                    //Debug.Log(s.gameObject);
                    if (PlayerStatus.HP > 0) //플레이어가 죽었을때는 실행하면안되
                    {
                        P_KnockBack(c.gameObject, this.gameObject);
                        PlayerSuper.P_Power();
                        WeaponSuper.P_Power();
                    }
                    else return;
                }
            }
        }
    }


    public void P_KnockBack(GameObject a, GameObject b) //플레이어 넉백
    {
        Vector3 vTargetPos = a.transform.position;
        Vector3 vPos = b.transform.position;
        Vector3 vDist = vTargetPos - vPos;
        Vector3 vDir = vDist.normalized;

        Rigidbody2D r = a.GetComponent<Rigidbody2D>();
        r.AddForce(vDir * P_Back);
        //Debug.Log("P_KnockBack");
    }
    public void M_KnockBack(GameObject a, GameObject b) //몬스터 넉백
    {
        Vector3 vTargetPos = a.transform.position;
        Vector3 vPos = b.transform.position;
        Vector3 vDist = vTargetPos - vPos;
        Vector3 vDir = vDist.normalized;

        Rigidbody2D r = a.GetComponent<Rigidbody2D>();
        r.AddForce(vDir * M_TotalBack);
        //Debug.Log("P_KnockBack");
    }

    public bool Death()
    {
        if (HP <= 0)
        {
            PlayerStatus p = Player.gameObject.GetComponent<PlayerStatus>();
            if (isDeath == false)
            {
                p.EXP += this.EXP;
                StartCoroutine(ProcessDeath());
            }
            return true;
        }
        else return false;
    } //몬스터죽음

    public void OnTriggerEnter2D(Collider2D c)
    {
        if (c)
        {
            MonsterSuper Super = this.gameObject.GetComponent<MonsterSuper>();
            if (c.gameObject.tag == "Bullet" && Super.isPower == false)
            {
                Bullet b = c.gameObject.GetComponent<Bullet>(); //피깎
                if (b.TotalDamage > this.DEFENCE) //플레이어 총데미지가 방어력보다 높으면 
                {
                    int Damage = b.TotalDamage - this.DEFENCE;
                    this.HP -= Damage;
                }
                else if (b.TotalDamage <= this.DEFENCE)
                {
                    this.HP--;
                }
                M_TotalBack = M_Back + b.Back;
                Super.P_Power();
                if (c != null)
                {
                    M_KnockBack(this.gameObject, c.gameObject); //몬스터 넉백
                    Destroy(c.gameObject);
                    AttackedCount++;
                }
            }
            if (c.gameObject.tag == "SwordEffect" && Super.isPower == false)
            {
                SwordEffect s = c.gameObject.GetComponent<SwordEffect>();
                if (s.TotalDamage > this.DEFENCE) //플레이어 총데미지가 방어력보다 높으면 
                {
                    int Damage = s.TotalDamage - this.DEFENCE;
                    this.HP -= Damage;
                }
                else if (s.TotalDamage <= this.DEFENCE)
                {
                    this.HP--;
                }
                M_TotalBack = M_Back + s.Back;
                Super.P_Power();
                if (c != null)
                {
                    M_KnockBack(this.gameObject, c.gameObject); //몬스터 넉백
                    AttackedCount++;
                }
            }
            if (c.gameObject.tag == "SwordSkill" && Super.isPower == false)
            {
                SwordEffect s = c.gameObject.GetComponent<SwordEffect>();
                if (s.TotalDamage > this.DEFENCE) //플레이어 총데미지가 방어력보다 높으면 
                {
                    int Damage = s.TotalDamage - this.DEFENCE;
                    this.HP -= Damage;
                }
                else if (s.TotalDamage <= this.DEFENCE)
                {
                    this.HP--;
                }
                M_TotalBack = M_Back + s.Back;
                Super.P_Power();
                if (c != null)
                {
                    M_KnockBack(this.gameObject, c.gameObject); //몬스터 넉백
                    AttackedCount++;
                }
            }
        }
    }

    IEnumerator ProcessDeath()
    {
        isDeath = true;
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }
}