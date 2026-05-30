using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobIdle : IState<MobController>
{
    private MobController m_mobController;

    public void OperateEnter(MobController sender)
    {
        m_mobController = sender;
    }
    public void OperateUpdate(MobController sender)
    {
        return;
    }
    public void OperateExit(MobController sender)
    {

    }
}
