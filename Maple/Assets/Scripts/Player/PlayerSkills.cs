using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{

    void Start()
    {

    }


    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        PlayerMove p = this.gameObject.GetComponent<PlayerMove>();
        if (p.isLadder == false)
        {
            Gun();
        }
    }

    public void Gun()
    {
        PlayerEquip Equip = this.gameObject.GetComponent<PlayerEquip>();
        if (Equip.string_Weapon == "Weapon_Gun")
        {
            Gun gun = Equip.Weapon_Gun.GetComponent<Gun>();
            if (Input.GetKey(KeyCode.X) && gun.isReload == false)
            {
                gun.Shot();
            }
            if (Input.GetKeyDown(KeyCode.C) && gun.isReload == false)
            {
                gun.Reload();
            }
        }
    }
    //public void GunReload()
    //{
    //    PlayerEquip Equip = this.gameObject.GetComponent<PlayerEquip>();
    //    if (Equip.string_Weapon == "Weapon_Gun")
    //    {
            
    //    }
    //}
}

