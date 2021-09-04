using UnityEngine;
using System.Collections;
using Photon.Bolt;
using UdpKit;

[BoltGlobalBehaviour]
public class CNetworkCallbacks : GlobalEventListener
{
    public int m_playerIdx = 0;

    CPingPongManager m_gameMgr = null;

    bool m_isServer = false;

    //씬로딩 다됬나?
    public override void SceneLoadLocalDone(string scene, IProtocolToken token)
    {

        if (scene != "PingPongTest") return;


        Debug.LogWarning("SceneLoadLocalDone");

        // 네트워크 플레이어 생성하고 index 할당
        BoltNetwork.Instantiate(BoltPrefabs.Pref_Player, Vector3.zero, Quaternion.identity);
        //CMoveCtrl get = BoltNetwork.Instantiate(BoltPrefabs.Pref_Player, Vector3.zero, Quaternion.identity).GetComponent<CMoveCtrl>();
        
        m_gameMgr = GameObject.FindWithTag("GameManager").GetComponent<CPingPongManager>();
        if (m_gameMgr == null) Debug.LogError("GameManager Not Founded");
        if (m_isServer == true) m_gameMgr.m_playerIdx = 0;
        else                    m_gameMgr.m_playerIdx = 1;
        //
        ////시작위치 설정해주기
        //m_gameMgr.OnPlayer((int)get.m_playerIdx, get.transform);
    }

    public override void SceneLoadLocalBegin(string scene, IProtocolToken token)
    {
        Debug.LogWarning("SceneLoadLocalBegin");

    }





    //클라로 서버에 들어왔을때
    public override void SceneLoadRemoteDone(BoltConnection connection, IProtocolToken token)
    {
        m_isServer = false;
        Debug.LogWarning("remote is first");
    }

    public override void SessionConnected(UdpSession session, IProtocolToken token)
    {
        Debug.LogWarning("sessionConnected is first");
    }

    //서버가 방파고 들어왔을때
    public override void SessionCreatedOrUpdated(UdpSession session)
    {
        m_isServer = true;
        Debug.LogWarning("sessionCreated is first");
    }



    private void OnApplicationQuit()
    {
        //BoltNetwork.ShutdownImmediate();
        BoltLauncher.Shutdown();
        Debug.Log("Quit");
    }

}