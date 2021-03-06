using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManosSnailRespawnManager : MonoBehaviour
{
    public string MonsterName;
    public SpawnSnailForManoSkill MonsterRespawn; //관리할 몬스터
    public PlayerRespawn PlayerRespawn; //플레이어

    public void FixedUpdate()
    {
        GameObject objP = PlayerRespawn.obj_Respawn;
        GameObject objM = MonsterRespawn.obj_Respawn;
        if (objM)
        {
            MonsterStatus m = objM.gameObject.GetComponent<MonsterStatus>();
            m.Player = objP;
        }
    }
}
