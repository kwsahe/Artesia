using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DmgText : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float alphaSpeed;
    [SerializeField] float destroyTime;

    public Vector3 parentPosition;
    public int Damage;

    TextMeshPro text;
    Color alpha;

    public void Init(int dmg, Vector3 parent)
    {
        text = GetComponent<TextMeshPro>();
        alpha = text.color;
        Invoke("destroyObject", destroyTime);
        text.text = dmg.ToString();
        transform.position = parent + Vector3.up;

        StartCoroutine("UpdateObj");
    }

    IEnumerator UpdateObj()
    {
        float elapsedTime = 0f;
        while (elapsedTime < destroyTime)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up, moveSpeed * Time.deltaTime);
            alpha.a = Mathf.Lerp(alpha.a, 0, alphaSpeed * Time.deltaTime);
            text.color = alpha;
            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }

    void destroyObject()
    {
        Destroy(gameObject);
    }
}
