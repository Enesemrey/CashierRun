using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoorController : MonoBehaviour
{
    public float customersMoney;
    public float itemPrice;
    public float change;

    public TextMeshPro itemText;
    public TextMeshPro moneyText;
    public List<float> values;
    public float checkSum;
    public int customerID;
    public List<GameObject> customers;
    public List<GameObject> carrierBags;
    public bool isBlocked;
    public DoorController otherDoor;
    private void Start()
    {
        itemText.text = itemPrice.ToString("0.00" + "$");
        moneyText.text = customersMoney.ToString("0.00" + "$");
        change = customersMoney - itemPrice;
        //ActivateCustomer(customerID);
        ActivateRandom();
        CheckSum();

    }
    public void CheckSum()
    {
        foreach (float value in values)
        {
            checkSum += value;
        }
    }
    public void ActivateCustomer(int id)
    {
        if (id > 0 && id <= 4)
        {
            customers[id - 1].SetActive(true);
        }
    }
    public void ActivateRandom()
    {
        int tempNumber = Random.Range(0,12);
        carrierBags[tempNumber].SetActive(true);
    }
    public float GetChange()
    {
        return change;
    }

    public void GiveMoney()
    {
        foreach (float value in values)
        {
            CashRegisterController.instance.Placement(value);
        }
    }
}
