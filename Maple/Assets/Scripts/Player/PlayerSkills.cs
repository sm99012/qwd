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
        //EquipManager();
    }

    private void FixedUpdate()
    {
        PlayerMove p = this.gameObject.GetComponent<PlayerMove>();
        if (p.isLadder == false)
        {
            Attack();
        }
    }

    public void Attack()
    {
        PlayerEquip Equip = this.gameObject.GetComponent<PlayerEquip>();
        PlayerStatus Status = this.gameObject.GetComponent<PlayerStatus>();
        if (Equip.string_Weapon == "Weapon_Gun" && Equip.Weapon_Gun != null)
        {
            Gun gun = Equip.Weapon_Gun.GetComponent<Gun>();
            if (Input.GetKey(KeyCode.X) && gun.isReload == false)
            {
                gun.Shot();
            }
            if (Input.GetKey(KeyCode.C) && gun.isReload == false)
            {
                Debug.Log("Reload");
                gun.Reload();
            }
        }
        else if (Equip.string_Weapon == "Weapon_Sword" && Equip.Weapon_Sword != null)
        {
            Sword sword = Equip.Weapon_Sword.GetComponent<Sword>();
            if (Input.GetKey(KeyCode.X))
            {
                sword.Brandish();
            }
            if (Input.GetKey(KeyCode.S) && Status.MP >= 5) //스킬.1
            {
                sword.LongBrandish();
            }
            if (Input.GetKey(KeyCode.D) && Status.MP >= 1) //스킬.2
            {
                sword.ContinuingBrandish();
            }
        }
    }

    public void EquipManager()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayerEquip Equip = this.gameObject.GetComponent<PlayerEquip>();
            Equip.Weapon_Gun = null;
            Equip.Weapon_Sword = null;
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

