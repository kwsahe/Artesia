using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAtk : IState<MobController>
{
    private MobController m_mobController;
    Vector2 m_Dir;
    float AtkSpeed;
    Vector3 m_OriPos;
    float elapsedTime;
    Vector2 m_targetPos;

    public void OperateEnter(MobController sender)
    {
        if (!m_mobController)
            m_mobController = sender;

        m_targetPos = sender.TargetPos;
        m_Dir = sender.Dir;
        m_OriPos = sender.transform.position;
        AtkSpeed = sender.speed / 2f;

        elapsedTime += Time.deltaTime;
        m_mobController.transform.position = Vector2.Lerp(sender.transform.position, m_targetPos, elapsedTime / AtkSpeed);
    }
    public void OperateUpdate(MobController sender)
    {
        Vector2 nowPos = m_mobController.transform.position;
        m_mobController.transform.position = Vector2.Lerp(nowPos, m_targetPos, elapsedTime / AtkSpeed);

        elapsedTime += Time.deltaTime;
        if (elapsedTime >= AtkSpeed)
        { // 한번 도달하면 되돌아오게
            m_mobController.transform.position = m_targetPos;
            m_targetPos = m_OriPos;
            elapsedTime = 0;
        }
    }
    public void OperateExit(MobController sender)
    {
        m_mobController.transform.position = m_OriPos;
        elapsedTime = 0;
        TurnManager.instance.setTurn(sender.gameObject, true);
    }
}
