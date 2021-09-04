using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Bolt;

public class CGameEvent : GlobalEventListener
{

    //방향과 위치 지정해주기
    //공 부딛히는 이벤트
    public override void OnEvent(BallHit evnt)
    {
        Debug.LogWarning("Ball hit event gogo");
    }

    //게임 시작 이벤트
    public override void OnEvent(GameStart evnt)
    {
        if (BoltNetwork.Server == false) return;
        //공 활성화

        BoltNetwork.Instantiate(BoltPrefabs.Ball, new Vector3( 3, 1f, 0), Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.Ball, new Vector3( 0, 1f, 0), Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.Ball, new Vector3(-3, 1f, 0), Quaternion.identity);
    }
}
