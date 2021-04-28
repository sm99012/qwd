using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public int M_HP;
    public int M_MP; //스탯 최대치

    public int HP;
    public int MP;
    public int Lv;
    public int EXP;
    public int STR;
    public int DEFENCE; //현재스탯

    public float PowerTime; //피격시무적, 무적시간
    public bool isPower; //무적판단(플레이어만)

    //public GameObject Weapon; //장비 - 무기

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
        LvUp();
        MaxHeal();
        Death();
    }

    public void LvUp()
    {
        if (EXP >= 100)
        {
            Lv++;
            M_HP += 5;
            M_MP += 5;
            STR += 1;

            HP = M_HP; //Lv업 시 체력, 마나 풀.
            MP = M_MP;
            if (Lv > 1 && (Lv - 1) % 5 == 0) //DEFENCE 는 5레벨당 1오름
            {
                DEFENCE += 1;
            }

            if (200 > EXP) //Lv 는 한번에 최대 1업까지밖에 불가능.
            {
                EXP -= 100;
            }
            if (EXP >= 200)
            {
                EXP = 99;
            }
        }
    } //레벨업함수

    public void Hit(MonsterStatus target) //target 을 때림
    {
        if (this.STR > target.DEFENCE)
        {
            int Damage = this.STR - target.DEFENCE;
            target.HP -= Damage;
        }
        if (this.STR == target.DEFENCE)
        {
            int Damage = 0;
            target.HP -= Damage;
        }
    } //방여력 + 데미지 + 장비스탯

    public bool Death()
    {
        if (HP <= 0)
        {
            Destroy(this.gameObject);
            return true;
        }
        else return false;
    } //죽음

    public void MaxHeal()
    {
        if (HP > M_HP)
        {
            HP = M_HP;
        }
        if (MP > M_MP)
        {
            MP = M_MP;
        }
    } //체력, 마나의 최대치회복

    IEnumerator ProcessHit()
    {
        isPower = true;
        yield return new WaitForSeconds(PowerTime);
        isPower = false;
    } //피격시무적

    private void OnGUI()
    {
        GUI.Box(new Rect(0 + 150 * idx, 0, 150, 20), this.gameObject.name);
        GUI.Box(new Rect(0 + 150 * idx, 20, 150, 20), "Lv/Exp : " + Lv + "/" + EXP);
        GUI.Box(new Rect(0 + 150 * idx, 40, 150, 20), "M_HP/HP : " + M_HP + "/" + HP);
        GUI.Box(new Rect(0 + 150 * idx, 60, 150, 20), "M_MP/MP : " + M_MP + "/" + MP);
        GUI.Box(new Rect(0 + 150 * idx, 80, 150, 20), "Str : " + STR);
        GUI.Box(new Rect(0 + 150 * idx, 100, 150, 20), "DEFENCE : " + DEFENCE);
    }
}