using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq; //배열에서 각각의 배열값의 존재여부확인을위해 추가한 라이브러리.
using System;

public class GetItem : MonoBehaviour //아이템획득관련 스크립트
{
    public GUIManager GUIManager;
    Vector3 ciPos;
    Vector3 Size; //오브젝트의 크기
    public int I_Gold; //보유골드
    //string S_Gold;

    //bool isT;

    //public List<Item> Slota; //장비인벤토리
    //public List<Item> Slotb; //소비인벤토리
    //public List<GameObject> Slotc; //기타인벤토리
    public GameObject[] Slota; //장비인벤토리
    public int[] Slota_Count; //장비인벤토리 한칸당 들어가는 아이템 수.
    public GameObject[] Slotb; //소비인벤토리
    public int[] Slotb_Count; //소비인벤토리 한칸당 들어가는 아이템 수.
    public GameObject[] Slotc; //기타인벤토리
    public int[] Slotc_Count; //기타인벤토리 한칸당 들어가는 아이템 수.

    public bool isfora;
    public bool isforb;
    public bool isforc;
    void Start()
    {
        isfora = true;
        isforb = true;
        isforc = true;
    }


    void Update()
    {
        InventorySystem();
        //S_Gold = I_Gold.ToString(); //보유 골드 현황
        //GUIManager.HoldGold.text = S_Gold;
        //if (isT == true)
        //{
        //    StartCoroutine(ProcessGetItem());
        //}
        Debug.Log(isforc);
    }

    private void FixedUpdate()
    {
        GetTheItem();
    }

