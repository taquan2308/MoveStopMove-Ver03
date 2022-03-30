public class StatePatrolEnemy : IState
{
    public void OnEnter(EnemyMain enemyMain)
    {
        //change anim run
        enemyMain.FindTransNearestEnemy();
        enemyMain.AsignDestination();
        enemyMain.EnemyAnimation.SetStateAnimation(CharaterAnimation.StateCharacter.run_trigger);
        enemyMain.ResetBoolIsFirstAttackEveryTimeRun();
    }

    public void OnExecute(EnemyMain enemyMain)
    {
        //run
        enemyMain.CheckIsRunToIdle();
        enemyMain.CountDowntIdle();
        enemyMain.CountDowntAttack();
    }

    public void OnExit(EnemyMain enemyMain)
    {
        enemyMain.EnemyAnimation.ResetAllTrigger();
    }

}
