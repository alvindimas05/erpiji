using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartBattle : MonoBehaviour
{
    BattleData battleData;
    void Start()
    {
        battleData = BattleData.Instance;
        battleData.playerPanel = GameObject.Find("PlayerDialogPanel");
        battleData.enemyPanel = GameObject.Find("EnemyDialogPanel");

        battleData.textPlayer = GameObject.Find("PlayerText").GetComponent<TextMeshProUGUI>();
        battleData.textEnemy = GameObject.Find("EnemyText").GetComponent<TextMeshProUGUI>();

        string[] txt = File.ReadAllLines(Application.streamingAssetsPath + "/Dialogs.txt");
        GameObject.Find("EnemyName").GetComponent<TextMeshProUGUI>().text = txt[0];
        EnemyData.Instance.losingDialog = txt[1];

        battleData.playerPanel.SetActive(false);
        battleData.enemyPanel.SetActive(false);

        DialogData dialogData = DialogData.Instance;
        List<string> dialogs = new List<string>();
        foreach(string dial in txt)
        {
            if (dial.Contains("0:") || dial.Contains("1:")) dialogs.Add(dial);
            else if (dial.Contains("StartBattle")) break;
        }
        dialogData.beforeDialogs = dialogs.ToArray();
        dialogData.onDialog = true;
        dialogData.onBeforeDialog = true;
    }

    void Update()
    {
        if (battleData.shaking && !battleData.isShaking)
        {
            battleData.isShaking = true;
            battleData.shaking = false;
            StartCoroutine(Blink());
            StartCoroutine(Shake());
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    IEnumerator Shake()
    {
        float curdur = 0f, duration = .4f;
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Shake");
        List<RectTransform> trans = new List<RectTransform>();
        List<Vector2> curvec = new List<Vector2>();

        foreach (GameObject obj in objs)
        {
            trans.Add(obj.GetComponent<RectTransform>());
        }

        foreach (RectTransform tr in trans)
        {
            curvec.Add(tr.anchoredPosition);
        }

        while (curdur < duration)
        {
            float xOff = Random.Range(-0.4f, 0.4f),
                yOff = Random.Range(-0.4f, 0.4f);
            foreach (RectTransform tr in trans)
            {
                int plus = Random.Range(0, 1);
                tr.anchoredPosition += new Vector2(xOff, yOff) * (plus == 1 ? -1 : 1);
            }
            curdur += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].name != "Player")
            {
                trans[i].anchoredPosition = curvec[i];
            }
        }
        battleData.isShaking = false;
    }
    IEnumerator Blink()
    {
        Image img = GameObject.Find("Player").GetComponent<Image>();
        float curdur = 0f, duration = .4f, time = .15f;

        while(curdur < duration)
        {
            img.color -= new Color(0, 0, 0, 255);
            yield return new WaitForSeconds(time);
            img.color += new Color(0, 0, 0, 255);
            yield return new WaitForSeconds(time);
            curdur += time;
        }
    }
}
