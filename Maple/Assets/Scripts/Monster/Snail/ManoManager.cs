using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManoManager : MonoBehaviour
{
    public Respawn ManoRespawn;

    public SpawnSnailForManoSkill S1;
    public SpawnSnailForManoSkill S2;
    public SpawnSnailForManoSkill S3;
    public SpawnSnailForManoSkill S4;
    public SpawnSnailForManoSkill S5;
    public SpawnSnailForManoSkill S6;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        GameObject obj_Mano = ManoRespawn.obj_Respawn;
        if (obj_Mano)
        {
            this.transform.position = obj_Mano.transform.position; //이 스크립트가 마노를 따라가게.
            ManoMove ManoMove = obj_Mano.GetComponent<ManoMove>();
            ManoMove.S1 = S1;
            ManoMove.S2 = S2;
            ManoMove.S3 = S3;
            ManoMove.S4 = S4;
            ManoMove.S5 = S5;
            ManoMove.S6 = S6;
        }
    }
}
