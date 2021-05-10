using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public string RespawnObject; //리스폰할 대상의 이름, 초기값 설정필요
    public GameObject obj_Respawn; //리스폰할 대상.
    public float RespawnTime; //리스폰 대기시간
    public bool isRespawn; //true : 리스폰 가능
    public int idx; //몹의 번호지정

    void Start()
    {
        isRespawn = false;
        InitialRespawn();
    }

    public void FixedUpdate()
    {
        if (obj_Respawn)
        {
            if (obj_Respawn.tag == "Monster")
            {
                Setidx();
            }
        }
    }

    public void obj_RespawnTarget()//후기리스폰 오브젝트사망시 GUI.Death의 버튼을 눌렀을때 리스폰
    {
        if (obj_Respawn == null && isRespawn == true)
        {
            isRespawn = false;
            GameObject Prefab_obj_Respawn = Resources.Load("Prefabs/" + RespawnObject) as GameObject;
            obj_Respawn = Instantiate(Prefab_obj_Respawn);
            obj_Respawn.transform.position = this.transform.position;
            isRespawn = true;
        }
    }

    public void Setidx()
    {
        MonsterStatus m = obj_Respawn.GetComponent<MonsterStatus>();
        m.idx = this.idx;
    }

    public void InitialRespawn() //초기리스폰 시작과동시에 리스폰
    {
        GameObject Prefab_obj_Respawn = Resources.Load("Prefabs/" + RespawnObject) as GameObject;
        obj_Respawn = Instantiate(Prefab_obj_Respawn);
        obj_Respawn.transform.position = this.transform.position;
        isRespawn = true;
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, 0.1f);
    }
}
