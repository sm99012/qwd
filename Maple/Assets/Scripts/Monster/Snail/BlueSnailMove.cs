using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueSnailMove : MonoBehaviour //맞으면 때리러옴
{
    public int RandomValue;
    public int RandomTime; //이동이 지속될 시간.

    public float Speed; //몬스터 속도

    public bool isMove;
    public bool isSlowMove;
    public bool isFastMove;
    public bool isRandomValue; //true 일때만 랜덤값지정가능
    public bool isRandomTime; //true 일때만 랜덤값지정가능
    public bool isRight; //오브젝트가 우측을 바라봄
    public bool isLeft; //오브젝트가 좌측을 바라봄
    public bool isAttack; //오브젝트가 선공모드

    public Vector3 vStartPos;
    public Vector3 vConstantPos;
    public Vector3 FindArea;

    public GameObject Target; //Target이 있다면 추적, 없으면 추적안함
    public float AttackTime; //공격당했을때 추적하는시간.

    public GameObject Player;

    public int HP;
    public void Start()
    {
        isMove = true;
        isSlowMove = true;
        isFastMove = false;
        isRandomValue = true;
        isRandomTime = true;
        isRight = true;
        isLeft = false;
        isAttack = false;

        vStartPos = this.gameObject.transform.position;

        MonsterStatus m = this.gameObject.GetComponent<MonsterStatus>(); //맞으면 일정시간동안 따라가는기능을
                                                                         //구현하기위해
        HP = m.HP;
        Player = m.Player;
    }

    public void Update()
    {
        vConstantPos = this.transform.position;
        Find();
        if (Target == null)
        {
            if (isRandomValue == true)
            {
                StartCoroutine(ProcessSetRandomValue());
            }
            if (isRandomTime == true)
            {
                StartCoroutine(ProcessSetRandomTime());
            }
            Patrol();
            SetDirection();
        }

        MonsterStatus m = this.gameObject.GetComponent<MonsterStatus>();
        Player = m.Player;
    }

    private void FixedUpdate()
    {

    }

    public void Patrol()
    {
        if (isMove == true)
        {
            if (RandomValue == 0) //Stop
            {
                this.transform.position += new Vector3(0, 0, 0);
            }
            if (RandomValue == 1) //우측이동
            {
                Move(Vector3.right);
                isRight = true;
                isLeft = false;
            }
            if (RandomValue == 2) //좌측이동
            {
                Move(Vector3.left);
                isRight = false;
                isLeft = true;
            }
        }
    }

    public void SetDirection()
    {
        if (isRight == true && isLeft == false)
        {
            Transform t = this.gameObject.GetComponent<Transform>();
            t.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
        }
        else if (isRight == false && isLeft == true)
        {
            Transform t = this.gameObject.GetComponent<Transform>();
            t.localScale = new Vector3(+0.5f, 0.5f, 0.5f);
        }
    } //오브젝트 방향전환

    public void Move(Vector3 vDir)
    {
        if (isSlowMove == true && isFastMove == false)
        {
            this.transform.position += vDir * Speed * 0.25f * Time.deltaTime;
            StartCoroutine(ProcessSlowMove());
        }
        else if (isSlowMove == false && isFastMove == true)
        {
            this.transform.position += vDir * Speed * Time.deltaTime;
            StartCoroutine(ProcessFastMove());
        }
    }

    public void Find() //몬스터의 탐지범위, 몬스터가 맞았을때
    {
        int nLayer = 1 << LayerMask.NameToLayer("Player");
        Collider2D c = Physics2D.OverlapBox(vConstantPos, FindArea, 0f, nLayer);
        MonsterStatus mm = this.gameObject.GetComponent<MonsterStatus>();

        if (HP != mm.HP && isAttack == false)
        {
            StartCoroutine(ProcessAttack(Player));
        }
        else if (HP != mm.HP && isAttack == true)
        {
            Vector3 vTargetPos = Player.gameObject.transform.position;
            Vector3 vDist = vTargetPos - vConstantPos;
            float vTargetPosX = vTargetPos.x;
            float vConstantPosX = vConstantPos.x;
            Vector3 vDir = vDist.normalized;
            if (vTargetPosX >= vConstantPosX) //몹 우측이동
            {
                Transform t = this.gameObject.GetComponent<Transform>();
                t.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
            }
            else
            {
                Transform t = this.gameObject.GetComponent<Transform>();
                t.localScale = new Vector3(+0.5f, 0.5f, 0.5f);
            }
            Move(vDir);
        }

        if (c && isAttack == false)
        {
            if (c.gameObject.tag == "Player")
            {
                Vector3 vTargetPos = c.gameObject.transform.position;
                Vector3 vDist = vTargetPos - vConstantPos;
                float vTargetPosX = vTargetPos.x;
                float vConstantPosX = vConstantPos.x;
                Vector3 vDir = vDist.normalized;

                Target = c.gameObject;
                if (vTargetPosX >= vConstantPosX) //몹 우측이동
                {
                    Transform t = this.gameObject.GetComponent<Transform>();
                    t.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
                }
                else
                {
                    Transform t = this.gameObject.GetComponent<Transform>();
                    t.localScale = new Vector3(+0.5f, 0.5f, 0.5f);
                }

                if (Target != null)
                {
                    Move(vDir);
                }
            }
        }
        else
        {
            if (isAttack == false)
            {
                Target = null;
            }
        }
    }

    IEnumerator ProcessSetRandomValue()
    {
        isRandomValue = false;
        yield return new WaitForSeconds(RandomTime);
        RandomValue = Random.Range(0, 3);
        isRandomValue = true;
    } //랜덤하게 Patrol 값지정
    IEnumerator ProcessSetRandomTime()
    {
        isRandomTime = false;
        yield return new WaitForSeconds(RandomTime);
        RandomTime = Random.Range(3, 6);
        isRandomTime = true;
    } //랜덤하게 Patrol 이 지속될 시간지정
    IEnumerator ProcessSlowMove()
    {
        isSlowMove = true;
        isFastMove = false;
        yield return new WaitForSeconds(1f);
        isSlowMove = false;
        isFastMove = true;
    } //기본적인 달팽이의 무빙
    IEnumerator ProcessFastMove()
    {
        isSlowMove = false;
        isFastMove = true;
        yield return new WaitForSeconds(1f);
        isSlowMove = true;
        isFastMove = false;
    } //기본적인 달팽이의 무빙

    IEnumerator ProcessAttack(GameObject obj) //플레이어 선공시 추적
    {
        Target = obj;
        isAttack = true;
        Debug.Log("선빵맞음");
        yield return new WaitForSeconds(AttackTime);
        MonsterStatus m = this.gameObject.GetComponent<MonsterStatus>();
        HP = m.HP;
        Target = null;
        isAttack = false;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(this.gameObject.transform.position, FindArea);
    }
}