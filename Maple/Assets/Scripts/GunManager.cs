using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public Respawn PlayerRespawn;
    public GameObject Weapon_Gun;

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
                    GameObject objWeapon = Equip.Weapon_Gun;
                    Gun g = objWeapon.gameObject.GetComponent<Gun>();
                    objWeapon.SetActive(true); //착용무기만 보이도록 하기.
                                               //문제는 착용하지않는 무기는 보이면안된다. - Gun 스크립트에서 해결.
                    this.Weapon_Gun = g.gameObject;
                    break;
            }
        }

    }
}
