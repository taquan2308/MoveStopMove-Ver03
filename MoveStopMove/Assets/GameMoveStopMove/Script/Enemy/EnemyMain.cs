using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.AI;
using Random = System.Random;
using System.Collections.Generic;

public class EnemyMain : Character, IInitializeVariables
{
    #region Parameter
    private PlayerMain playerMain;
    private EnemyAnimation enemyAnimation;
    public event Action OnAttack = delegate { };
    public event Action OnDance = delegate { };
    public event Action OnDead = delegate { };
    public event Action OnDeadReverse = delegate { };
    public event Action OnIdle = delegate { };
    public event Action OnRun = delegate { };
    public event Action OnUlti = delegate { };
    public event Action OnWin = delegate { };
    private NavMeshAgent agent;
    private Rigidbody enemyRb;
    [HideInInspector] public Vector3 enemyOldPos;
    private GameObject[] enemys;
    [SerializeField] private float turnSpeed;
    [SerializeField] private Vector2 direction;
    [SerializeField] private Vector3 directionMove;
    [SerializeField] private Vector3 dir;
    //Attack
    private Transform nearestEnemyOtherFromThisEnemyTrans;
    [SerializeField] private float rangeAttack;
    [SerializeField] private int experience;
    [SerializeField] private string nameEnemy;
    [SerializeField] private EnemySO enemySO;
    [SerializeField] private Transform pointFire;
    [SerializeField] private float timeStartAttack;
    private float timeCountdowntAttack;
    private float timeStartIdle;
    private float timeCountdowntIdle;
    private Transform thisEnemyTransform;

    // get radompoint
    private RandomPoints randomPoints;
    private Vector3 pointToGo;
    //Exp Canvas
    private TextMeshProUGUI txtExp;
    private TextMeshProUGUI txtNameEnemy;
    private Transform canvasExpTrans;
    private bool isAddExp;
    [SerializeField] private GameObject expPrefabs;
    //Animation Player
    [SerializeField] private Animator playerAni;
    //Check State Player
    
