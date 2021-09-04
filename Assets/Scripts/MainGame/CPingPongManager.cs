using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Bolt;

public class CPingPongManager : MonoBehaviour
{
    [SerializeField] Transform m_Player01_Loc = null;
    [SerializeField] Transform m_Player02_Loc = null;


    [SerializeField] Transform m_map = null;
    public Rect m_MapSize = Rect.zero;

    //내가 몇번째 플레이어인지
    public int m_playerIdx = 0;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.LogWarning("Scene Load Clear");

        //맵 스케일 받기
        m_MapSize.xMin = m_map.localScale.x * -5f;
        m_MapSize.xMax = m_map.localScale.x *  5f;
        m_MapSize.yMin = m_map.localScale.z * -5f;
        m_MapSize.yMax = m_map.localScale.z *  5f;
    }


    public void OnPlayer(Transform playerLoc)
    {
        //m_playerIdx = state.PlayerIdx++;

        if (m_playerIdx == 0)
        {
            m_Player01_Loc.gameObject.SetActive(true);
            playerLoc.position = m_Player01_Loc.position;
            playerLoc.rotation = Quaternion.LookRotation(m_Player01_Loc.forward, Vector3.up);
        }
        else
        {
            m_Player02_Loc.gameObject.SetActive(true);
            playerLoc.position = m_Player02_Loc.position;
            playerLoc.rotation = Quaternion.LookRotation(m_Player02_Loc.forward, Vector3.up);

            Invoke("JoinCheck", 3f);
            
        }
    }

    void JoinCheck()
    {
        GameStart startEvent = GameStart.Create(GlobalTargets.Everyone);
        startEvent.Send();
    }
}
