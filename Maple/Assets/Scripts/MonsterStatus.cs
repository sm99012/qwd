using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public float P_Back; //플레이어 넉백 값
    public float M_Back; //몬스터자체 넉백 값
    public float M_TotalBack; //몬스터자체 넉백 값 + 무기의 넉백 값
    public bool isPower; //몬스터 무적판단

    public int idx; //이 스테이터스가 무슨 오브젝트에 지정되는지.

    void Start()
    {
        isPower = false;
        HP = M_HP;
        MP = M_MP;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FixedUpdate()
    {
        P_Hitted();
        Death();
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
            PlayerSuper PlayerSuper = c.gameObject.GetComponent<PlayerSuper>();
            PlayerStatus PlayerStatus = c.gameObject.GetComponent<PlayerStatus>();
            if (c.gameObject.tag == "Player" && PlayerSuper.isPower == false)
            {
                Hit(PlayerStatus);
                //Debug.Log(s.gameObject);
                if (PlayerStatus.HP > 0) //플레이어가 죽었을때는 실행하면안되
                {
                    P_KnockBack(c.gameObject, this.gameObject);
                    PlayerSuper.P_Power();
                }
                else return;
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
        r.AddForce(vDir * P_Back * Time.deltaTime);
        Debug.Log("P_KnockBack");
    }
    public void M_KnockBack(GameObject a, GameObject b) //플레이어 넉백
    {
        Vector3 vTargetPos = a.transform.position;
        Vector3 vPos = b.transform.position;
        Vector3 vDist = vTargetPos - vPos;
        Vector3 vDir = vDist.normalized;

        Rigidbody2D r = a.GetComponent<Rigidbody2D>();
        r.AddForce(vDir * M_TotalBack * Time.deltaTime);
        Debug.Log("P_KnockBack");
    }

    public bool Death()
    {
        if (HP <= 0)
        {
            PlayerStatus p = Player.gameObject.GetComponent<PlayerStatus>();
            p.EXP += this.EXP;
            Destroy(this.gameObject);
            return true;
        }
        else return false;
    } //몬스터죽음

    public void OnGUI()
    {
        GUI.Box(new Rect(150 + 150 * idx, 0, 150, 20), this.gameObject.name);
        GUI.Box(new Rect(150 + 150 * idx, 20, 150, 20), "Lv/Exp : " + Lv + "/" + EXP);
        GUI.Box(new Rect(150 + 150 * idx, 40, 150, 20), "M_HP/HP : " + M_HP + "/" + HP);
        GUI.Box(new Rect(150 + 150 * idx, 60, 150, 20), "M_MP/MP : " + M_MP + "/" + MP);
        GUI.Box(new Rect(150 + 150 * idx, 80, 150, 20), "Str : " + STR);
        GUI.Box(new Rect(150 + 150 * idx, 100, 150, 20), "DEFENCE : " + DEFENCE);
    } //임시 GUI

    public void OnTriggerEnter2D(Collider2D c)
    {
        if (c)
        {
            MonsterSuper s = this.gameObject.GetComponent<MonsterSuper>();
            if (c.gameObject.tag == "Bullet" && s.isPower == false)
            {
                Bullet b = c.gameObject.GetComponent<Bullet>(); //피깎
                HP -= b.TotalDamage;
                M_TotalBack = M_Back + b.Back;
                s.P_Power();
                if (c != null)
                {
                    M_KnockBack(this.gameObject, c.gameObject); //몬스터 넉백
                    Destroy(c.gameObject);
                }
            }
        }
    }
}