using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    bool started = false, attacking = false;

    void Update()
    {

        if (BattleData.Instance.started && !started)
        {
            started = true;
            StartCoroutine(Battle());
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "PlayerAttack")
        {
            RectTransform rect = GameObject.Find("EnemyHP").GetComponent<RectTransform>();
            EnemyData enemyData = EnemyData.Instance;
            enemyData.healthPoints -= PlayerData.Instance.damage;

            rect.sizeDelta = new Vector2(enemyData.healthPoints / enemyData.maxHealthPoints * 100, rect.sizeDelta.y);
            GameObject.Find("Attacking").GetComponent<AudioSource>().Play();
            StartCoroutine(Shake());
            StartCoroutine(Blink());
        }
    }

    IEnumerator Shake()
    {
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        Vector2 curvec = rect.anchoredPosition;
        float curdur = 0f, duration = .5f;
        while (curdur < duration)
        {
            float xOff = Random.Range(-0.4f, 0.4f),
                yOff = Random.Range(-0.4f, 0.4f);

            int plus = Random.Range(0, 1);
            rect.anchoredPosition += new Vector2(xOff, yOff) * (plus == 1 ? -1 : 1);

            curdur += Time.deltaTime;
            yield return null;
        }
        rect.anchoredPosition = curvec;
    }
    IEnumerator Blink()
    {
        Image img = gameObject.GetComponent<Image>();
        float curdur = 0f, duration = 1f, time = .15f;

        while (curdur < duration)
        {
            img.color -= new Color(0, 0, 0, 255);
            yield return new WaitForSeconds(time);
            img.color += new Color(0, 0, 0, 255);
            yield return new WaitForSeconds(time);
            curdur += time;
        }
    }

    IEnumerator Battle()
    {
        DialogData dialogData = DialogData.Instance;
        int duration = 0;
        bool foundStart = false;
        string[] txt = File.ReadAllLines(Application.streamingAssetsPath + "/Dialogs.txt");
        for (int i = 0; i < txt.Length; i++)
        {
            while (dialogData.onDialog || dialogData.onBattleDialog) yield return null;
            if (foundStart)
            {
                float d;
                if (float.TryParse(txt[i], out d))
                {
                    attacking = true;
                    yield return Attack(d);
                    i++;
                    dialogData.battleDialog = txt[i];
                    dialogData.onBattleDialog = true;
                }
                else
                {
                    dialogData.battleDialog = txt[i];
                    dialogData.onBattleDialog = true;
                    Debug.Log("Stopped!");
                    yield break;
                }
            }
            else if (txt[i].Contains("StartBattle") && !foundStart)
            {
                foundStart = true;
                if (txt[i] == "StartBattleNonStop")
                {
                    int.TryParse(txt[i + 1], out duration);
                    break;
                }
            }
        }

        while (EnemyData.Instance.healthPoints > 0 && PlayerData.Instance.healthPoints > 0)
        {
            attacking = true;
            yield return Attack(duration);
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator Attack(float duration)
    {
        int right = Random.Range(0, 2);
        StartCoroutine(Wait(duration));
        while (attacking)
        {
            GameObject currentObj = GameObject.Instantiate(GameObject.Find("EnemyAttack" + (right == 1 ? "Right" : "Left") +"1"));
            RectTransform currentRect = currentObj.GetComponent<RectTransform>();
            float height = GameObject.Find("BattleCanvas").GetComponent<RectTransform>().sizeDelta.y;
            Debug.Log(height);

            currentObj.transform.SetParent(GameObject.Find("EnemyAttacks").transform, false);
            float rand = Random.Range((height - (currentRect.sizeDelta.y / 2)) * -1, currentRect.sizeDelta.y / 2 * -1);
            currentRect.anchoredPosition = new Vector2(currentRect.anchoredPosition.x, (float)rand);

            yield return new WaitForSeconds(.5f);
        }
    }
    IEnumerator Wait(float duration)
    {
        yield return new WaitForSeconds(duration);
        attacking = false;
    }
}
