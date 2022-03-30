using System.Collections.Generic;
using UnityEngine;

// named Arrow2 because do multi Project on One Project, other has Arrow
public class Arrow : MonoBehaviour, IInitializeVariables
{
    private PlayerMain playerMain;
    [SerializeField] private ArrowSO arrowSO2;
    private float speedArrow2;
    private Vector3 targetPosition;
    private Vector3 dirrectVector3;
    private float rangeAttack;
    private Vector3 offsetPositionTarget;
    private int iDOwner;
    private float rangeAdd;
    private int experienceAdd;
    private float distaneToTarget;
    private bool isFirtSetUp;
    public ArrowSO ArrowSO2 { get => arrowSO2; set => arrowSO2 = value; }
    public float SpeedArrow2 { get => speedArrow2; set => speedArrow2 = value; }
    public Vector3 TargetPosition { get => targetPosition; set => targetPosition = value; }
    public Vector3 DirrectVector3 { get => dirrectVector3; set => dirrectVector3 = value; }
    public float RangeAttack { get => rangeAttack; set => rangeAttack = value; }
    public Vector3 OffsetPositionTarget { get => offsetPositionTarget; set => offsetPositionTarget = value; }
    public int IDOwner { get => iDOwner; set => iDOwner = value; }
    public float RangeAdd { get => rangeAdd; set => rangeAdd = value; }
    public int ExperienceAdd { get => experienceAdd; set => experienceAdd = value; }
    public float DistaneToTarget { get => distaneToTarget; set => distaneToTarget = value; }
    public bool IsFirtSetUp { get => isFirtSetUp; set => isFirtSetUp = value; }
    
    // Start is called before the first frame update
    void Start()
    {
        InitializeVariables();
    }

    // Update is called once per frame
    void Update()
    {
        MoveArrow2();
    }
    public void SetTaget(Vector3 _targetPosition, float _rangeAttack, int _iDOwner)
    {
        targetPosition = _targetPosition + offsetPositionTarget;
        rangeAttack = _rangeAttack;
        iDOwner = _iDOwner;
    }
    public void MoveArrow2()
    {
        // Move Arrow to Max Pig rangeAttack Player with direction cacular
        dirrectVector3 = targetPosition - transform.position;
        if (isFirtSetUp)
        {
            targetPosition = transform.position + (dirrectVector3.normalized * rangeAttack);
            isFirtSetUp = false;
        }
        distaneToTarget = dirrectVector3.magnitude;
        transform.Translate(dirrectVector3.normalized * Time.deltaTime * speedArrow2, Space.World);
        // do Arrow look at Enemy Direction
        if (!arrowSO2.isRoteArrow)
        {
            Vector3 dirPlayerToEnemy = targetPosition - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dirPlayerToEnemy);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 30).eulerAngles;//Time.deltaTime * turnSpeed
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
        //
        if ((targetPosition - transform.position).magnitude <= 0.1f)
        {
            gameObject.SetActive(false);
            //Reset is firt setup for next time active
            isFirtSetUp = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
        //check attack, default iDOwner = 0 if not yet asign
        if (iDOwner == 0 || ((int)iDOwner - (int)other.gameObject.GetInstanceID()) == 0)
        {
            return;
        }
        if ( other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player") && ((int)iDOwner - (int)other.gameObject.GetInstanceID()) != 0)
        {
            //
            Character otherCharacter = other.GetComponent<Character>();
            EnemyMain otherEnemyMain = other.GetComponent<EnemyMain>();
            Transform otherTransform = other.GetComponent<Transform>();
            PlayerAnimation otherPlayerAnimation = null;
            if (other.GetComponent<PlayerAnimation>() != null)
            {
                otherPlayerAnimation = other.GetComponent<PlayerAnimation>();
            }
            //PlayerMain otherPlayerMain = other.GetComponent<PlayerMain>();
            if (otherPlayerAnimation != null)
            {
                GameManager.Instance.GameLose = true;
                playerMain.DieLater();
            }
            if (otherEnemyMain != null)
            {
                otherEnemyMain.DieLater();
            }
            if (GameManager.Instance.ListCharacter.Contains(otherCharacter))
            {
                GameManager.Instance.ListCharacter.Remove(otherCharacter);
                GameManager.Instance.ListTransformCharacter.Remove(otherTransform);
                GameManager.Instance.EnemyAlive -= 1;
                UIManager.Instance.UpdateAlives();
            }
            for (int i = 0; i < GameManager.Instance.ListCharacter.Count; i++)
            {
                if (GameManager.Instance.ListCharacter[i] != null)
                {
                    if (GameManager.Instance.ListCharacter[i].gameObject.GetInstanceID() == iDOwner)
                    {
                        if ((GameManager.Instance.ListCharacter[i] as PlayerMain) != null)
                        {
                            GameManager.Instance.KilledCount += 1;
                            playerMain.RangeAttack += playerMain.RangeAttack * rangeAdd;
                            playerMain.Experience += 2;
                            playerMain.IsAddExp = true;
                            playerMain.IsEffect = true;
                            playerMain.PlayDieAudio();
                            playerMain.ShowArrow();
                            gameObject.SetActive(false);
                        }
                        EnemyMain enemyMainOwner;
                        enemyMainOwner = GameManager.Instance.ListCharacter[i] as EnemyMain;
                        if (enemyMainOwner != null)
                        {
                            enemyMainOwner.RangeAttack += enemyMainOwner.RangeAttack * rangeAdd;
                            enemyMainOwner.Experience += 2;
                            enemyMainOwner.IsAddExp = true;
                            enemyMainOwner.IsEffect = true;
                            enemyMainOwner.PlayDieAudio();
                            if (otherPlayerAnimation != null)
                            {
                                GameManager.Instance.NameKillPlayer = enemyMainOwner.NameEnemy;
                            }
                            enemyMainOwner.ShowArrow();
                            gameObject.SetActive(false);
                        }
                    }
                }
            }
            gameObject.SetActive(false);
        }
    }

    public void InitializeVariables()
    {
        playerMain = PlayerMain.Instance;
        speedArrow2 = arrowSO2.speedArrow2;
        isFirtSetUp = true;
        offsetPositionTarget = new Vector3(0, 1.26f, 0);
        // add 10 %
        rangeAdd = 0.1f;
        experienceAdd = 2;
    }
}
