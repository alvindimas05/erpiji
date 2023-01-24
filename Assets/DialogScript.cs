using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogScript : MonoBehaviour
{
    DialogData dialogData;
    BattleData battleData;

    GameObject playerPanel, enemyPanel, currentPanel;
    TextMeshProUGUI textPlayer, textEnemy, currentText;
    IEnumerator coroutine;

    bool isPlayer = false, beforeStarted = false;
    string dialog;
    void Start()
    {
        dialogData = DialogData.Instance;
        battleData = BattleData.Instance;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (dialogData.onBeforeDialog)
            {
                if(dialogData.onDialog) StopDialog();
                else if (dialogData.beforeIndex >= dialogData.beforeDialogs.Length)
                {
                    StopDialog();
                    currentPanel.SetActive(false);
                    dialogData.onBeforeDialog = false;
                    battleData.started = true;
                    PlayerData.Instance.canWalk = true;
                }
                else StartDialog();
            }
        } else if (dialogData.onBeforeDialog && !beforeStarted)
        {
            beforeStarted = true;
            playerPanel = battleData.playerPanel;
            enemyPanel = battleData.enemyPanel;

            textPlayer = battleData.textPlayer;
            textEnemy = battleData.textEnemy;
            StartDialog();
        } else if (dialogData.onBattleDialog && !dialogData.onDialog)
        {
            dialogData.onDialog = true;
            StartDialog();
        }
    }
    void StartDialog()
    {
        if (dialogData.onBeforeDialog)
        {
            dialog = dialogData.beforeDialogs[dialogData.beforeIndex];
            string[] arr = dialog.Split(":");
            isPlayer = arr[0] == "1";
            dialog = arr[1];
        } else if (dialogData.onBattleDialog)
        {
            isPlayer = false;
            dialog = dialogData.battleDialog;
        }

        if (currentPanel) currentPanel.SetActive(false);
        textPlayer.text = "";
        textEnemy.text = "";

        currentPanel = isPlayer ? playerPanel : enemyPanel;
        currentPanel.SetActive(true);
        dialogData.onDialog = true;

        coroutine = Dialog();
        StartCoroutine(coroutine);
    }

    void StopDialog()
    {
        StopCoroutine(coroutine);
        currentText.text = dialog;
        dialogData.onDialog = false;
        dialogData.beforeIndex++;
    }

    IEnumerator Dialog()
    {
        currentText = isPlayer ? textPlayer : textEnemy;
        for (int j = 0; j < dialog.Length; j++)
        {
            yield return new WaitForSeconds(.1f);
            currentText.text += dialog[j];
        }
        if (dialogData.onBeforeDialog) {
            dialogData.beforeIndex++;
        } else if(dialogData.onBattleDialog) {
            yield return new WaitForSeconds(1f);
            currentPanel.SetActive(false);
            dialogData.onBattleDialog = false;
        }
        dialogData.onDialog = false;
    }
}
