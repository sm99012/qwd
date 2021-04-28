using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    public GameObject F_Cam; //따라갈 Cam 설정, 사라지지 않는다.
    public Respawn Player; //Cam 이 따라갈 대상선정 (Player), 초기값 설정필요

    public void FixedUpdate()
    {
        Cam c = F_Cam.gameObject.GetComponent<Cam>();
        if (c.Player == null)
        {
            c.Player = Player.obj_Respawn;
        }
    }
}
