using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    bool started = false, attacking = false;
    Rigidbody2D rb;
    void Update()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        if (BattleData.Instance.started && !started)
        {
            started = true;
            StartCoroutine(Battle());
        }

    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            Vector3 vec = gameObject.transform.position - collision.gameObject.transform.position;
            rb.velocity = vec.normalized * EnemyData.Instance.walkSpeed;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        rb.velocity = Vector2.zero;
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
