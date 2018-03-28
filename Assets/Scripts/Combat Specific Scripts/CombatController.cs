using UnityEngine;

/// <summary>
/// Controlador de todo lo que haría el gamecontroller durante el combate.
/// </summary>
public class CombatController : MonoBehaviour {

    public GameObject combatLayout;
    public RectTransform contentContainer;
    [Space(10)]
    public PlayerUIDuringCombat playerUI;
    public EnemyUIDuringCombat enemyUI;

    private EnemyNPC enemy;
    private PlayerManager player;

    public NPCTemplate TryToFight(string keywordGiven, Room currentRoom)
    {
        for (int i = 0; i < currentRoom.npcsInRoom.Count; i++)
        {
            NPCTemplate npc = currentRoom.npcsInRoom[i];

            if (npc.GetType() == typeof(EnemyNPC))
            {
                foreach (string keyword in npc.keyword)
                {
                    if (keyword == keywordGiven)
                    {
                        return npc;
                    }
                }
            }
        }

        return null;
    }


    public void PrepareFight(NPCTemplate npc, PlayerManager thisPlayer)
    {
        enemy = npc as EnemyNPC;
        player = thisPlayer;

        GameState.Instance.ChangeCurrentState(GameState.GameStates.combat);
    }

    public void StartFight()
    {
        ChangeLayout();
    }

    private void ChangeLayout()
    {
        GameObject newCombat = Instantiate(combatLayout, contentContainer);

        playerUI.InstantiateMyStuff(newCombat.GetComponent<RectTransform>());
        InitializePlayer();
        enemyUI.InstantiateMyStuff(newCombat.GetComponent<RectTransform>());
        InitializeEnemy();
    }

    private void InitializePlayer()
    {
        playerUI.title.text = player.playerName + "\n" +
            player.characteristics.playerJob.jobName +
            " <Aun nada>";

        playerUI.lifeSlider.maxValue = player.MaxHealth;
        playerUI.lifeSlider.value = player.currentHealth;
        playerUI.lifeText.text = ((player.currentHealth / player.MaxHealth) * 100).ToString() + "%";

        playerUI.turnSlider.maxValue = player.MaxTurn;
        playerUI.turnSlider.value = player.currentTurn;

        playerUI.willText.text = "V[" + player.currentWill + "/" + player.MaxWill + "]";

        playerUI.habilitiesText.text = "[0] Atacar";

        playerUI.optionsText.text = "[1] Inventario \n" +
            "[2] Reposicionamiento \n" +
            "[3] Escapar (" + player.characteristics.other.EscapeProbability() + "%)";

        playerUI.logText.text = "";

    }

    private void InitializeEnemy()
    {
        enemyUI.title.text = enemy.npcName + "\n" +
            enemy.npcJob.jobName +
            " <Aun nada>";

        enemyUI.lifeSlider.maxValue = enemy.MaxHealth;
        enemyUI.lifeSlider.value = enemy.currentHealth;
        enemyUI.lifeText.text = ((enemy.currentHealth / enemy.MaxHealth) * 100).ToString() + "%";

        enemyUI.turnSlider.maxValue = enemy.MaxTurn;
        enemyUI.turnSlider.value = enemy.currentTurn;

        enemyUI.descriptionText.text = enemy.npcDetailedDescription;

        enemyUI.logText.text = "";
    }
}
