using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using MoreMountains.NiceVibrations;

public class CashRegisterController : MonoSingleton<CashRegisterController>
{
    public List<CashLocController> cashLocations;
    public GameObject dolar_100, dolar_50, dolar_20, dolar_10, dolar_5, cent_50, cent_25, cent_10;
    public int minCount;
    public float change;
    public CashLocController lastCheckedLocation;
    public TextMeshPro endingScore;
    public float total;
    public float highScore;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI highScoreCount;
    public ParticleSystem confetti;
    public GameObject indicator;
    public float randomNumber;

    public float numberOf100, numberOf50, numberOf20, numberOf10, numberOf5, numberOf50Cent, numberOf25Cent, numberOf10Cent;
    public GameObject handLeft;
    public GameObject handRight;

    public TextMeshProUGUI dontHaveChangeText;
    public int Dolar100, Dolar50, Dolar20, Dolar10, Dolar5, Cent50, Cent25, Cent10;
    
    private void Start()
    {
        //totalMoneyText.text = total.ToString();
        SetValues();
        dontHaveChangeText.gameObject.SetActive(false);

        SpawnMoney();
        GetHighScore();
    }
    private void Update()
    {
        endingScore.text = randomNumber.ToString("0.00");

    }
    public void OnTriggerEnter(Collider obj)
    {
        if (obj.TryGetComponent(out Collectable collectable))
        {
            if (!collectable.isCollected)
            {
                Placement(collectable.value);
                collectable.DisableTween();
                Destroy(obj.gameObject);
                //haptic low
                MMVibrationManager.Haptic(HapticTypes.LightImpact);
            }
        }
        if (obj.TryGetComponent(out DoorController door))
        {
            if (!door.isBlocked)
            {
                //haptic medium
                MMVibrationManager.Haptic(HapticTypes.MediumImpact);

                door.otherDoor.isBlocked = true;
                door.isBlocked = true;
                change = door.change;
                //CheckLowestCount(change, door);
                if (CheckTotal())
                {
                    Calculation(change);


                    if (CheckRegisterMoney())
                    {
                        door.GiveMoney();
                        Calculate();
                    }
                    else
                    {
                        ShowText();
                    }
                }
                else
                {
                    ShowText();

                }
            }
        }
    }
    public void SetHighScore()
    {

        if (total >= highScore)
        {
            highScore = total;
            highScoreCount.text = highScore.ToString("0.00");

            PlayerPrefs.SetFloat("HighScore", highScore);
            //play confetti
            confetti.Play();

        }
    }
    public void GetHighScore()
    {
        highScore = PlayerPrefs.GetFloat("HighScore", highScore);
        highScoreCount.text = highScore.ToString("0.00");
    }
    public void ShowHighscore()
    {
        highScoreText.gameObject.SetActive(true);

    }
    public void Calculation(float change)
    {
        int count100, count50, count20, count10, count5, count50cent, count25cent, count10cent;
        count100 = cashLocations[0].GetCount();
        count50 = cashLocations[1].GetCount();
        count20 = cashLocations[2].GetCount();
        count10 = cashLocations[3].GetCount();
        count5 = cashLocations[4].GetCount();
        count50cent = cashLocations[5].GetCount();
        count25cent = cashLocations[6].GetCount();
        count10cent = cashLocations[7].GetCount();

        if (change >= 100)
        {
            //numberOf100 = tempNumber /= 100;
            //tempNumber = tempNumber /= 100;
            for (int i = 0; i < count100; i++)
            {
                numberOf100++;
                change -= 100;
            }
        }
        if (change >=50)
        {
            //numberOf50 = tempNumber /= 50;
            //tempNumber = tempNumber /= 50;
            for (int i = 0; i < count50; i++)
            {
                numberOf50++;
                change -= 50;
            }
        }
        if (change >=20)
        {
            //numberOf20 = tempNumber /= 20;
            //tempNumber = tempNumber /= 20;
            for (int i = 0; i < count20; i++)
            {
                numberOf20++;
                change -= 20;
            }
        }
        if (change >=10)
        {
            //numberOf10 = tempNumber /= 10;
            //tempNumber = tempNumber /= 10;
            for (int i = 0; i < count10; i++)
            {
                numberOf10++;
                change -= 10;

            }
        }
        if (change >=5)
        {
            //numberOf5 = tempNumber /= 5;
            //tempNumber = tempNumber /= 5;
            for (int i = 0; i < count5; i++)
            {
                numberOf5++;
                change -= 5;

            }
        }
        if (change >= 0.50f)
        {
            //numberOf50Cent = tempNumber /= 0.50f;
            //tempNumber = tempNumber /= 0.50f;
            for (int i = 0; i < count50cent; i++)
            {
                numberOf50Cent++;
                change -= 0.50f;

            }
        }
        if (change >= 0.25f)
        {
            //numberOf25Cent = tempNumber /= 0.25f;
            //tempNumber = tempNumber /= 0.25f;
            for (int i = 0; i < count25cent; i++)
            {
                numberOf25Cent++;
                change -= 0.25f;

            }
        }
        if (change >= 0.10f)
        {
            //numberOf10Cent = tempNumber % 0.10f;
            for (int i = 0; i < count10cent; i++)
            {
                numberOf10Cent++;
                change -= 0.10f;

            }
        }
    }
    public void Calculate()
    {
        while (change != 0)
        {
            if (change - 100 >= 0 && cashLocations[0].count > 0)
            {
                Displacement(100);
                change -= 100;
                Debug.Log("100 dolar verildi");

            }
            else if (change - 50 >= 0 && cashLocations[1].count > 0)
            {
                Displacement(50);
                change -= 50;
                Debug.Log("50 dolar verildi");

            }
            else if (change - 20 >= 0 && cashLocations[2].count > 0)
            {
                Displacement(20);
                change -= 20;
                Debug.Log("20 dolar verildi");

            }
            else if (change - 10 >= 0 && cashLocations[3].count > 0)
            {
                Displacement(10);
                change -= 10;
                Debug.Log("10 dolar verildi");

            }
            else if (change - 5 >= 0 && cashLocations[4].count > 0)
            {
                Displacement(5);
                change -= 5;
                Debug.Log("5 dolar verildi");

            }
            else if (change - 0.50f >= 0 && cashLocations[5].count > 0)
            {
                Displacement(0.50f);
                change -= 0.50f;
                Debug.Log("0.50f cent verildi");

            }
            else if (change - 0.25f >= 0 && cashLocations[6].count > 0)
            {
                Displacement(0.25f);
                change -= 0.25f;
                Debug.Log("0.25f cent verildi");

            }
            else if (change - 0.10f >= 0 && cashLocations[7].count > 0)
            {
                Displacement(0.10f);
                change -= 0.10f;
                Debug.Log("0.10f cent verildi");

            }
            endingScore.text = randomNumber.ToString("0.00");

        }
        ResetCounts();
    }
    public void ResetCounts()
    {
        numberOf100 = 0; numberOf50 = 0; numberOf20 = 0; numberOf10 = 0; numberOf5 = 0; numberOf50Cent = 0; numberOf25Cent = 0; numberOf10Cent = 0;
    }
    public void SetValues()
    {
        cashLocations[0].SetValue(100);
        cashLocations[1].SetValue(50);
        cashLocations[2].SetValue(20);
        cashLocations[3].SetValue(10);
        cashLocations[4].SetValue(5);
        cashLocations[5].SetValue(0.50f);
        cashLocations[6].SetValue(0.25f);
        cashLocations[7].SetValue(0.10f);
    }
    public void Displacement(float value)
    {
        switch (value)
        {
            case 100:
                cashLocations[0].RemoveMoney(1);

                break;
            case 50:
                cashLocations[1].RemoveMoney(1);

                break;
            case 20:
                cashLocations[2].RemoveMoney(1);

                break;
            case 10:
                cashLocations[3].RemoveMoney(1);

                break;
            case 5:
                cashLocations[4].RemoveMoney(1);

                break;
            case 0.50f:
                cashLocations[5].RemoveMoney(1);

                break;
            case 0.25f:
                cashLocations[6].RemoveMoney(1);

                break;
            case 0.10f:
                cashLocations[7].RemoveMoney(1);

                break;

            default:
                break;

        }
    }
    public void Placement(float value) 
    {
        switch (value)
        {
            case 100:
                cashLocations[0].AddMoney(dolar_100);

                break;
            case 50:
                cashLocations[1].AddMoney(dolar_50);

                break;
            case 20:
                cashLocations[2].AddMoney(dolar_20);

                break;
            case 10:
                cashLocations[3].AddMoney(dolar_10);

                break;
            case 5:
                cashLocations[4].AddMoney(dolar_5);

                break;
            case 0.50f:
                cashLocations[5].AddMoney(cent_50);

                break;
            case 0.25f:
                cashLocations[6].AddMoney(cent_25);

                break;
            case 0.10f:
                cashLocations[7].AddMoney(cent_10);

                break;

            default:
                break;

        }
    }
    public void AddMoney(float amount)
    {
        total += amount;
    }    
    public void RemoveMoney(float amount)
    {
        total -= amount;
    }

