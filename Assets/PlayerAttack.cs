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
            img.color += new Color(0, 0, 0, curspeed / 100);
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
            img.color -= new Color(0, 0, 0, curspeed / 100);
            return;
        }
        Destroy(gameObject);
    }
}
