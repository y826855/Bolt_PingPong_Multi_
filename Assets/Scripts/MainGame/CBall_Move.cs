using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Bolt;

public class CBall_Move : EntityBehaviour<IBall>
{
    Rigidbody m_rigid = null;
    [SerializeField] Vector3 m_debugVec = Vector3.zero;
    Renderer m_renderer = null;

    private void Awake()
    {
        m_rigid = this.GetComponent<Rigidbody>();
        m_renderer = this.GetComponent<Renderer>();
    }

    public override void Attached()
    {

        state.SetTransforms(state.BallTransform, this.transform);
        if(entity.IsOwner) state.Speed = 15;
        state.AddCallback("BallColor", HitPlayer); //색깔 바뀔때 HitPlayer 콜백호출함
    }

    //충돌시 색깔 바뀜
    void HitPlayer()
    {
        m_renderer.material.SetColor("_AlbedoColor",    state.BallColor);
        m_renderer.material.SetColor("_TestEmission",   state.BallColor);
        m_renderer.material.SetFloat("_Intencity",      state.Speed);

        Debug.Log("changeMat");
    }



    private void Start()
    {
        m_rigid.velocity = state.Speed * this.transform.forward;
    }


    [SerializeField] float debugSpeed = 5f;
    void Update()
    {



    }


    private void OnCollisionEnter(Collision collision)
    {
        //if (BoltNetwork.IsServer == false) return;
        if (entity.IsOwner == false) return;

        

        //충돌시 방향 변경
        if (collision.transform.gameObject.layer == LayerMask.NameToLayer("BallHitable"))
        {
            Debug.Log("coll enter now");
            Vector3 hitVec = Vector3.Reflect(this.transform.forward, collision.GetContact(0).normal);
            hitVec.y = 0; 


            if (collision.transform.tag == "ball")
            {
                //정면으로 부딛히는지 체크
                if(this.transform.forward + hitVec.normalized != Vector3.zero)
                    hitVec = this.transform.forward + hitVec.normalized;
            }
            this.transform.forward = hitVec.normalized;

            //플레이어와 충돌 했을때
            if (collision.transform.tag == "Player")
            {
                CMoveCtrl player = collision.transform.GetComponent<CMoveCtrl>();
                if (player.m_currTeam == PlayerTeam.BLUE)
                {
                    state.BallColor = player.state.MoveColor;
                    state.Speed *= 1.1f;
                }
                else
                {
                    state.BallColor = player.state.MoveColor;
                    state.Speed *= 1.1f;
                }
                //속도에 따른 색상
                m_renderer.material.SetFloat("_Intencity", state.Speed);
            }

            m_debugVec = m_rigid.velocity = state.Speed * this.transform.forward;
        }
    }


}