    public void ShowIndicator()
    {

        Vector3 tempPos = new Vector3(indicator.transform.position.x, 1.35f, 2);
        indicator.transform.DOLocalMove(tempPos, 1);
    }
    public void StartCounting()
    {
        //randomNumber = Random.Range(100,9999);
        DOTween.To(() => randomNumber, x => randomNumber = x, GetTotal(), 4).OnComplete(WaitToEnd);

    }
    public void SetTotal()
    {
        endingScore.text = total.ToString();
    }
    public float GetTotal()
    {
        return total;
    }
    public bool CheckTotal()
    {
        if (change <= total)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void WaitForDestroy()
    {
        IEnumerator Wait()
        {
            for (int i = cashLocations.Count - 1; i > -1; i--)
            {
                cashLocations[i].DestroyStack();
                yield return new WaitForSeconds(0.4f);
            }
        }

        StartCoroutine(Wait());
    }
    public bool CheckRegisterMoney()
    {
        if (numberOf100 > 0 && cashLocations[0].count < numberOf100)
        {
            return false;
        }
        

        if (numberOf50 > 0 && cashLocations[1].count < numberOf50)
        {
            return false;
        }
        

        if (numberOf20 > 0 && cashLocations[2].count < numberOf20)
        {
            return false;
        }
        

        if (numberOf10 > 0 && cashLocations[3].count < numberOf10)
        {
            return false;
        }
        

        if (numberOf5 > 0 && cashLocations[4].count < numberOf5)
        {
            return false;
        }
        

        if (numberOf50Cent > 0 && cashLocations[5].count < numberOf50Cent)
        {
            return false;
        }
        

        if (numberOf25Cent > 0 && cashLocations[6].count < numberOf25Cent)
        {
            return false;
        }
        

        if (numberOf10Cent > 0 && cashLocations[7].count < numberOf10Cent)
        {
            return false;
        }
        

        return true;
    }
    public void DisableAllCounts()
    {
        foreach (CashLocController loc in cashLocations)
        {
            loc.DisableCountText();
        }
    }
    public void DisableHands()
    {
        handLeft.SetActive(false);
        handRight.SetActive(false);
    }
    public void ChangeStateOfAllLocations()
    {
        foreach (CashLocController loc in cashLocations)
        {
            loc.ChangeState();
        }
    }
    public void ShowText()
    {
        IEnumerator Show()
        {
            dontHaveChangeText.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            dontHaveChangeText.gameObject.SetActive(false);

        }

        StartCoroutine(Show());

    }
    public void SpawnMoney()
    {
        if (Dolar100 > 0)
        {
            for (int i = 0; i < Dolar100; i++)
            {

            }
            Placement(100);
        }
        if (Dolar50 > 0)
        {
            for (int i = 0; i < Dolar50; i++)
            {
                Placement(50);

            }
        }
        if (Dolar20 > 0)
        {
            for (int i = 0; i < Dolar20; i++)
            {
                Placement(20);

            }
        }
        if (Dolar10 > 0)
        {
            for (int i = 0; i < Dolar10; i++)
            {
                Placement(10);

            }
        }
        if (Dolar5 > 0)
        {
            for (int i = 0; i < Dolar5; i++)
            {
                Placement(5);

            }
        }
        if (Cent50 > 0)
        {
            for (int i = 0; i < Cent50; i++)
            {
                Placement(0.50f);

            }
        }
        if (Cent25 > 0)
        {
            for (int i = 0; i < Cent25; i++)
            {
                Placement(0.25f);

            }
        }
        if (Cent10 > 0)
        {
            for (int i = 0; i < Cent10; i++)
            {
                Placement(0.10f);

            }
        }
    }
    public void WaitToEnd()
    {
        IEnumerator Wait()
        {
            SetHighScore();
            ShowHighscore();
            yield return new WaitForSeconds(4);
            GameManager.instance.EndGame(true);

        }

        StartCoroutine(Wait());
    }
    
}
