using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    Rigidbody2D rb;
    PlayerData playerData;
    RectTransform cooldown;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        cooldown = GameObject.Find("Cooldown").GetComponent<RectTransform>();
        playerData = PlayerData.Instance;
    }

    void Update()
    {
        if (playerData.canWalk && !playerData.winning)
        {
            rb.velocity = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * PlayerData.Instance.walkSpeed;
        } else
        {
            rb.velocity = new Vector3(0, 0);
        }

        if (playerData.canWalk && !playerData.isCooldown && Input.GetKeyDown(KeyCode.E))
        {
            playerData.isCooldown = true;
            GameObject atk = Instantiate(Resources.Load("PlayerAttack")) as GameObject;
            RectTransform rect = atk.GetComponent<RectTransform>();
            atk.name = "PlayerAttack";
            atk.transform.SetParent(GameObject.Find("BattleCanvas").transform);
            rect.localScale = new Vector2(1, 1);
            rect.anchoredPosition = new Vector2(100, -170);

            cooldown.sizeDelta = new Vector2(1000, cooldown.sizeDelta.y);
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown()
    {
        float cur = playerData.cooldown;
        while(cur > 0f)
        {
            cooldown.sizeDelta = new Vector2(cur / playerData.cooldown * 1000, cooldown.sizeDelta.y);
            cur -= Time.deltaTime;
            yield return null;
        }
        playerData.isCooldown = false;
    }
}
