using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Respawn : MonoBehaviour
{
    public string RespawnObject; //리스폰할 대상의 이름, 초기값 설정필요
    public GameObject obj_Respawn; //리스폰할 대상.
    public float RespawnTime; //리스폰 대기시간
    public bool isRespawn; //true : 리스폰 가능
    public bool isDropItem; //true : 아이템드롭
    public int idx; //몹의 번호지정

    MonsterStatus MonsterStatus;
    public int RewordGold; //몬스터처치 보상 
    public GameObject[] RewordItemc; //몬스터처치 기타아이템 보상
    int random;
    void Start()
    {
        isRespawn = false;
        isDropItem = false;
        InitialRespawn();
    }

    public void FixedUpdate()
    {
        obj_RespawnTarget();

        if (obj_Respawn)
        {
            if (obj_Respawn.tag == "Monster")
            {
                Setidx();
            }
        }
    }

    public void Update()
    {
        if (obj_Respawn)
        {
            MonsterStatus = obj_Respawn.GetComponent<MonsterStatus>();
            DropItem();
        }
    }

    public void obj_RespawnTarget()
    {
        if (obj_Respawn == null && isRespawn == true)
        {
            StartCoroutine(ProcessRespawn());
        }
    }

    public void Setidx()
    {
        MonsterStatus m = obj_Respawn.GetComponent<MonsterStatus>();
        m.idx = this.idx;
    }

    public void DropItem()
    {
        if (obj_Respawn)
        {
            if (MonsterStatus.Death() == true && isDropItem == false)
            {
                //Gold획득관련
                int R = Random.Range(1, 11); 
                if (R == 1) //드랍골드 없음.
                {
                    RewordGold = 0;
                    return;
                }
                else if (R == 2 || R == 3 || R == 4 || R == 5 || R == 6 || R == 7) //드랍골드 Lv만큼
                {
                    RewordGold = MonsterStatus.Lv;
                    
                }
                else if (R == 8 || R == 9 || R == 10) //드랍골드 Lv x 2만큼.
                {
                    RewordGold = MonsterStatus.Lv * 2;
                }

                int Count10 = RewordGold / 10; //10원짜리 동전 갯수
                int Count5 = (RewordGold - Count10 * 10) / 5;
                int Count1 = ((RewordGold - Count10 * 10) - Count5 * 5) % 5;
                Debug.Log("10원 : " + Count10);
                Debug.Log("5원 : " + Count5);
                Debug.Log("1원 : " + Count1);
                for (int i = 0; i < Count10; i++)
                {
                    GameObject objCoin10 = Resources.Load("Prefabs/10Coin") as GameObject;
                    GameObject P_objCoin10 = Instantiate(objCoin10);
                    Rigidbody2D r10 = P_objCoin10.GetComponent<Rigidbody2D>();
                    r10.AddForce(Vector3.up * 75f);
                    P_objCoin10.transform.position = obj_Respawn.gameObject.transform.position + new Vector3(0.05f * i, 0, 0);
                }
                for (int i = 0; i < Count5; i++)
                {
                    GameObject objCoin5 = Resources.Load("Prefabs/5Coin") as GameObject;
                    GameObject P_objCoin5 = Instantiate(objCoin5);
                    Rigidbody2D r5 = P_objCoin5.GetComponent<Rigidbody2D>();
                    r5.AddForce(Vector3.up * 75f);
                    P_objCoin5.transform.position = obj_Respawn.gameObject.transform.position + new Vector3(0.05f * i, 0, 0);
                }
                for (int i = 0; i < Count1 ; i++)
                {
                    GameObject objCoin1 = Resources.Load("Prefabs/1Coin") as GameObject;
                    GameObject P_objCoin1 = Instantiate(objCoin1);
                    Rigidbody2D r = P_objCoin1.GetComponent<Rigidbody2D>();
                    r.AddForce(Vector3.up * 75f);
                    P_objCoin1.transform.position = obj_Respawn.gameObject.transform.position + new Vector3(0.05f * i, 0, 0);
                }

                //Item획득관련
                for (int i = 0;  i < RewordItemc.Length; i++)
                {
                    random = Random.Range(0, 2);
                    if (random == 0)
                    {
                        GameObject objItem = Resources.Load("Prefabs/" + RewordItemc[i].name) as GameObject;
                        GameObject P_objItem = Instantiate(objItem);
                        Rigidbody2D r = P_objItem.GetComponent<Rigidbody2D>();
                        r.AddForce(Vector3.up * 75f);
                        P_objItem.transform.position = obj_Respawn.gameObject.transform.position + new Vector3(0.05f * i, 0, 0);
                    }
                }

                isDropItem = true;
            }
        }
    }

    public void InitialRespawn() //초기리스폰 시작과동시에 리스폰
    {
        GameObject Prefab_obj_Respawn = Resources.Load("Prefabs/" + RespawnObject) as GameObject;
        obj_Respawn = Instantiate(Prefab_obj_Respawn);
        obj_Respawn.transform.position = this.transform.position;
        isRespawn = true;
        isDropItem = false;
    }

    IEnumerator ProcessRespawn() //후기리스폰 오브젝트사망시 RespawnTime 경과 후 리스폰
    {
        isRespawn = false;
        yield return new WaitForSeconds(RespawnTime);
        GameObject Prefab_obj_Respawn = Resources.Load("Prefabs/" + RespawnObject) as GameObject;
        obj_Respawn = Instantiate(Prefab_obj_Respawn);
        obj_Respawn.transform.position = this.transform.position;
        isRespawn = true;
        isDropItem = false;
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, 0.1f);
    }
}
