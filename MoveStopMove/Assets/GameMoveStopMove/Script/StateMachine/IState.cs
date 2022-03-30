public interface IState
{
    void OnEnter(EnemyMain enemyMain);
    void OnExecute(EnemyMain enemyMain);
    void OnExit(EnemyMain enemyMain);
}
