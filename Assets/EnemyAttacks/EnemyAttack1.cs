using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack1 : MonoBehaviour
{
    bool isLeft, isClone;
    float attackSpeed;
    
    void Start()
    {
        isClone = gameObject.name.Contains("Clone");
        isLeft = gameObject.name.Contains("Left");
        attackSpeed = EnemyData.Instance.attackSpeed1;
    }

    void Update()
    {
        if (isClone)
        {
            RectTransform rect = gameObject.GetComponent<RectTransform>();
            float xpos = rect.offsetMin.x;
            if ((xpos > 1080 && !isLeft) || (xpos < (rect.sizeDelta.x * -1) && isLeft))
            {
                Destroy(gameObject);
            }
            gameObject.transform.position += new Vector3(isLeft ? -1 : 1, 0) * attackSpeed * Time.deltaTime;
        } 
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            BattleData battleData = BattleData.Instance;
            if (!battleData.shaking && !battleData.isShaking)
            battleData.shaking = true;

            EnemyData enemyData = EnemyData.Instance;
            PlayerData playerData = PlayerData.Instance;

            RectTransform rect = GameObject.Find("PlayerHP").GetComponent<RectTransform>();

            playerData.healthPoints -= enemyData.damage;
            rect.sizeDelta = new Vector2((playerData.healthPoints / playerData.maxHealthPoints) * 100, rect.sizeDelta.y);
            GameObject.Find("Attacked").GetComponent<AudioSource>().Play();
        }
    }
}
