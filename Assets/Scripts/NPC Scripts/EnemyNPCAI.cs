using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// La IA de todo EnemyNPC. Requiere estar en un prefab.
/// </summary>
public class EnemyNPCAI : MonoBehaviour {

    /// <summary>
    /// Modos de pelea del enemigo
    /// </summary>
    public enum EnemyMode
    {
        Defensive,
        Aggresive,
        Balanced,
        Afraid
    }

    /// <summary>
    /// Estados para actuar del enemigo
    /// </summary>
    public enum EnemyStates
    {
        Wait,
        Attack,
        Reposition,
        UseHighHability,
        UseMidHability,
        UseLowerHability,
        Escape
    }

    [HideInInspector] public EnemyNPC myNPC;
    [HideInInspector] public PlayerManager player;

    public EnemyMode defaultMode;
    [Space(10)]
    [Range(10, 100)] public float lifeThreshold1 = 50;
    [Range(0, 90)] public float lifeThreshold2 = 25;
    [Range(0, 30)] public float lifeThreshold3 = 0;
    [Space(5)]
    [Range(0.01f, 5)] public float timeBetweenIterations = 0.5f;
    [Space(5)]
    [Range(10, 100)] public float turnThreshold1 = 100;
    [Range(0, 90)] public float turnThreshold2 = 50;
    [Range(0, 30)] public float turnThreshold3 = 0;
    [Space(5)]
    [Range(0, 1)] public float chanceOfChangingMode = 0.3f;

    protected bool isActive = false;
    protected EnemyMode currentMode;
    protected EnemyStates currentState;

    protected List<bool> habilitiesReady = new List<bool>();

    protected float playerHealthPercent;
    protected float playerTurnPercent;
    protected float npcHealthPercent;
    protected float npcTurnPercent;

    /// <summary>
    /// Inicia la IA del enemigo.
    /// </summary>
    public void StartAI()
    {
        isActive = true;
        currentMode = defaultMode;

        StopCoroutine(WaitBetweenIterations(timeBetweenIterations));
        StartCoroutine(WaitBetweenIterations(timeBetweenIterations));
    }

    /// <summary>
    /// Termina el funcionamiento de la IA del enemigo.
    /// </summary>
    public void StopAI()
    {
        isActive = false;
        currentMode = defaultMode;
        StopCoroutine(WaitBetweenIterations(timeBetweenIterations));
    }

    /// <summary>
    /// El tiempo que esperará la IA antes de analizar la situación de combate.
    /// </summary>
    /// <param name="howMuchTime"></param>
    /// <returns></returns>
    public IEnumerator WaitBetweenIterations(float howMuchTime)
    {
        while (isActive)
        {
            yield return new WaitForSecondsRealtime(howMuchTime);
            AnalizeSituation();
        }
    }

