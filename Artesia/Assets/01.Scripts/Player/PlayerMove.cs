using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : IState<PlayerController>
{
    private PlayerController m_playerController;
    float speed;
    float elapsedTime;
    Vector2 m_targetPos;

    public void OperateEnter(PlayerController sender)
    {
        if (!m_playerController)
            m_playerController = sender;
        elapsedTime = 0;

        m_playerController.isMoving = true;
        speed = m_playerController.speed;
        m_targetPos = m_playerController.TargetPos;
        m_playerController.AnimationUpdate();
    }

    public void OperateUpdate(PlayerController sender)
    {
        Vector2 nowPos = m_playerController.transform.position;
        m_playerController.transform.position = Vector2.Lerp(nowPos, m_targetPos, elapsedTime / speed);

        elapsedTime += Time.deltaTime;
        if (elapsedTime >= speed)
        {
            m_playerController.transform.position = m_targetPos;
        }
    }

    public void OperateExit(PlayerController sender)
    {
        m_playerController.transform.position = m_targetPos;
        m_playerController.isMoving = false;
        m_playerController.AnimationUpdate();
        TurnManager.instance.EndPlayerTurn();
    }
}