using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRespawnManager : MonoBehaviour
{
    public string MonsterName; 
    public Respawn MonsterRespawn; //관리할 몬스터
    public Respawn PlayerRespawn; //플레이어

    public void FixedUpdate()
    {
        GameObject objP = PlayerRespawn.obj_Respawn;
        GameObject objM = MonsterRespawn.obj_Respawn;
        if (objP && objM)
        {
            MonsterStatus m = objM.gameObject.GetComponent<MonsterStatus>();
            m.Player = objP;
        }
    }
}
