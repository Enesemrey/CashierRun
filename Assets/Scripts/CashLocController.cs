using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class CashLocController : MonoBehaviour
{
    public int count;
    public TextMeshPro text;
    public List<GameObject> moneyStack;
    public GameObject lastSpawnedObject;
    public float value;
    public float total;

    public Vector3 targetPos;
    private void Start()
    {
        targetPos = new Vector3(0,0.5f,0);
    }
    public void AddMoney(GameObject spawnObject)
    {

        GameObject tempObject = Instantiate(spawnObject, transform.position, Quaternion.Euler(0, 0, 0));

        if (lastSpawnedObject != spawnObject || lastSpawnedObject == null)
        {
            lastSpawnedObject = tempObject;
        }
        Collectable collectable = tempObject.GetComponent<Collectable>();
        collectable.isCollected = true;
        moneyStack.Add(lastSpawnedObject);
        count++;
        EditCountText();
        if (moneyStack.Count > 1)
        {
            collectable.connectedNode = moneyStack[count - 2];
            if (collectable.isCent)
            {
                collectable.connectedNode = gameObject;
            }
        }
        else if (moneyStack.Count == 1)
        {
            collectable.connectedNode = gameObject;
        }
        MoneyAdded();
        WaitForScale();
    }
    public void RemoveMoney(float times)
    {
        for (int i = 0; i < times; i++)
        {
            GameObject tempObject = moneyStack[moneyStack.Count - 1];
            moneyStack.Remove(tempObject);
            Destroy(tempObject);
            count--;
            MoneyRemoved();

        }
        EditCountText();

    }

    public void MoneyAdded()
    {
        CashRegisterController.instance.AddMoney(value);
    }
    public void MoneyRemoved()
    {
        CashRegisterController.instance.RemoveMoney(value);
    }
    public void SetValue(float tempValue)
    {
        value = tempValue;
    }
    public void EditCountText()
    {
        text.text = count.ToString();
    }
    public void DisableCountText()
    {
        text.gameObject.SetActive(false);

    }
    public void ChangeState()
    {
        foreach (GameObject money in moneyStack)
        {
            money.GetComponent<Collectable>().atEndingState = true;
        }
    }
    public void WaitForScale()
    {
        IEnumerator Wait()
        {
            //moneyStack[0].GetComponent<Collectable>().DoMove();
            //DoMove();
            for (int i = 0; i < moneyStack.Count; i++)
            {
                GameObject money = moneyStack[i];
                yield return money.GetComponent<Collectable>().DoScale();
            }
        }

        StartCoroutine(Wait());
    }
    public int GetCount()
    {
        return count;
    }
    public CashLocController GetLoc()
    {
        return this;
    }
    public float GetValue()
    {
        return value;
    }
    public void DestroyStack()
    {
        IEnumerator Wait()
        {

            if (moneyStack.Count != 0)
            {
                for (int i = 0; i < count; i++)
                {
                    GameObject tempObject = moneyStack[moneyStack.Count - 1];
                    moneyStack.Remove(tempObject);
                    Destroy(tempObject);
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }

        StartCoroutine(Wait());
   
    }
    public void DoMove()
    {
        IEnumerator MoveOverTime()
        {
                transform.DOLocalMove(transform.localPosition + targetPos, 0.08f).SetEase(Ease.InOutSine);
                yield return new WaitForSeconds(0.08f);


                transform.DOLocalMove(transform.localPosition - targetPos, 0.08f).SetEase(Ease.InOutSine);

        }
        StartCoroutine(MoveOverTime());
    }
}
