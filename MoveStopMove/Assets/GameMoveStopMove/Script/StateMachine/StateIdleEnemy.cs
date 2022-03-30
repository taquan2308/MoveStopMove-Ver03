public class StateIdleEnemy : IState
{
    public void OnEnter(EnemyMain enemyMain)
    {
        //attack
        enemyMain.EnemyAnimation.SetStateAnimation(CharaterAnimation.StateCharacter.idle_trigger);
        enemyMain.ShowArrow();
        enemyMain.ResetTimeIdle();
    }

    public void OnExecute(EnemyMain enemyMain)
    {
        //check enemyNearest to State attack
        enemyMain.FindTransNearestEnemy();
        enemyMain.LockOntarget();
        enemyMain.CheckIsIdleToAttack();//check enemyNearest to State attack
        enemyMain.CheckIsIdleToRun();//check enemyNearest to State Patrol
        enemyMain.CountDowntIdle();
        enemyMain.CountDowntAttack();
    }

    public void OnExit(EnemyMain enemyMain)
    {
        enemyMain.EnemyAnimation.ResetAllTrigger();
    }

}
