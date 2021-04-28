using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject User; //Gun.gameObject 를 사용하는 주체(Player, NPC, Monster)
    public GameObject Bullet; //Gun.gameObject 가 사용할 총알
    public string Name; //이 스크립트를 필요로하는 총의 이름

    public string User_Name;
    public string Bullet_Name;

    public float ShotSpeed; //발사속도
    public int HaveBullet; //총알 보유 갯수. 0 이되면 사격불가능
    public float Count; //탄창
    public float ShotCount; //탄피 탄창의 갯수만큼되면 자동으로 재장전
    public float ShotTerm; //발사 텀
    public float ReloadTime; //장전시간
    public float Dist; //사거리

    public bool isShot; //true : 발사가능
    public bool isReload; //true : 장전중

    void Start()
    {
        User_Name = User.name; //리스폰시 Player(Clone) 라고 되기때문에 초기이름으로한다.
        Bullet_Name = Bullet.name; //총알발사할때 프리팹으로 총알을 불러오기위해
        isShot = true;
        isReload = false;
        ShotCount = 0;
    }

    
    void Update()
    {
        
    }

    public void FixedUpdate()
    {
        if (isShot == true && isReload == false)
        {
            if (Input.GetKey(KeyCode.X))
            {
                if (HaveBullet > 0)
                {
                    PlayerMove p = User.gameObject.GetComponent<PlayerMove>();
                    if (p.isRight == true && p.isLeft == false)
                    {
                        StartCoroutine(ProcessShot(Vector3.right));
                    }
                    else if (p.isRight == false && p.isLeft == true)
                    {
                        StartCoroutine(ProcessShot(Vector3.left));
                    }
                    ShotCount++;
                    HaveBullet--;
                }
                if (HaveBullet == 1)
                {
                    Debug.Log("보유한 총알이 부족합니다.");
                }
            }
        }
        if (ShotCount >= Count)
        {
            StartCoroutine(ProcessReload());
        } //자동재장전
        if (isReload == false)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                StartCoroutine(ProcessReload());
            }
        }
    }

    public void PasteBullet(Vector3 vDir) //발사할 총알 복제.
    {
        GameObject PrefabBullet = Resources.Load("Prefabs/" + Bullet_Name) as GameObject;
        GameObject FireBullet = Instantiate(PrefabBullet);
        FireBullet.transform.position = this.transform.position;
        FireBullet.transform.position += new Vector3(0, 0.01f, 0);
        Rigidbody2D r = FireBullet.GetComponent<Rigidbody2D>();
        r.AddForce(vDir * ShotSpeed * Time.deltaTime);
        Bullet b = FireBullet.gameObject.GetComponent<Bullet>();
        b.Dist = this.Dist;
    }

    IEnumerator ProcessShot(Vector3 vDir)
    {
        isShot = false;
        PasteBullet(vDir);
        Debug.Log("R.Shot");
        yield return new WaitForSeconds(ShotTerm);
        isShot = true;
    } //발사텀설정(공격속도)

    IEnumerator ProcessReload()
    {
        isReload = true;
        isShot = false;
        yield return new WaitForSeconds(ReloadTime);
        ShotCount = 0;
        isReload = false;
        isShot = true;
    }
}
