using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdpKit;
using Photon.Bolt;
using Photon.Bolt.Matchmaking;
using System;
using UnityEngine.SceneManagement;



public class CMenu : GlobalEventListener
{
    [SerializeField] string m_GameSceneName = "";

    //클라 시작
    public void OnClick_Client()
    {
        BoltLauncher.StartClient();
    }
    //서버 시작
    public void OnClick_Server()
    {
        BoltLauncher.StartServer();
    }

    //씬 나가기
    public void OnClick_Exit()
    {
        BoltLauncher.Shutdown();
        SceneManager.LoadScene("TestMenu");
    }

    //볼트 진입 성공시
    public override void BoltStartDone()
    {
        Debug.LogWarning("done is first");

        if (BoltNetwork.IsServer)
        {
            //전역 고유 식별자 생성
            string matchName = Guid.NewGuid().ToString();

            //세션 생성되며 씬 이동
            BoltMatchmaking.CreateSession(
                sessionID: matchName,
                //sceneToLoad: "Game"
                sceneToLoad: m_GameSceneName
            );
        }
    }

    //세션 업데이트 //모든 세션을 리스트로 받아옴
    public override void SessionListUpdated(Map<Guid, UdpSession> sessionList)
    {
        Debug.LogWarningFormat("Session list updated: {0} total sessions", sessionList.Count);

        foreach (var session in sessionList)
        {
            UdpSession photonSession = session.Value as UdpSession;

            //udp 세션인가?
            if (photonSession.Source == UdpSessionSource.Photon)
            {
                BoltMatchmaking.JoinSession(photonSession);
                
                break;
            }
        }
    }
}