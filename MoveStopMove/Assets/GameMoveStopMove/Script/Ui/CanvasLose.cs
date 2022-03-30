using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CanvasLose : UICanvas, IInitializeVariables
{
    //Button
    [SerializeField] private Button exitBtn;
    [SerializeField] private Button retryBtn;
    //Price
    [SerializeField] private TextMeshProUGUI killedTxt;
    [SerializeField] private TextMeshProUGUI nameKill;
    [SerializeField] private TextMeshProUGUI ranking;
    
    //UI
    private PlayerMain playerMain;
    // Load weapon to UI
    public Button ExitBtn { get => exitBtn; set => exitBtn = value; }
    public Button RetryBtn { get => retryBtn; set => retryBtn = value; }
    public TextMeshProUGUI NameKill { get => nameKill; set => nameKill = value; }
    public TextMeshProUGUI Ranking { get => ranking; set => ranking = value; }
    public void Exit()
    {
        gameObject.SetActive(false);
        Retry();
    }
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void OnEnable()
    {
        InitializeVariables();
    }
    public void InitializeVariables()
    {
        playerMain = PlayerMain.Instance;
        exitBtn.onClick.AddListener(Exit);
        retryBtn.onClick.AddListener(Retry);
        killedTxt.text = "Killed: " + GameManager.Instance.KilledCount.ToString();// do not turn CanvasLoss in start game,  it make nature
        if (nameKill != null && GameManager.Instance.NameKillPlayer != null)
        {
            nameKill.text = GameManager.Instance.NameKillPlayer.ToString();// chet cung luc player win
        }
        ranking.text = "#" + (GameManager.Instance.EnemyAlive + 1).ToString();
        //
        nameUI = UIName.Lose;
    }
}
