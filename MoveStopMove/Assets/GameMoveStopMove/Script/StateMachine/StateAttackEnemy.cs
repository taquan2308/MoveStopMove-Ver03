public class StateAttackEnemy : IState
{
    public void OnEnter(EnemyMain enemyMain)
    {
        //change anim attack
        enemyMain.EnemyAnimation.SetStateAnimation(CharaterAnimation.StateCharacter.attack_trigger);
        enemyMain.HideArrow();
    }

    public void OnExecute(EnemyMain enemyMain)
    {
        //attack
        enemyMain.CheckIsAttackToIdle();
    }

    public void OnExit(EnemyMain enemyMain)
    {
        enemyMain.EnemyAnimation.ResetAllTrigger();
    }

}
