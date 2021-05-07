using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    public Respawn PlayerRespawn;

    public bool isGameStart;

    public enum E_SCENE_STATE { START, SYSTEM, DEATH, ENDING }
    public E_SCENE_STATE m_eCurState;
    public List<GameObject> m_listScenes;

    void Start()
    {
        SetState(m_eCurState);
        StartCoroutine(ProcessStartGame());
    }

    void Update()
    {
        UpdateState();
    }

    void FixedUpdate()
    {
        
    }


    public void ShowScene(E_SCENE_STATE state)
    {
        for (int i = 0; i < m_listScenes.Count; i++)
        {
            if (i == (int)state)
            {
                m_listScenes[i].SetActive(true);
            }
            else
            {
                m_listScenes[i].SetActive(false);
            }
        }
    }

    public void SetState(E_SCENE_STATE state)
    {
        switch (state)
        {
            case E_SCENE_STATE.START:
                Time.timeScale = 0;
                break;
            case E_SCENE_STATE.SYSTEM:
                Time.timeScale = 1;
                break;
            case E_SCENE_STATE.DEATH:
                Time.timeScale = 1;
                break;
            case E_SCENE_STATE.ENDING:
                Time.timeScale = 0;
                break;
        }
        ShowScene(state);
        m_eCurState = state;
    }

    public void UpdateState()
    {
        GameObject objPlayer = PlayerRespawn.obj_Respawn;
        switch (m_eCurState)
        {
            case E_SCENE_STATE.START:
                break;
            case E_SCENE_STATE.SYSTEM:
                {
                    if (isGameStart == true)
                    {
                        if (objPlayer)
                        {
                            PlayerStatus PlayerStatus = objPlayer.GetComponent<PlayerStatus>();
                            if (PlayerStatus.HP <= 0)
                            {
                                SetState(E_SCENE_STATE.DEATH);
                            }
                        }
                    }
                    if (objPlayer)
                    {
                        PlayerMove PlayerMove = objPlayer.GetComponent<PlayerMove>();
                        if (PlayerMove.isEnding == true)
                        {
                            SetState(E_SCENE_STATE.ENDING);
                        }
                    }
                }
                break;
            case E_SCENE_STATE.DEATH:
                break;
            case E_SCENE_STATE.ENDING:
                break;
        }
    }

    //public void EventReset() //리셋기능
    //{
    //    Destroy(PlayerRespawn.obj_Respawn);
    //    Destroy...Snail
    //    Destroy...RedSnail
    //    ...
    //}

    public void EventSceneChange(int sceneNumber)
    {
        SetState((E_SCENE_STATE)sceneNumber);
    }

    IEnumerator ProcessStartGame()
    {
        yield return new WaitForSeconds(0.1f); //0.1초후 정상구동
        isGameStart = true;
    }
}
