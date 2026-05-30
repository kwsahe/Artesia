using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    private bool isAttacking = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Potion"))
        {
            gameObject.GetComponent<PlayerStat>().addHP(other.GetComponent<PotionStat>().HP);
            Destroy(other.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!isAttacking && gameObject.GetComponent<Collider2D>().bounds.center == other.transform.position)
            {
                isAttacking = true;
                gameObject.GetComponent<PlayerStat>().TakeDamage(other.GetComponent<MobStat>().ATK);
                gameObject.GetComponent<PlayerController>().DirControl(other.GetComponent<MobController>().OriPos - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y));
                StartCoroutine(ResetAttackAfterDelay());
            }
        }
    }

    IEnumerator ResetAttackAfterDelay()
    {
        yield return new WaitForSeconds(0.5f); // 공격 후 0.5초 대기
        isAttacking = false;
    }
}