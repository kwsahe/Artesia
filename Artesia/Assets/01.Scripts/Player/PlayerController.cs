using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, ITurn
{
    public enum PlayerState
    {
        Idle,
        Move,
        Atk,
        Skill,
    }

    [SerializeField] private Dictionary<PlayerState, IState<PlayerController>> dicState = new Dictionary<PlayerState, IState<PlayerController>>();
    private StateMachine<PlayerController> SM;
    public Vector2 OriPos { get; private set; }
    public Vector2 Dir { get; set; } = Vector2.down;
    public Vector2 TargetPos { get; private set; }
    [SerializeField][Range(0.0001f, 1f)][Tooltip("커질수록 느려짐")] float Speed = 0.2f;
    public float speed
    {
        get { return Speed; }
    }
    public bool isMoving { get; set; } = false;
    public bool isSkillActive { get; set; } = false;
    public bool PlayedTurn { get; set; }
    public bool EnemyHit { get; set; } = false;

    public string destroySceneName;

    private void Awake()
    {
        IState<PlayerController> idle = new PlayerIdle();
        IState<PlayerController> move = new PlayerMove();
        IState<PlayerController> atk = new PlayerAtk();
        IState<PlayerController> skill = new PlayerSkill();

        dicState.Add(PlayerState.Idle, idle);
        dicState.Add(PlayerState.Move, move);
        dicState.Add(PlayerState.Atk, atk);
        dicState.Add(PlayerState.Skill, skill);


        SM = new StateMachine<PlayerController>(this, dicState[PlayerState.Idle]);
    }

    void Start()
    {
        MovePos();
        DontDestroyOnLoad(gameObject);
        if (GameObject.FindGameObjectsWithTag("Player").Length > 1)
            Destroy(gameObject);
        UIManager.instance.SetActiveUI("Status", true);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == destroySceneName)
            Destroy(gameObject);

        if ((Vector2)transform.position == TargetPos && SM.CurState == dicState[PlayerState.Move])
            SM.SetState(dicState[PlayerState.Idle]);

        if (!EnemyHit && SM.CurState == dicState[PlayerState.Atk])
            SM.SetState(dicState[PlayerState.Idle]);

        if (SM.CurState == dicState[PlayerState.Skill] && !isSkillActive)
        {
            SM.SetState(dicState[PlayerState.Idle]);
        }

        if (Mathf.Abs(Dir.x) == 1)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = (Dir.x == 1);
        }

        SM.DoOperateUpdate();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "BaseCamp")
        {
            transform.position = Vector3.zero;
        }
    }

    public void AnimationUpdate()
    {
        Animator animator;

        animator = GetComponent<Animator>();

        animator.SetFloat("DirX", Dir.x);
        animator.SetFloat("DirY", Dir.y);
        animator.SetBool("isMoving", isMoving);
    }

    void OnMove(InputValue value)
    {
        if (!PlayedTurn && !UIManager.instance.isFade)
        {
            Vector2 input = value.Get<Vector2>();
            if (input != Vector2.zero && SM.CurState == dicState[PlayerState.Idle])
            { // 
                DirControl(input);
                Vector3Int OriPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);
                RaycastHit2D hit = Physics2D.Raycast((Vector2Int)OriPos, input, 1, LayerMask.GetMask("Tile") | LayerMask.GetMask("Enemy"));

                if (!hit)
                {
                    TargetPos = OriPos + new Vector3(Dir.x, Dir.y, 0);

                    SM.SetState(dicState[PlayerState.Move]);
                }
            }

        }
    }

    void OnSkill(InputValue value)
    {
        if (!PlayedTurn && SM.CurState == dicState[PlayerState.Idle] && !UIManager.instance.isFade)
        {
            SM.SetState(dicState[PlayerState.Skill]);
            isSkillActive = true;
        }
    }

    void OnOption(InputValue value)
    {
        UIManager.instance.SetActiveUI("option", true);
        if (SceneManager.GetActiveScene().name != "BaseCamp")
            UIManager.instance.SetActiveUI("escape", true);
        Time.timeScale = 0f;
    }

    void OnAtk(InputValue value)
    {
        if (!PlayedTurn && SM.CurState == dicState[PlayerState.Idle] && !UIManager.instance.isFade)
        {
            EnemyHit = true;
            SM.SetState(dicState[PlayerState.Atk]);
        }
    }

    public void DirControl(Vector2 dir)
    { //
        Vector2 Result = dir;
        float x, y;
        x = Mathf.Abs(Result.x);
        y = Mathf.Abs(Result.y);

        if ((x + y) > 1)
        {
            Result.x = Result.x > 0 ? Mathf.CeilToInt(Result.x) : Mathf.FloorToInt(Result.x);
            Result.y = Result.y > 0 ? Mathf.CeilToInt(Result.y) : Mathf.FloorToInt(Result.y);
        }

        Dir = Result;
        AnimationUpdate();
    }

    public void MovePos()
    {
        SM.SetState(dicState[PlayerState.Idle]);

        if (MapGenerator.instance != null)
            transform.position = MapGenerator.instance.StartPos;
        else
            transform.position = Vector2.zero;
    }

    public void MovePos(Vector3 pos)
    {
        SM.SetState(dicState[PlayerState.Idle]);
        transform.position = pos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube((Vector2)transform.position + Dir, new Vector2(0.5f, 0.5f));
    }
}