using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Bolt;

public class CBall_Net : EntityBehaviour<IBall>
{
    public Vector3 m_direction = Vector3.forward;
    public float m_speed = 0;


    float move = 0;
    float m_ballSize = 0;

    [SerializeField] bool m_randomX = false;

    Ray m_rayMove = new Ray();
    int m_layerMask = 0;

    public int m_ballIdx = 0;

    BallHit m_ballHitEvent = null;

    public override void Attached()
    {
        state.SetTransforms(state.BallTransform,this.transform);
        //state.Direction = m_direction;
    }


    void Start()
    {
        m_direction = this.transform.forward;
        if (m_randomX == true) m_direction.x = Random.Range(0f, 1f);
        m_layerMask = (1 << LayerMask.NameToLayer("Player")) + (1 << LayerMask.NameToLayer("BallHitable"));
        m_ballSize = this.transform.localScale.x * 0.75f;

        //이벤트 생성하기
        //m_ballHitEvent = BallHit.Create(GlobalTargets.Everyone);
    }

    void Update()
    {
        move = m_speed * BoltNetwork.FrameDeltaTime;
        bool hit = Physics.Raycast(this.transform.position, m_direction, out RaycastHit rayHit, move + m_ballSize, m_layerMask);


        if (hit == true)
        {
            HitFunc(rayHit.normal);

            if (rayHit.transform.tag == "ball")
            {
                rayHit.transform.GetComponent<CBall_Net>().HitFunc(-rayHit.normal);
            }
            this.transform.position += m_direction * move;
        }
        else
        {
            this.transform.position += m_direction * move;
        }
    }

    public void HitFunc(Vector3 normal)
    {
        //반사벡터
        m_direction = Vector3.Reflect(m_direction, normal);
        m_direction.y = 0; //공중으로 못뜨게
        //state.Direction = m_direction;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ball" || other.tag == "Player")
        {//공끼리 부딪히면

            //가까운 포인트 찾아서 노멀 계산
            Vector3 normal = (other.transform.position - other.ClosestPoint(other.transform.position)).normalized;

            HitFunc(normal);
            //m_direction = Vector3.Reflect(m_direction, normal);
            //m_direction.y = 0; //공중으로 못뜨게
        }
    }
}
