public class EnemyAnimation : CharaterAnimation, IInitializeVariables, ISubcribers
{
    private EnemyMain enemyMain;
    
    private void OnEnable()
    {
        InitializeVariables();
        SubscribeEvent();
    }
    private void OnDisable()
    {
        UnSubscribeEvent();
    }

    public void InitializeVariables()
    {
        enemyMain = gameObject.GetComponent<EnemyMain>();
    }
    

    public void SubscribeEvent()
    {
        enemyMain.OnAttack += PlayAttackAnimation;
        enemyMain.OnDance += PlayDanceAnimation;
        enemyMain.OnDead += PlayDeadAnimation;
        enemyMain.OnDeadReverse += PlayDeadReverseAnimation;
        enemyMain.OnIdle += PlayIdleAnimation;
        enemyMain.OnRun += PlayRunAnimation;
        enemyMain.OnWin += PlayWinAnimation;
        //throw new System.NotImplementedException();
    }

    public void UnSubscribeEvent()
    {
        enemyMain.OnAttack -= PlayAttackAnimation;
        enemyMain.OnDance -= PlayDanceAnimation;
        enemyMain.OnDead -= PlayDeadAnimation;
        enemyMain.OnDeadReverse -= PlayDeadReverseAnimation;
        enemyMain.OnIdle -= PlayIdleAnimation;
        enemyMain.OnRun -= PlayRunAnimation;
        enemyMain.OnWin -= PlayWinAnimation;
        //throw new System.NotImplementedException();
    }
}
