using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public PlayerRespawn PlayerRespawn;
    public PlayerStatus PlayerStatus;
    public GetItem GetItem;
    public Sprite ItemSprite; //아이템이미지
    public int ItemCode; //아이템코드
    [TextArea]
    public string ItemDescription;

    public bool isDrop;
    public bool isItema; //장비템
    public bool isItemb; //소비템
    public bool isItemc; //기타템
    void Start()
    {
        isDrop = false;
        isItema = false;
        isItemb = false;
        isItemc = false;
    }

    void Update()
    {
        PlayerStatus = PlayerRespawn.obj_Respawn.gameObject.GetComponent<PlayerStatus>();
        GetItem = PlayerRespawn.obj_Respawn.gameObject.GetComponent<GetItem>();
        if (this.gameObject.tag == "Item1")
        {
            isItema = true;
        }
        if (this.gameObject.tag == "Item2")
        {
            isItemb = true;
        }
        if (this.gameObject.tag == "Item3")
        {
            isItemc = true;
        }
    }

    private void OnCollisionStay2D(Collision2D c)
    {
        if (c.gameObject.tag == "Bottom" || c.gameObject.tag == "Bottom to Ladder")
        {
            isDrop = true;
        }
    }

    public void ItembUse() //이 아이템이 소비템일때
    {
        if (isItemb == true)
        {
            //switch (ItemCode)
            //{
            //    case 2001:
            //        {
            //            Debug.Log("체력10회복");
            //            PlayerStatus.HP += 10;
            //        }
            //        break;
            //    case 2002:
            //        {
            //            Debug.Log("마나10회복");
            //            PlayerStatus.MP += 10;
            //        }
            //        break;
            //}
        }
    }
}
