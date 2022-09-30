using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [SerializeField] public Text Money;
    [SerializeField] public int MoneyPrice = 100;

    private float _targetMoney;
    private float _currentMoney;

    private void Start()
    {
        _currentMoney = _targetMoney = 0;
    }
    private void FixedUpdate()
    {

        _currentMoney = Mathf.Lerp(_currentMoney, _targetMoney, Time.deltaTime * 10f);

        Money.text = Mathf.RoundToInt(_currentMoney).ToString();
    }

    public void AddMoney()
    {
        _targetMoney += MoneyPrice;
    }

}
