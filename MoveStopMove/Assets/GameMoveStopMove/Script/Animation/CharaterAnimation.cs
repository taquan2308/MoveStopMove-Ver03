using System.Collections.Generic;
using UnityEngine;

public class CharaterAnimation : MonoBehaviour
{
    private Animator animator;
    //Check State Player
    [HideInInspector]
    public enum StateCharacter
    {
        attack_trigger,
        dance_trigger,
        dead_trigger,
        deadReverse_trigger,
        idle_trigger,
        run_trigger,
        ulti_trigger,
        win_trigger
    }
    //
    protected bool isAttack;
    protected bool isDance;
    protected bool isDead;
    protected bool isDeadReverse;
    protected bool isIdle;
    protected bool isRun;
    protected bool isUlti;
    protected bool isWin;
    protected IDictionary<StateCharacter, string> dictionaryStringTrigger = new Dictionary<StateCharacter, string>();
    //protected bool[] arrayBoolState;
    public bool IsAttack { get => isAttack; set => isAttack = value; }
    public bool IsDance { get => isDance; set => isDance = value;}
    public bool IsDead { get => isDead; set => isDead = value;}
    public bool IsDeadReverse { get => isDeadReverse; set => isDeadReverse = value;}
    public bool IsIdle { get => isIdle; set => isIdle = value;}
    public bool IsRun { get => isRun; set => isRun = value;}
    public bool IsUlti { get => isUlti; set => isUlti = value;}
    public bool IsWin { get => isWin; set => isWin = value;}
    public Animator Animator { get => animator; set => animator = value;}
    private void Awake()
    {
        InitializeVariablesCharAnim();
    }
    public void InitializeVariablesCharAnim()
    {
        animator = GetComponent<Animator>();
        isAttack = false;
        isDance = false;
        isDead = false;
        isDeadReverse = false;
        isRun = false;
        isUlti = false;
        isWin = false;
        SetStateAnimation(StateCharacter.idle_trigger);
        dictionaryStringTrigger.Add(StateCharacter.attack_trigger, "Attack_Trigger");
        dictionaryStringTrigger.Add(StateCharacter.dance_trigger, "Dance_Trigger");
        dictionaryStringTrigger.Add(StateCharacter.deadReverse_trigger, "DeadReverse_Trigger");
        dictionaryStringTrigger.Add(StateCharacter.dead_trigger, "Dead_Trigger");
        dictionaryStringTrigger.Add(StateCharacter.idle_trigger, "Idle_Trigger");
        dictionaryStringTrigger.Add(StateCharacter.run_trigger, "Run_Trigger");
        dictionaryStringTrigger.Add(StateCharacter.ulti_trigger, "Ulti_Trigger");
        dictionaryStringTrigger.Add(StateCharacter.win_trigger, "Win_Trigger");
    }
    #region Set State Player Function
    public void SetStateAnimation(StateCharacter _stateCharacter)
    {
        if (_stateCharacter == StateCharacter.attack_trigger)
        {
            isAttack = true;
            isDance = false;
            isDead = false;
            isDeadReverse = false;
            isRun = false;
            isIdle = false;
            isUlti = false;
            isWin = false;
            ResetAllTrigger();
            animator.SetTrigger("Attack_Trigger");
        }
        else if (_stateCharacter == StateCharacter.dance_trigger)
        {
            isAttack = false;
            isDance = true;
            isDead = false;
            isDeadReverse = false;
            isRun = false;
            isIdle = false;
            isUlti = false;
            isWin = false;
            ResetAllTrigger();
            animator.SetTrigger("Dance_Trigger");
        }
        else if (_stateCharacter == StateCharacter.dead_trigger)
        {
            isAttack = false;
            isDance = false;
            isDead = true;
            isDeadReverse = false;
            isRun = false;
            isIdle = false;
            isUlti = false;
            isWin = false;
            ResetAllTrigger();
            animator.SetTrigger("Dead_Trigger");
        }
        else if (_stateCharacter == StateCharacter.deadReverse_trigger)
        {
            isAttack = false;
            isDance = false;
            isDead = false;
            isDeadReverse = true;
            isRun = false;
            isIdle = false;
            isUlti = false;
            isWin = false;
            ResetAllTrigger();
            animator.SetTrigger("DeadReverse_Trigger");
        }
        else if (_stateCharacter == StateCharacter.idle_trigger)
        {
            isAttack = false;
            isDance = false;
            isDead = false;
            isDeadReverse = false;
            isRun = false;
            isIdle = true;
            isUlti = false;
            isWin = false;
            ResetAllTrigger();
            animator.SetTrigger("Idle_Trigger");
        }
        else if (_stateCharacter == StateCharacter.run_trigger)
        {
            isAttack = false;
            isDance = false;
            isDead = false;
            isDeadReverse = false;
            isRun = true;
            isIdle = false;
            isUlti = false;
            isWin = false;
            ResetAllTrigger();
            animator.SetTrigger("Run_Trigger");
        }
        else if (_stateCharacter == StateCharacter.ulti_trigger)
        {
            isAttack = false;
            isDance = false;
            isDead = false;
            isDeadReverse = false;
            isRun = false;
            isIdle = false;
            isUlti = true;
            isWin = false;
            ResetAllTrigger();
            animator.SetTrigger("Ulti_Trigger");
        }
        else if (_stateCharacter == StateCharacter.win_trigger)
        {
            isAttack = false;
            isDance = false;
            isDead = false;
            isDeadReverse = false;
            isRun = false;
            isIdle = false;
            isUlti = false;
            isWin = true;
            ResetAllTrigger();
            animator.SetTrigger("Win_Trigger");
        }
    }
    #endregion
    public void CheckStateTime()
    {
        //animator.
    }
    public void ResetAllTrigger()
    {
        foreach (var dict in dictionaryStringTrigger)
        {
            animator.ResetTrigger(dict.Value);
        }
    }
    #region PlayAnimation
    protected void PlayAttackAnimation()
    {
        SetStateAnimation(StateCharacter.attack_trigger);
    }
    protected void PlayDanceAnimation()
    {
        SetStateAnimation(StateCharacter.dance_trigger);
    }
    protected void PlayDeadAnimation()
    {
        SetStateAnimation(StateCharacter.dead_trigger);
    }
    protected void PlayDeadReverseAnimation()
    {
        SetStateAnimation(StateCharacter.deadReverse_trigger);
    }
    protected void PlayIdleAnimation()
    {
        SetStateAnimation(StateCharacter.idle_trigger);
    }
    protected void PlayRunAnimation()
    {
        SetStateAnimation(StateCharacter.run_trigger);
    }
    protected void PlayUltiAnimation()
    {
        SetStateAnimation(StateCharacter.ulti_trigger);
    }
    protected void PlayWinAnimation()
    {
        SetStateAnimation(StateCharacter.win_trigger);
    }
    #endregion
}