    /// <summary>
    /// Analiza el estado de la vida propia contra la del jugador, cuanto turno falta para que el jugador actúe, si tiene
    /// habilidades, etc. Y lo resuelve según su modo de combate actual.
    /// Funciona como una "máquina de estados".
    /// </summary>
    public void AnalizeSituation()
    {
        if (myNPC.currentTurn < myNPC.myTemplate.MaxTurn)
        {
            currentState = EnemyStates.Wait;
            return;
        }

        CheckHabilities();

        playerHealthPercent = (player.currentHealth / player.MaxHealth) * 100;
        playerTurnPercent = (player.currentTurn / player.MaxTurn) * 100;

        npcHealthPercent = (myNPC.currentHealth / myNPC.myTemplate.MaxHealth) * 100;
        npcTurnPercent = (myNPC.currentTurn / myNPC.myTemplate.MaxTurn) * 100;


        float r = Random.Range(0f, 1f);

        if (r <= chanceOfChangingMode && currentMode == defaultMode)
        {
            switch (currentMode)
            {
                case EnemyMode.Aggresive:
                    if (npcHealthPercent < lifeThreshold3)
                    {
                        currentMode = EnemyMode.Afraid;
                    }
                    else if (npcHealthPercent < lifeThreshold2)
                    {
                        currentMode = EnemyMode.Defensive;
                    }
                    else if (npcHealthPercent < lifeThreshold1)
                    {
                        currentMode = EnemyMode.Balanced;
                    }
                    break;


                case EnemyMode.Defensive:
                    if (npcHealthPercent < lifeThreshold3)
                    {
                        currentMode = EnemyMode.Aggresive;
                    }
                    else if (npcHealthPercent < lifeThreshold2)
                    {
                        currentMode = EnemyMode.Balanced;
                    }
                    else if (npcHealthPercent < lifeThreshold1)
                    {
                        currentMode = EnemyMode.Afraid;
                    }

                    break;


                case EnemyMode.Balanced:
                    if (playerTurnPercent > turnThreshold1)
                    {
                        currentMode = EnemyMode.Defensive;
                    }
                    else if (playerTurnPercent > turnThreshold2)
                    {
                        currentMode = EnemyMode.Aggresive;
                    }
                    else if (npcHealthPercent < lifeThreshold2)
                    {
                        currentMode = EnemyMode.Afraid;
                    }
                    break;


                case EnemyMode.Afraid:
                    if (playerHealthPercent < lifeThreshold3)
                    {
                        currentMode = EnemyMode.Aggresive;
                    }
                    else if (playerHealthPercent < lifeThreshold2)
                    {
                        currentMode = EnemyMode.Balanced;
                    }
                    else if (playerHealthPercent < lifeThreshold1)
                    {
                        currentMode = EnemyMode.Defensive;
                    }
                    break;
            }
        }
        else
        {
            currentMode = defaultMode;
        }

        switch (currentMode)
        {
            case EnemyMode.Aggresive:
                if (npcHealthPercent < lifeThreshold3)
                {
                    currentState = EnemyStates.Reposition;
                }
                else if (playerHealthPercent < lifeThreshold3)
                {
                    if (habilitiesReady.Count > 2)
                    {
                        currentState = EnemyStates.UseHighHability;
                    }
                    else
                    {
                        currentState = EnemyStates.Attack;
                    }
                }
                else
                {
                    currentState = EnemyStates.Attack;
                }
                break;


            case EnemyMode.Defensive:
                if (playerTurnPercent < turnThreshold3)
                {
                    currentState = EnemyStates.Attack;
                }
                else if (playerTurnPercent < turnThreshold2 || npcHealthPercent > lifeThreshold2)
                {
                    if (habilitiesReady.Count > 0)
                    {
                        currentState = EnemyStates.UseLowerHability;
                    }
                    else
                    {
                        currentState = EnemyStates.Attack;
                    }
                }
                else
                {
                    currentState = EnemyStates.Reposition;
                }
                break;


            case EnemyMode.Balanced:
                if (npcHealthPercent > playerHealthPercent)
                {
                    if (habilitiesReady.Count > 1)
                    {
                        currentState = EnemyStates.UseMidHability;
                    }
                    else
                    {
                        currentState = EnemyStates.Attack;
                    }
                }
                if (playerHealthPercent < lifeThreshold1 || playerTurnPercent < turnThreshold2)
                {
                    currentState = EnemyStates.Attack;
                }
                else if (npcHealthPercent < lifeThreshold3)
                {
                    currentState = EnemyStates.Escape;
                }
                else if (npcHealthPercent < lifeThreshold2 || playerTurnPercent > turnThreshold1)
                {
                    currentState = EnemyStates.Reposition;
                }
                else
                {
                    int r2 = Random.Range(1, 4);
                    if (r2 == 1)
                    {
                        currentState = EnemyStates.Attack;
                    }
                    else if (r2 == 2)
                    {
                        currentState = EnemyStates.Reposition;
                    }
                    else
                    {
                        if (habilitiesReady.Count > 0)
                        {
                            currentState = EnemyStates.UseLowerHability;
                        }
                        else
                        {
                            currentState = EnemyStates.Attack;
                        }
                    }
                }
 
                break;


            case EnemyMode.Afraid:
                if (playerTurnPercent < turnThreshold3 && npcHealthPercent > playerHealthPercent)
                {
                    int r1 = Random.Range(1, 4);
                    if (r1 == 1)
                    {
                        currentState = EnemyStates.Attack;
                    }
                    else
                    {
                        if (habilitiesReady.Count > 0)
                        {
                            currentState = EnemyStates.UseLowerHability;
                        }
                        else
                        {
                            currentState = EnemyStates.Attack;
                        }
                    }
                }
                else if (myNPC.myTemplate.EscapeProbability(myNPC, player) > 0)
                {
                    currentState = EnemyStates.Escape;
                }
                else
                {
                    currentState = EnemyStates.Reposition;
                }
                break;
        }

        RealizeAction();
    }

    /// <summary>
    /// Después de Analizar la situación, envía la acción a tomar según su estado actual.
    /// </summary>
    public void RealizeAction()
    {
        switch (currentState)
        {
            case EnemyStates.Attack:
                myNPC.AttackInCombat(player);
                break;

            case EnemyStates.Reposition:
                myNPC.RepositionInCombat();
                break;

            case EnemyStates.Escape:
                myNPC.TryToEscape(player);
                break;

            case EnemyStates.UseLowerHability:
                if (habilitiesReady[0])
                {
                    //myNPC.myTemplate.habilities[0].ImplementHability(myNPC, player); No implementado aún
                }
                else
                {
                    myNPC.AttackInCombat(player);
                }
                break;

            case EnemyStates.UseMidHability:
                if (habilitiesReady[1])
                {
                    //myNPC.myTemplate.habilities[1].ImplementHability(myNPC, player); No implementado aún
                }
                else
                {
                    myNPC.AttackInCombat(player);
                }
                break;

            case EnemyStates.UseHighHability:
                if (habilitiesReady[2])
                {
                    //myNPC.myTemplate.habilities[2].ImplementHability(myNPC, player); No implementado aún
                }
                else
                {
                    myNPC.AttackInCombat(player);
                }
                break;
        }
    }

    /// <summary>
    /// Revisa si sus habilidades están listas para usar.
    /// </summary>
    public void CheckHabilities()
    {
        habilitiesReady.Clear();
        for (int i = 0; i < myNPC.myTemplate.habilities.Length; i++)
        {
            habilitiesReady.Add(myNPC.myTemplate.habilities[i].isAvailable);
        }
    }
}
