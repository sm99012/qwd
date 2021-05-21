using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public int M_HP;
    public int M_MP; //스탯 최대치
    public int M_EXP; //경험치통 최대치

    public int HP;
    public int MP;
    public int Lv;
    public int EXP;
    public int STR; //플레이어의 공격력
    public int T_STR; //플레이어 공격력 + 무기 공격력 (+ 총알 공격력)
    public int DEFENCE; //현재스탯
    public float Speed;

    public float PowerTime; //피격시무적, 무적시간
    public bool isPower; //무적판단(플레이어만)

    //public GameObject Weapon; //장비 - 무기

    public int idx; //이 스테이터스가 무슨 오브젝트에 지정되는지.

    void Start()
    {
        isPower = false;
        HP = M_HP;
        MP = M_MP;
        //BeforeHP = M_HP;
    }

    // Update is called once per frame
    void Update()
    {
        AddDamage();
        //if (BeforeHP != HP) //맞았을때
        //{
        //    isAttacked = true;
        //    isAttacked = false;
        //    BeforeHP = HP;
        //}
        LvUp();
        Death();
        MaxHeal();
        MaxUse();
    }

    public void LvUp()
    {
        if (EXP >= M_EXP)
        {
            int ConstantEXP = EXP - M_EXP; //랩업하고 남은 EXP
            Lv++;
            M_HP += 5;
            M_MP += 5;
            STR += 1;
            M_EXP = (int)(M_EXP * 1.25f);
            HP = M_HP; //Lv업 시 체력, 마나 풀.
            MP = M_MP;

            if (M_EXP * 2 > EXP) //랩업하고 남은 EXP를 랩업하고난 뒤의 경험치통에 적용.
            {
                EXP = ConstantEXP;
            }
            else if (M_EXP * 2 <= EXP) //Lv 는 한번에 최대 1업까지밖에 불가능.
            {
                EXP = M_EXP - 1;
            }
        }
        if (Lv > 1 && (Lv - 1) % 5 == 0) //DEFENCE 는 5레벨당 1오름
        {
            DEFENCE += 1;
        }
    } //레벨업함수

    public void Hit(MonsterStatus target) //target 을 때림
    {
        if (this.STR > target.DEFENCE)
        {
            target.HP -= T_STR;
           
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
            HP = 0;
            StartCoroutine(ProcessDeath()); //GUI연동을위해 0.1초 딜레이.
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

    public void MaxUse()
    {
        if (HP < 0)
        {
            HP = 0;
        }
        if (MP < 0)
        {
            MP = 0;
        }
    } //체력, 마나 하락 최소치

    public void AddDamage()
    {
        PlayerEquip Equip = this.gameObject.GetComponent<PlayerEquip>(); //현재 착용하고있는 장비창
        switch (Equip.string_Weapon)
        {
            case "Weapon_Gun": //착용무기가 Gun 일때
                if (Equip.Weapon_Gun != null)
                {
                    GameObject objGun = Equip.Weapon_Gun;
                    Gun g = objGun.GetComponent<Gun>();
                    if (Equip.Bullet != null)
                    {
                        GameObject objBullet = Equip.Bullet;
                        Bullet b = objBullet.GetComponent<Bullet>();
                        T_STR = this.STR + g.GunDamage + b.BulletDamage;
                    }
                    else
                    {
                        T_STR = this.STR + g.GunDamage;
                    }
                }
                break;

            case "Weapon_Sword": //착용무기가 Sword 일때
                if (Equip.Weapon_Sword != null)
                {
                    GameObject objSword = Equip.Weapon_Sword;
                    Sword s = objSword.GetComponent<Sword>();
                    T_STR = this.STR + s.SwordDamage;
                }
                break;
        }
    }

    IEnumerator ProcessHit()
    {
        isPower = true;
        yield return new WaitForSeconds(PowerTime);
        isPower = false;
    } //피격시무적

    IEnumerator ProcessDeath()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }

    //private void OnGUI()
    //{
    //    GUI.Box(new Rect(0 + 150 * idx, 0, 150, 20), this.gameObject.name);
    //    GUI.Box(new Rect(0 + 150 * idx, 20, 150, 20), "Lv/Exp : " + Lv + "/" + EXP);
    //    GUI.Box(new Rect(0 + 150 * idx, 40, 150, 20), "M_HP/HP : " + M_HP + "/" + HP);
    //    GUI.Box(new Rect(0 + 150 * idx, 60, 150, 20), "M_MP/MP : " + M_MP + "/" + MP);
    //    GUI.Box(new Rect(0 + 150 * idx, 80, 150, 20), "Str : " + STR);
    //    GUI.Box(new Rect(0 + 150 * idx, 100, 150, 20), "DEFENCE : " + DEFENCE);
    //}
}