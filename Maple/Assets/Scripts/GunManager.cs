using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public Respawn PlayerRespawn;

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
                    break;
            }
        }

    }
}
