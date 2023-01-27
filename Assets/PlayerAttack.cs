using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    RectTransform rect;
    bool damaged = false;
    float speed, curspeed, wait1 = 0f, wait2 = 0f;
    void Start()
    {
        Physics2D.IgnoreCollision(GameObject.Find("Player").GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
        rect = gameObject.GetComponent<RectTransform>();
        speed = PlayerData.Instance.attackSpeed;
        curspeed = speed;
    }
    void Update()
    {
        float ypos = rect.anchoredPosition.y;
        Image img = gameObject.GetComponent<Image>();
        if(ypos < -80 && !damaged)
        {
            gameObject.transform.position += new Vector3(0, 1) * curspeed * Time.deltaTime;
            img.color += new Color(0, 0, 0, curspeed / 50);
            return;
        }

        if(wait1 < .75f)
        {
            wait1 += Time.deltaTime;
            return;
        }

        float xpos = rect.anchoredPosition.x;
        if (xpos > -100)
        {
            speed += .2f;
            gameObject.transform.position -= new Vector3(1, 0) * speed * Time.deltaTime;
            return;
        }

        damaged = true;

        if (wait2 < .75f)
        {
            wait2 += Time.deltaTime;
            return;
        }

        if (ypos > -170)
        {
            gameObject.transform.position -= new Vector3(0, 1) * curspeed * Time.deltaTime;
            img.color -= new Color(0, 0, 0, curspeed / 25);
            return;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Enemy")
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
        RectTransform rect = GameObject.Find("Enemy").GetComponent<RectTransform>();
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
    }
    IEnumerator Blink()
    {
        Image img = GameObject.Find("Enemy").GetComponent<Image>();
        float curdur = 0f, duration = 1f, time = .15f;

        while (curdur < duration)
        {
            img.color -= new Color(0, 0, 0, 255);
            yield return new WaitForSeconds(time);
            img.color += new Color(0, 0, 0, 255);
            yield return new WaitForSeconds(time);
            curdur += time;
        }
        Destroy(gameObject);
    }
}
