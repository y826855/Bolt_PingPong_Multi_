using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.UI;
using Photon.Bolt;

public enum PlayerTeam { BLUE = 0, RED };


public class CMoveCtrl : EntityBehaviour<IMoveCtrl>
{
    [SerializeField] Text m_test = null;
    CPingPongManager m_gameMgr = null;
    Renderer m_renderer = null;

    public PlayerTeam m_currTeam = PlayerTeam.BLUE;

    //맵
    //Rect m_MapSize = Rect.zero;
    Vector2 m_xRange = Vector2.zero;

    //Onwer Start 
    private void Awake()
    {
        //게임 메니저 받기
        m_gameMgr = GameObject.FindWithTag("GameManager").GetComponent<CPingPongManager>();
        if (m_gameMgr == null) Debug.LogError("GameManager Not Founded");

        m_renderer = this.GetComponent<Renderer>();
    }

    public override void Attached()
    {

        state.SetTransforms(state.MoveCtrlTransform, this.transform);
        state.OnHitBall = OnHit; //함수 연결, Trigger
        state.AddCallback("MoveColor", ColorChanged);




        if (entity.IsOwner == false) return;




        //이동 범위 지정
        float size = this.transform.localScale.x /2f;
        m_xRange.x = m_gameMgr.m_MapSize.xMin + size;
        m_xRange.y = m_gameMgr.m_MapSize.xMax - size;

        //팀 설정
        if (BoltNetwork.IsServer == true)
        { state.MoveColor = Color.blue; m_currTeam = PlayerTeam.BLUE; }
        else
        { state.MoveColor = Color.red; m_currTeam = PlayerTeam.RED; }

        Invoke("SetPlayerIndex", 1.0f);

    }

    public void ColorChanged()
    {
        //m_renderer.material.color = state.MoveColor;
        m_renderer.material.SetColor("_AlbedoColor", state.MoveColor);
        Debug.Log("색 설정  " + state.MoveColor);
    }
    

    void OnHit()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
            
    }


    //플레이어 인덱스 설정
    void SetPlayerIndex()
    {
        m_gameMgr.OnPlayer(this.transform);
        if (m_test != null) m_test.text = m_gameMgr.m_playerIdx.ToString();
    }


    //Owner Update
    public override void SimulateOwner()
    {
        float speed = 20f;
        float direction = 0;
        //Vector3 movement = Vector3.zero;


        //이동 방향
        if (Input.GetKey(KeyCode.A)) direction -= 1;
        if (Input.GetKey(KeyCode.D)) direction += 1;

        //this.transform.right;

        //이동 코드
        if (direction != 0)
        {
            Vector3 move = this.transform.localPosition + (this.transform.right * direction * speed * BoltNetwork.FrameDeltaTime);
            //범위 체크
            if (move.x < m_xRange.x) { move.x = m_xRange.x; }
            if (move.x > m_xRange.y) { move.x = m_xRange.y; }
            
            this.transform.localPosition = move;
        }

        //공생성
        if (Input.GetKeyDown(KeyCode.I) && BoltNetwork.IsServer == true)
        {
            BoltNetwork.Instantiate(BoltPrefabs.Ball, new Vector3(0, 1f, 0), Quaternion.identity);
        }

    }//SimulateOwner
}