    //Check first attack each time idle
    private bool isFirstAttackEveryTimeIdle;
    //Effect
    [SerializeField] private GameObject effectPrefabs;
    [SerializeField] private bool isEffect;
    //Audio
    [SerializeField] private AudioClip attackAudio;
    [SerializeField] private AudioClip dieAudio;
    private AudioSource audioSource;
    //Has point random
    //Play Game
    [SerializeField] private bool isPlay;
    [SerializeField] private GameObject arrowObject;
    private bool isSpawn;
    private bool isFirstTimeSpawnIconShowDirection;
    //
    public bool IsPlay { get => isPlay; set => isPlay = value; }
    public float RangeAttack { get => rangeAttack; set => rangeAttack = value; }
    public int Experience { get => experience; set => experience = value; }
    public bool IsAddExp { get => isAddExp; set => isAddExp = value; }
    public bool IsEffect { get => isEffect; set => isEffect = value; }
    public EnemySO EnemySO { get => enemySO; set => enemySO = value; }
    public Transform PointFire { get => pointFire; set => pointFire = value; }
    public Transform NearestEnemyOtherFromThisEnemyTrans { get => nearestEnemyOtherFromThisEnemyTrans; set => nearestEnemyOtherFromThisEnemyTrans = value; }
    public EnemyAnimation EnemyAnimation { get => enemyAnimation; set => enemyAnimation = value; }
    public GameObject ArrowObject { get => arrowObject; set => arrowObject = value; }
    public bool IsSpawn { get => isSpawn; set => isSpawn = value; }
    public string NameEnemy { get => nameEnemy; set => nameEnemy = value; }
    private IState currentState;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        InitializeVariables();//
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSpawn)
        {
            isSpawn = true;
            //get name
            nameEnemy = GameManager.Instance.ArrayName[GameManager.Instance.IndexName++];
            txtNameEnemy.text = nameEnemy;
            //
        }
        if (isPlay)
        {
            if (!isFirstTimeSpawnIconShowDirection)
            {
                isFirstTimeSpawnIconShowDirection = true;
                GameObject showPositionOnUi = (GameObject)Instantiate(UIManager.Instance.ShowPositionOnUi, UIManager.Instance.CanvasLive.transform);
                showPositionOnUi.GetComponent<ShowPositionOnUi>().GetTransformEnemy(this.gameObject.transform);
            }
            if (currentState != null)
            {
                currentState.OnExecute(this);
            }

            if (!enemyAnimation.IsDead)
            {
                #region grow,exp
                //Grow
                Grow();
                //
                if (isEffect)
                {
                    isEffect = false;
                    GameObject effectAdd = SpawnArrow.Instance.Spawns(effectPrefabs);
                    effectAdd.transform.position = transform.position;
                    effectAdd.transform.rotation = expPrefabs.transform.rotation;
                }
                
                if (isAddExp)
                {
                    isAddExp = false;
                    isEffect = false;
                    GameObject expAdd = SpawnArrow.Instance.Spawns(expPrefabs);
                    expAdd.transform.position = transform.position;
                    expAdd.transform.rotation = expPrefabs.transform.rotation;
                }
                #endregion
            }
        }
    }
    private void LateUpdate()
    {
        //Exp
        txtExp.text = experience.ToString();
        canvasExpTrans.eulerAngles = new Vector3(0, 90, 0);
    }

    public void ShowArrow()
    {
        if (arrowObject != null)
        {
            arrowObject.SetActive(true);
        }
    }
    public void HideArrow()
    {
        if (pointFire.childCount > 0)
        {
            arrowObject = pointFire.GetChild(0).gameObject;
        }
        if (arrowObject != null)
        {
            arrowObject.SetActive(false);
        }
    }
    public Transform FindTransNearestEnemy()
    {
        // Must Exclude itself
        float shortestDistance = Mathf.Infinity;
        nearestEnemyOtherFromThisEnemyTrans = null;
        //
        for (int i = 0; i < GameManager.Instance.ListTransformCharacter.Count; i++)
        {
            if (GameManager.Instance.ListTransformCharacter[i] != null && GameManager.Instance.ListTransformCharacter[i] != thisEnemyTransform)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, GameManager.Instance.ListTransformCharacter[i].position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemyOtherFromThisEnemyTrans = GameManager.Instance.ListTransformCharacter[i];
                }
            }
        }
        return nearestEnemyOtherFromThisEnemyTrans;
    }
    //
    public override void Attack()
    {
        //Play Audio
        PlayAttackAudio();
        GameObject arrow2 = SpawnArrow.Instance.Spawns(enemySO.arrowPrefabs);
        arrow2.transform.position = pointFire.position;
        arrow2.transform.rotation = enemySO.arrowPrefabs.transform.rotation;
        arrow2.GetComponent<Arrow>().SetTaget(nearestEnemyOtherFromThisEnemyTrans.position, rangeAttack, gameObject.GetInstanceID());
    }
    public void LockOntarget()
    {
        // check if have enemyNearest
        if (nearestEnemyOtherFromThisEnemyTrans != null)
        {
            Vector3 dirPlayerToEnemy = nearestEnemyOtherFromThisEnemyTrans.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dirPlayerToEnemy);
            //Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            //transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
            transform.rotation = lookRotation;
        }
    }
    public void Grow()
    {
        if (experience > 2)
        {
            transform.localScale = new Vector3(1 + experience / 2 * 0.1f, 1 + experience / 2 * 0.1f, 1 + experience / 2 * 0.1f);// buff 10% Scale
        }
    }
    //
    public void AsignDestination()
    {
        //check if in range no Enemmy
        if (nearestEnemyOtherFromThisEnemyTrans != null)//!isMove && 
        {
            pointToGo = nearestEnemyOtherFromThisEnemyTrans.GetComponent<RandomPoints>().GetPointRandomAroundThisObject();
            //
            directionMove = pointToGo - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(directionMove);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;//Time.deltaTime * turnSpeed
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
            Debug.DrawRay(pointToGo, Vector3.up, Color.black, 2);
            agent.destination = pointToGo;
        }
    }
    
    //
    public void PlayAttackAudio()
    {
        audioSource.PlayOneShot(attackAudio);
    }
    public void PlayDieAudio()
    {
        audioSource.PlayOneShot(dieAudio);
    }
    //
    IEnumerator DelayDie()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
    public void DieLater()
    {
        isPlay = false;
        OnDead?.Invoke();
        gameObject.tag = "Untagged";
        agent.isStopped = true;
        canvasExpTrans.gameObject.SetActive(false);
        StartCoroutine(DelayDie());
    }
    // chu y sua ben duoi
    private void OnCollisionEnter(Collision collision)// va vao nhau suot thi tim mai dich den ak?
    {
        if (collision.gameObject.CompareTag("Enemy")|| collision.gameObject.CompareTag("Player"))
        {
            AsignDestination();
        }
    }
    IEnumerator CallDeadInverse()
    {
        OnDeadReverse?.Invoke();
        yield return new WaitForSeconds(0.1f);
        isPlay = true;
    }
    public void StartDeadInverse()
    {
        StartCoroutine(CallDeadInverse());
    }
    //
    public bool CheckIsRunToIdle()
    {
        bool isIdle = false;
        if (!agent.hasPath)
        {
            isIdle = true;
            ChangeState(new StateIdleEnemy());
        }
        return isIdle;
    }
    public bool CheckIsAttackToIdle()//check animation is playing or has finished? to change State Idle,
    {
        bool isIdle = false;
        if (enemyAnimation.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !enemyAnimation.Animator.IsInTransition(0))//check if an animation is playing or has finished?
        {
            isIdle = true;
            ChangeState(new StateIdleEnemy());
        }
        return isIdle;
    }
    public bool CheckIsIdleToRun()
    {
        bool isRun = false;
        if (timeCountdowntIdle <= 0)
        {
            isRun = true;
            ChangeState(new StatePatrolEnemy());
        }
        return isRun;
    }
    public bool CheckIsIdleToAttack()//check range to change State Attack, coundownt time
    {
        bool isAttack = false;
        if (nearestEnemyOtherFromThisEnemyTrans != null)// neu co enemy
        {
            bool isOnrange = (transform.position - nearestEnemyOtherFromThisEnemyTrans.position).magnitude
                <= rangeAttack;
            if (timeCountdowntAttack <= 0 && isOnrange && isFirstAttackEveryTimeIdle)//neu trong tam
            {
                isFirstAttackEveryTimeIdle = false;
                timeCountdowntAttack = timeStartAttack;
                isAttack = true;
                ChangeState(new StateAttackEnemy());
            }
        }
        return isAttack;
    }
    public void ResetTimeIdle()
    {
        timeCountdowntIdle = timeStartIdle;
    }
    public void ResetBoolIsFirstAttackEveryTimeRun()
    {
        isFirstAttackEveryTimeIdle = true;
    }
    public void CountDowntIdle()
    {
        timeCountdowntIdle -= Time.deltaTime;
        timeCountdowntIdle = Mathf.Clamp(timeCountdowntIdle, 0, Mathf.Infinity);
    }
    public void CountDowntAttack()
    {
        timeCountdowntAttack -= Time.deltaTime;
        timeCountdowntAttack = Mathf.Clamp(timeCountdowntAttack, 0, Mathf.Infinity);
    }
    public void ChangeState(IState state)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = state;

        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }
    public void InitializeVariables()
    {
        playerMain = PlayerMain.Instance;
        isEffect = false;
        enemyAnimation = GetComponent<EnemyAnimation>();
        agent = GetComponent<NavMeshAgent>();
        enemyRb = GetComponent<Rigidbody>();
        enemyOldPos = transform.position;
        //attack
        rangeAttack = enemySO.rangeAttack;
        //Countdownt attack
        timeStartAttack = enemySO.speedAttack;
        timeCountdowntAttack = 0;
        timeStartIdle = enemySO.timeIdleStart;
        timeCountdowntIdle = timeStartIdle;
        // initialization 
        turnSpeed = enemySO.turnSpeed;
        // get radom level
        Random rnd = new Random();
        experience = rnd.Next(0, 4);
        //
        // get radompoint and going this
        randomPoints = GetComponent<RandomPoints>();
        //canvas
        txtExp = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        txtNameEnemy = gameObject.GetComponentsInChildren<TextMeshProUGUI>()[1];
        canvasExpTrans = gameObject.GetComponentsInChildren<Transform>()[1];
        isAddExp = false;
        // Set State
        isFirstAttackEveryTimeIdle = true;
        isEffect = false;
        //Audio
        audioSource = GetComponent<AudioSource>();
        isPlay = GameManager.Instance.IsPlay;
        GameManager.Instance.ListCharacter.Add(this.gameObject.GetComponent<Character>());
        GameManager.Instance.ListTransformCharacter.Add(this.gameObject.transform);//
        isFirstTimeSpawnIconShowDirection = false;
        //
        isSpawn = false;
        ChangeState(new StateIdleEnemy());
        thisEnemyTransform = this.gameObject.transform;
    }
}
