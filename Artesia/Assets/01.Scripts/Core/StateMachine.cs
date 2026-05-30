using UnityEngine;

public class StateMachine<T>
{
    private T m_sender;

    //현재 상태를 담는 프로퍼티
    public IState<T> CurState { get; set; }


    //기본 상태를 생성시에 설정하게 생성자 선언
    public StateMachine(T sender, IState<T> state)
    {
        m_sender = sender;
        SetState(state);
    }

    public void SetState(IState<T> state)
    {
        if (m_sender == null || CurState == state)
        {
            return;
        }

        if (CurState != null)
            CurState.OperateExit(m_sender);

        //상태 교체.
        CurState = state;

        //새 상태의 Enter를 호출한다.
        if (CurState != null)
            CurState.OperateEnter(m_sender);
    }

    //State용 Update 함수.
    public void DoOperateUpdate()
    {
        if (m_sender == null)
            return;
        CurState.OperateUpdate(m_sender);
    }
}