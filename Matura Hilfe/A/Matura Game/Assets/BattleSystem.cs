using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // <-- damit du DoTween benutzen kannst

public class BattleSystem : MonoBehaviour
{
    public GameObject PlayerChar;
    public GameObject EnemyChar;
    public Transform playerSpawn;
    public Transform enemySpawn;
    public Transform infoSpawn;
     
    private Unit PlayerUnit;
    private Unit EnemyUnit;

    public enum BattleState { start, Playerturn, Enemyturn, won, lost }

    public BattleState state;
    public CharHUD playerHUD;
    public CharHUD enemyHUD;
    public GameObject infoBox;
    public Text StateText;
    public Text Information;

    [Header("ByDave")]
    public Transform container_attackButtons;
    public float attackButtonAnimSpeed;
    public float attackButtonYHideRange;
    private Vector3 originalAttackButtonsPos;

    void Start()
    {  
        state = BattleState.start;

        attackButtonsShown = true;
        // MoveAttackButtons(false);

        SetupBattle();
        StartCoroutine(SetupBattle());
    }

    private string lastInformation;
    private void Update()
    {
        // bischen weird das so zu coden, aber egal
        /// sobald sich der Informations text zu "" AttackButtons angezeigt
        
        // text hat sich geändert
        if(lastInformation != Information.text)
            MoveAttackButtons(Information.text == "");

        lastInformation = Information.text;
    }

    IEnumerator SetupBattle()
    {
        GameObject PlayerGO = Instantiate(PlayerChar,playerSpawn);
        PlayerUnit = PlayerGO.GetComponent<Unit>();

        GameObject EnemyGO = Instantiate(EnemyChar,enemySpawn);
        EnemyUnit = EnemyGO.GetComponent<Unit>();

        yield return new WaitForSeconds(1f);

        Information.text="The fight starts!";

        enemyHUD.SetHUD(EnemyUnit);
        playerHUD.SetHUD(PlayerUnit);

        StateText.text="Start";

        yield return new WaitForSeconds(1f);
        infoBox.transform.position = new Vector2(1000,0);
        Information.text="";

        state=BattleState.Playerturn;
        Playerturn();
    }

    IEnumerator PlayerAttack()
    {
        bool isDead = EnemyUnit.TakeDamage(PlayerUnit.damage);
        enemyHUD.SetHP(EnemyUnit.currentHP);
        
        Information.text="You have done "+ PlayerUnit.damage +" Damage!";
      
        if(isDead)
        {
            state=BattleState.won;
            yield return new WaitForSeconds(1f);
            Information.text="Gewonnen!";
            EndBattle();
        } else
        {
            state=BattleState.Enemyturn;
            yield return new WaitForSeconds(1f);
            Information.text="";
            StartCoroutine(Enemyturn());
        }
    }

    IEnumerator Enemyturn()
    {
        bool isDead=PlayerUnit.TakeDamage(EnemyUnit.damage);
        playerHUD.SetHP(PlayerUnit.currentHP);
        StateText.text="Enemyturn";
        Information.text="You have received "+ EnemyUnit.damage +" Damage!";

        yield return new WaitForSeconds(1f);
        Information.text="";

        if(isDead)
        {
            state=BattleState.lost;
            yield return new WaitForSeconds(1f);
            Information.text="Verloren!";
            EndBattle();
        }
        else
        {
            state=BattleState.Playerturn;
            Playerturn();
        }
    }

    void EndBattle()
    {
        StateText.text="Fertig";
    }

    void Playerturn()
    {
        StateText.text="Playerturn";
        Instantiate(infoBox,infoSpawn);
    }

    public void OnAttackButton()
    {
        if (state != BattleState.Playerturn)
            return;
        
        StartCoroutine(PlayerAttack());
    }

    #region AttackButton Animation

    private bool attackButtonsShown;
    public void MoveAttackButtons(bool show)
    {
        // wenn sie schon angezeigt sind und man sie anzeigen will -> funktion stoppen (return)
        if (attackButtonsShown == show) return;

        Vector3 direction = show ? Vector3.up : Vector3.down;
        Vector3 adjustedPos = container_attackButtons.position + direction * attackButtonYHideRange;

        //container_attackButtons.position = adjustedPos;

        attackButtonsShown = show;

        print(show);
        container_attackButtons.DOKill();
        container_attackButtons.DOMove(adjustedPos, attackButtonAnimSpeed);
        //online = show;
    }

    #endregion
}
