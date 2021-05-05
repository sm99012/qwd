using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquip : MonoBehaviour //플레이어의 착용장비.
{
    public GameObject Weapon_Gun; //무기 - 총
    public GameObject Bullet; //총알

    public GameObject Weapon_Knife; //무기 - 칼
                                    //무기는 하나만.
    public string string_Weapon; //착용한무기의 종류

    void Start()
    {

    }


    void Update()
    {
        if (Weapon_Gun != null && Weapon_Knife == null)
        {
            string_Weapon = "Weapon_Gun";
        }
        else if (Weapon_Gun != null && Weapon_Knife == null)
        {
            string_Weapon = "Weapon_Knife";
        }
    }

    public void FixedUpdate()
    {
        Use();
    }

    public void Use() //사용되는 Gun 오브젝트의 Gun스크립트에 User를 설정해줌.
    {
        if (string_Weapon == "Weapon_Gun")
        {
            Gun g = Weapon_Gun.GetComponent<Gun>();
            g.User = this.gameObject;
            g.Bullet = this.Bullet;
        }
    }
}