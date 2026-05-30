using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAtk : IState<PlayerController>
{
    Vector2 m_Dir;
    float AtkSpeed;
    private PlayerController m_playerController;
    float elapsedTime;

    Vector2 atkCenter;
    Vector2 atkSize;
    int atkDamage;

    public void OperateEnter(PlayerController sender)
    {
        if (!m_playerController)
            m_playerController = sender;
        initStat();

        atkCenter = (Vector2)sender.transform.position + m_Dir * 0.5f;

        if (BattleManager.Instance != null)
            BattleManager.Instance.AddLogMessage($"레이나 일반공격 사용!");

        normalAtk();

        elapsedTime += Time.deltaTime;
    }
    public void OperateUpdate(PlayerController sender)
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= AtkSpeed)
        {
            sender.EnemyHit = false;
        }
    }
    public void OperateExit(PlayerController sender)
    {
        elapsedTime = 0;
        TurnManager.instance.EndPlayerTurn();
    }

    void normalAtk()
    {
        atkSize = new Vector2(0.5f, 0.5f);
        Collider2D[] others = Physics2D.OverlapBoxAll(atkCenter, atkSize, 0);

        foreach (Collider2D collider in others)
        {
            if (collider.CompareTag("Enemy"))
            {
                collider.gameObject.GetComponent<MobStat>().TakeDamage(atkDamage);
            }
        }
    }

    void initStat()
    {
        m_Dir = m_playerController.Dir;
        AtkSpeed = m_playerController.speed / 2f; // 수치는 애니메이션 뽑히는거 보고 바뀌지 않을지. / 아니면 애니메이터에서 관리?
        atkDamage = m_playerController.GetComponent<PlayerStat>().Atk;
    }
}