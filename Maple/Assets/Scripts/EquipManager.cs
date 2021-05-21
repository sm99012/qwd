using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    public PlayerRespawn PlayerRespawn;
    public GameObject Weapon;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        GameObject objPlayer = PlayerRespawn.obj_Respawn;
        if (objPlayer != null)
        {
            PlayerEquip Equip = objPlayer.GetComponent<PlayerEquip>();
            switch (Equip.string_Weapon)
            {
                case "Weapon_Gun": //착용무기가 Gun 일때
                    GameObject objGun = Equip.Weapon_Gun;
                    if (objGun)
                    {
                        Gun g = objGun.gameObject.GetComponent<Gun>();
                        objGun.SetActive(true); //착용무기만 보이도록 하기.
                                                //문제는 착용하지않는 무기는 보이면안된다. - Gun 스크립트에서 해결.
                        this.Weapon = g.gameObject;
                    }
                    break;
                case "Weapon_Sword":
                    GameObject objSword = Equip.Weapon_Sword;
                    if (objSword)
                    {
                        Sword s = objSword.gameObject.GetComponent<Sword>();
                        objSword.SetActive(true);
                        this.Weapon = s.gameObject;
                    }
                    break;
                case "":
                    this.Weapon = null;
                    break;
            }
        }

    }
}
