public class PlayerAnimation : CharaterAnimation, IInitializeVariables, ISubcribers
{
    private PlayerMain playerMain;
    
    void Start()
    {
        InitializeVariables();
        SubscribeEvent();//
    }
    private void OnDisable()
    {
        UnSubscribeEvent();//
    }
    
    public void InitializeVariables()
    {
        playerMain = PlayerMain.Instance;
    }
    
    public void SubscribeEvent()
    {
        playerMain.OnAttack += PlayAttackAnimation;//
        playerMain.OnDance += PlayDanceAnimation;
        playerMain.OnDead += PlayDeadAnimation;
        playerMain.OnIdle += PlayIdleAnimation;
        playerMain.OnRun += PlayRunAnimation;
        playerMain.OnWin += PlayWinAnimation;
    }

    public void UnSubscribeEvent()
    {
        
    }
    public void AttackFromPlayerMain()
    {
        playerMain.Attack();
    }
    public void PlayWinAnim()
    {
        PlayWinAnimation();
    }
    public void PlayDanceAnim()
    {
        PlayDanceAnimation();
    }
}
