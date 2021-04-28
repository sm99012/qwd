using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameObject Weapon;
    public GameObject Bullet;

    public bool isGround;
    public bool isLadder;
    public bool isjump;
    public bool ispJump; //점프가 중첩되는버그를 없애기위해서
    public bool isMove; //몬스터에 데이고있는상황 false 면 이동불가
    public bool isRight;//플레이어가 오른쪽으로 이동했었음
    public bool isLeft;//플레이어가 왼쪽으로 이동했었음
    public bool isGetKeyLeftAlt;

    public float Speed;
    public float JumpPower;
    public int JumpCount;//인스펙터창에서 설정가능

    public int JumpCountProgram;//Script에서만 설정가능 (실 기능)
    void Start()
    {
        isGround = true;
        isLadder = false;
        isjump = false;
        ispJump = false;
        isMove = true;
        isGetKeyLeftAlt = false;

        isRight = true;
        isLeft = false;
    }

    void Update()
    {

    }

    public void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftAlt) == true) //n단점프때문에 추가
        {
            isGetKeyLeftAlt = true;
        }
        else
        {
            isGetKeyLeftAlt = false;
        }
        if (isLadder == false && isMove == true)//평시
        {
            if (Input.GetKey(KeyCode.RightArrow)) //오른쪽이동
            {
                transform.position += Vector3.right * Speed * Time.deltaTime;
                isRight = true;
                isLeft = false;
            }

            if (Input.GetKey(KeyCode.LeftArrow)) //왼쪽이동
            {
                transform.position += Vector3.left * Speed * Time.deltaTime;
                isRight = false;
                isLeft = true;
            }

            if (JumpCountProgram > 0 && isGround == true && ispJump == false)//점프키 꾹누를때 점프 계속되는기능
            {
                if (Input.GetKey(KeyCode.LeftAlt))
                {
                    StartCoroutine(ProcessJump());
                }
            }
            if (JumpCountProgram > 0) //n단점프
            {
                if (Input.GetKeyDown(KeyCode.LeftAlt) && isGround == false)
                {
                    Debug.Log("n단점프");
                    if (isRight == true && isLeft == false)
                    {
                        Vector3 rPos = Vector3.right;//플레이어 진행방향.
                        JumpCountProgram--;
                        Rigidbody2D r = GetComponent<Rigidbody2D>();
                        r.AddForce(rPos * JumpPower * 25f * Time.deltaTime);
                        r.AddForce(Vector3.up * JumpPower * 15f * Time.deltaTime);
                    }
                    if (isRight == false && isLeft == true)
                    {
                        Vector3 rPos = Vector3.left;//플레이어 진행방향.
                        JumpCountProgram--;
                        Rigidbody2D r = GetComponent<Rigidbody2D>();
                        r.AddForce(rPos * JumpPower * 25f * Time.deltaTime);
                        r.AddForce(Vector3.up * JumpPower * 15f * Time.deltaTime);
                    }
                }
            }
        }//평시무빙

        if (isLadder == true && isMove == true)//사다리에있을시
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += Vector3.right * Time.deltaTime;
                //Debug.Log("Move Right");
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position += Vector3.left * Time.deltaTime;
                //Debug.Log("Move Left");
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.position += Vector3.up * Time.deltaTime;
                //Debug.Log("Move Up");
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.position += Vector3.down * Time.deltaTime;
                //Debug.Log("Move Down");
            }
            if (JumpCountProgram > 0) //사다리에서의 점프
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    GetComponent<Rigidbody2D>().gravityScale = 1;
                    Rigidbody2D r = GetComponent<Rigidbody2D>();
                    r.AddForce(Vector3.up * JumpPower * Time.deltaTime);
                    //Debug.Log("JumpSucess");
                    JumpCountProgram--;

                }
            }
        }//사다리무빙
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "Bottom")//땅에데였을때
        {
            isGround = true;
            isjump = false;
            JumpCountProgram = JumpCount; // JumpCountProgram 에 JumpCount 를 되돌려줘라.
            //Debug.Log(c.gameObject + "Enter Bottom");
        }
    }

    private void OnCollisionStay2D(Collision2D c)
    {
        if (c.gameObject.tag == "Bottom")
        {
            isGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D c)
    {
        if (c.gameObject.tag == "Bottom")
        {
            isGround = false;
            //Debug.Log(c.gameObject + "Exit Bottom");
        }
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Ladder")//사다리무빙
        {
            isGround = false;
            isLadder = true;
            isjump = false;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //Debug.Log(c.gameObject + "Enter Ladder");
        }
    }

    private void OnTriggerExit2D(Collider2D c)
    {
        if (c.gameObject.tag == "Ladder")
        {
            isGround = true;
            isLadder = false;
            isjump = true;
            GetComponent<Rigidbody2D>().gravityScale = 1;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //Debug.Log(c.gameObject + "Exit Ladder");
        }
    }

    IEnumerator ProcessJump()
    {
        ispJump = true;
        JumpCountProgram--;
        Rigidbody2D r = GetComponent<Rigidbody2D>();
        r.AddForce(Vector3.up * JumpPower * 40f * Time.deltaTime);
        yield return new WaitForSeconds(0.3f);
        ispJump = false;
    }

    IEnumerator ProcessNJump(Vector3 rPos)
    {
        JumpCountProgram--;
        Rigidbody2D r = GetComponent<Rigidbody2D>();
        r.AddForce(rPos * JumpPower * 25f * Time.deltaTime);
        r.AddForce(Vector3.up * JumpPower * 25f * Time.deltaTime);
        yield return new WaitForSeconds(0.0000001f);
    }
}