    public void GetTheItem()
    {
        ciPos = this.gameObject.transform.position;
        int nLayer = 1 << LayerMask.NameToLayer("Item");
        BoxCollider2D ci = this.gameObject.GetComponent<BoxCollider2D>();
        Size = ci.size;
        ciPos += new Vector3(ci.offset.x, ci.offset.y, 0);
        Collider2D c = Physics2D.OverlapBox(ciPos, ci.size, 0 , nLayer);
        if (c)
        {
            Item item = c.gameObject.GetComponent<Item>();
            if (Input.GetKey(KeyCode.Z) && item.isDrop == true)
            {
                if (c.gameObject.tag == "Coin") //Gold획득
                {
                    if (c.gameObject.name == "1Coin(Clone)")
                    {
                        I_Gold += 1;
                        Destroy(c.gameObject);
                    }
                }
                if (c.gameObject.tag == "Coin") //Gold획득
                {
                    if (c.gameObject.name == "5Coin(Clone)")
                    {
                        I_Gold += 5;
                        Destroy(c.gameObject);
                    }
                }
                if (c.gameObject.tag == "Coin") //Gold획득
                {
                    if (c.gameObject.name == "10Coin(Clone)")
                    {
                        I_Gold += 10;
                        Destroy(c.gameObject);
                    }
                }
                if (c.gameObject.tag == "Item1") //장비
                {
                    if (isfora == true)
                    {
                        for (int i = 0; i < Slota.Length; i++) //실행우선순위.1
                        {
                            if (Slota[i] != null && Slota[i].name == c.gameObject.name && Slota_Count[i] < 5)
                            {
                                Slota_Count[i]++;
                                Destroy(c.gameObject);
                                isfora = true;
                                break;
                            }
                            else isfora = false;
                        }
                    }
                    //위에for문이 실행된다면 밑의 for문은 실행안되 - bool형 추가
                    if (isfora == false)
                    {
                        for (int i = 0; i < Slota.Length; i++) //실행우선순위.2
                        {
                            if (Slota[i] == null)
                            {
                                GameObject objItema = Resources.Load("Prefabs/" + c.gameObject.name) as GameObject;
                                Slota[i] = objItema;
                                Slota_Count[i]++;
                                Destroy(c.gameObject);
                                isfora = true;
                                break;
                            }
                            if (Slota[i] == c.gameObject && Slota_Count[i] < 5)
                            {
                                Slota_Count[i]++;
                                Destroy(c.gameObject);
                                isfora = true;
                                break;
                            }
                        }
                    }
                } //장비
                if (c.gameObject.tag == "Item2") //소비
                {
                    if (isforb == true)
                    {
                        for (int i = 0; i < Slotc.Length; i++) //실행우선순위.1
                        {
                            if (Slotb[i] != null && Slotb[i].name == c.gameObject.name && Slotb_Count[i] < 5)
                            {
                                Slotb_Count[i]++;
                                Destroy(c.gameObject);
                                isforb = true;
                                break;
                            }
                            else isforb = false;
                        }
                    }
                    //위에for문이 실행된다면 밑의 for문은 실행안되 - bool형 추가
                    if (isforb == false)
                    {
                        for (int i = 0; i < Slotb.Length; i++) //실행우선순위.2
                        {
                            if (Slotb[i] == null)
                            {
                                GameObject objItemb = Resources.Load("Prefabs/" + c.gameObject.name) as GameObject;
                                Slotb[i] = objItemb;
                                Slotb_Count[i]++;
                                Destroy(c.gameObject);
                                isforb = true;
                                break;
                            }
                            if (Slotb[i] == c.gameObject && Slotb_Count[i] < 5)
                            {
                                Slotb_Count[i]++;
                                Destroy(c.gameObject);
                                isforb = true;
                                break;
                            }
                        }
                    }
                } //소비
                if (c.gameObject.tag == "Item3") //기타
                {
                    if (isforc == true)
                    {
                        for (int i = 0; i < Slotc.Length; i++) //실행우선순위.1
                        {
                            if (Slotc[i] != null && Slotc[i].name == c.gameObject.name && Slotc_Count[i] < 5)
                            {
                                Slotc_Count[i]++;
                                Destroy(c.gameObject);
                                isforc = true;
                                break;
                            }
                            else isforc = false; Debug.Log("asd");
                        }
                    }
                    //위에for문이 실행된다면 밑의 for문은 실행안되 - bool형 추가
                    if (isforc == false)
                    {
                        for (int i = 0; i < Slotc.Length; i++) //실행우선순위.2
                        {
                            if (Slotc[i] == null)
                            {
                                GameObject objItemc = Resources.Load("Prefabs/" + c.gameObject.name) as GameObject;
                                Slotc[i] = objItemc;
                                Slotc_Count[i]++;
                                Destroy(c.gameObject);
                                isforc = true;
                                break;
                            }
                            if (Slotc[i] == c.gameObject && Slotc_Count[i] < 5)
                            {
                                Slotc_Count[i]++;
                                Destroy(c.gameObject);
                                isforc = true;
                                break;
                            }
                        }
                    }

                    //if (Slotc.Contains(c.gameObject.name)) //이미 아이템을 가지고 있을때 중복해서 가질수 있는 양.
                    //{
                    //    int pos = System.Array.IndexOf(Slotc, c.gameObject.name);
                    //    if (Slotc[pos] == c.gameObject.name && Slotc_Count[pos] < 5)
                    //    {
                    //        //한칸당 최대 5개까지 보관가능.
                    //        Slotc_Count[pos]++;
                    //        Destroy(c.gameObject);
                    //        break;
                    //    }
                    //    else if (Slotc[pos] == c.gameObject.name && Slotc_Count[pos] >= 5)
                    //    {
                    //        if (Slotc[i] == "")
                    //        {
                    //            Slotc[i] = c.gameObject.name;
                    //            Slotc_Count[i]++;
                    //            Destroy(c.gameObject);
                    //            break;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    if (Slotc[i] == "") //빈 아이템슬롯에 아이템을 등록후 반복문을 마침.
                    //    {
                    //        Slotc[i] = c.gameObject.name;
                    //        Slotc_Count[i]++;
                    //        Destroy(c.gameObject);
                    //        break;
                    //    }
                    //}

                    //if (Slotc[0] == "" && a == 0)
                    //{
                    //    Slotc[0] = c.gameObject.name;
                    //    Destroy(c.gameObject);
                    //    a++;
                    //}
                    //if (Slotc[1] == "" && a == 0)
                    //{
                    //    Slotc[1] = c.gameObject.name;
                    //    Destroy(c.gameObject);
                    //    a++;
                    //}
                    //if (Slotc[2] == "" && a == 0)
                    //{
                    //    Slotc[2] = c.gameObject.name;
                    //    Destroy(c.gameObject);
                    //    a++;
                    //}
                    //if (Slotc[3] == "" && a == 0)
                    //{
                    //    Slotc[3] = c.gameObject.name;
                    //    Destroy(c.gameObject);
                    //    a++;
                    //}
                    //if (Slotc[4] == "" && a == 0)
                    //{
                    //    Slotc[4] = c.gameObject.name;
                    //    Destroy(c.gameObject);
                    //    a++;
                    //}
                    //if (Slotc[5] == "" && a == 0)
                    //{
                    //    Slotc[5] = c.gameObject.name;
                    //    Destroy(c.gameObject);
                    //    a++;
                    //}
                    //if (Slotc[6] == "" && a == 0)
                    //{
                    //    Slotc[6] = c.gameObject.name;
                    //    Destroy(c.gameObject);
                    //    a++;
                    //}
                    //if (Slotc[7] == "" && a == 0)
                    //{
                    //    Slotc[7] = c.gameObject.name;
                    //    Destroy(c.gameObject);
                    //    a++;
                    //}
                    //if (Slotc[8] == "" && a == 0)
                    //{
                    //    Slotc[8] = c.gameObject.name;
                    //    Destroy(c.gameObject);
                    //    a++;
                    //}
                    //if (Slotc[9] == "" && a == 0)
                    //{
                    //    Slotc[9] = c.gameObject.name;
                    //    Destroy(c.gameObject);
                    //    a++;
                    //}
                } //기타
            }
        }
    }

    public void InventorySystem()
    {
        for (int i = 0; i < Slotc.Length; i++)
        {
            if (Slota[i] == null)
            {
                Slota_Count[i] = 0;
            }
            if (Slotb[i] == null)
            {
                Slotb_Count[i] = 0;
            }
            if (Slotc[i] == null)
            {
                Slotc_Count[i] = 0;
            }

            if (Slota_Count[i] == 0)
            {
                Slota[i] = null;
            }
            if (Slotb_Count[i] == 0)
            {
                Slotb[i] = null;
            }
            if (Slotc_Count[i] == 0)
            {
                Slotc[i] = null;
            }
        }
    }

    //IEnumerator ProcessGetItem()
    //{
    //    isT = false;
    //    yield return new WaitForSeconds(0.1f);
    //    isT = true;
    //    a = 0;
    //}

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(ciPos, Size);
        Gizmos.color = Color.blue;
    }
}
