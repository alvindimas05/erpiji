using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleData : Singleton<BattleData>
{
    public GameObject playerPanel, enemyPanel;
    public TextMeshProUGUI textPlayer, textEnemy;
    public bool started = false, shaking = false, isShaking = false;
}

public class DialogData : Singleton<DialogData>
{
    public int beforeIndex = 0;
    public string[] beforeDialogs;
    public string battleDialog;
    public bool onDialog = false, onBeforeDialog = false, onBattleDialog = false;
}

public class EnemyData : Singleton<EnemyData>
{
    public float maxHealthPoints = 100,
        healthPoints = 100,
        attackSpeed1 = 5,
        walkSpeed = 7,
        damage = 1;
    public string losingDialog;
}

public class PlayerData : Singleton<PlayerData>
{
    public float maxHealthPoints = 100,
        healthPoints = 100,
        damage = 10,
        cooldown = 5f,
        attackSpeed = 10,
        walkSpeed = 8;
    public bool canWalk = false, winning = false, isCooldown = false;
}