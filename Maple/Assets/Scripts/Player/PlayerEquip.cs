using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquip : MonoBehaviour //플레이어의 착용장비.
{
    public GUIManager GUIManager;
    public GameObject Weapon_Gun; //무기 - 총
    public GameObject Bullet; //총알

    public GameObject Weapon_Sword; //무기 - 칼
                                    //무기는 하나만.
    public string string_Weapon; //착용한무기의 종류

    Vector3 Size;
    Vector3 ciPos;
    void Start()
    {

    }


    void Update()
    {
        if (Weapon_Gun != null)
        {
            string_Weapon = "Weapon_Gun";
            Weapon_Sword = null;
        }
        else if (Weapon_Sword != null)
        {
            string_Weapon = "Weapon_Sword";
            Weapon_Gun = null;
        }
        else if (Weapon_Gun == null && Weapon_Sword == null)
        {
            string_Weapon = "";
        }
    }

    public void FixedUpdate()
    {
        Use();
        //GetItem();
    }

    public void Use() //사용되는 Gun 오브젝트의 Gun스크립트에 User를 설정해줌.
    {
        if (string_Weapon == "Weapon_Gun" && Weapon_Gun != null)
        {
            Gun g = Weapon_Gun.GetComponent<Gun>();
            g.User = this.gameObject;
            g.Bullet = this.Bullet;
        }
        else if (string_Weapon == "Weapon_Sword" && Weapon_Sword != null)
        {
            Sword s = Weapon_Sword.GetComponent<Sword>();
            s.User = this.gameObject;

        }
    }

    //public void GetItem()
    //{
    //    ciPos = this.transform.position;
    //    int nLayer = 1 << LayerMask.NameToLayer("Item");
    //    BoxCollider2D ci = this.gameObject.GetComponent<BoxCollider2D>();
    //    Size = ci.size;
    //    ciPos += new Vector3(ci.offset.x, ci.offset.y, 0);
    //    Collider2D c = Physics2D.OverlapBox(ciPos, Size, nLayer);
    //    if (c)
    //    {
    //        if (Input.GetKeyDown(KeyCode.Z) && c.gameObject.tag == "Coin")
    //        {
    //            GUIManager.S1.sprite = coin;
    //            Destroy(c.gameObject);
    //        }
    //    }
    //}

    
}