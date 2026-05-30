using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MobMove : IState<MobController>
{
    private MobController m_mobController;
    float m_speed;
    Vector2 m_targetPos;
    float elapsedTime;

    public void OperateEnter(MobController sender)
    {
        if (!m_mobController)
            m_mobController = sender;
        elapsedTime = 0;

        m_speed = m_mobController.speed;
        m_targetPos = m_mobController.TargetPos;
    }
    public void OperateUpdate(MobController sender)
    {
        Vector3 nowPos = m_mobController.transform.position;
        m_mobController.transform.position = Vector2.Lerp(nowPos, m_targetPos, elapsedTime / m_speed);
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= m_speed)
        {
            m_mobController.transform.position = m_targetPos;
        }
    }
    public void OperateExit(MobController sender)
    {
        m_mobController.transform.position = m_targetPos;
        TurnManager.instance.setTurn(sender.gameObject, true);
    }

}