using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public string RespawnObject; //리스폰할 대상의 이름, 초기값 설정필요
    public GameObject obj_Respawn; //리스폰할 대상.
    public float RespawnTime; //리스폰 대기시간
    public bool isRespawn; //true : 리스폰 가능

    void Start()
    {
        isRespawn = false;
        StartCoroutine(ProcessInitialRespawn());
    }

    public void FixedUpdate()
    {
        obj_RespawnTarget();
    }

    public void obj_RespawnTarget()
    {
        if (obj_Respawn == null && isRespawn == true)
        {
            StartCoroutine(ProcessRespawn());
        }
    }

    IEnumerator ProcessInitialRespawn() //초기리스폰 시작과동시에 리스폰
    {
        yield return new WaitForSeconds(0);
        GameObject Prefab_obj_Respawn = Resources.Load("Prefabs/" + RespawnObject) as GameObject;
        obj_Respawn = Instantiate(Prefab_obj_Respawn);
        obj_Respawn.transform.position = this.transform.position;
        isRespawn = true;
    }

    IEnumerator ProcessRespawn() //후기리스폰 오브젝트사망시 RespawnTime 경과 후 리스폰
    {
        isRespawn = false;
        yield return new WaitForSeconds(RespawnTime);
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
