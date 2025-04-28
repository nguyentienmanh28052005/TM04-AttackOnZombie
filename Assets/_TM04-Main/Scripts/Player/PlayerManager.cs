using System;
using TMPro;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private TextMeshProUGUI _textHealth;
    [SerializeField] private TextMeshProUGUI _textFood;
    [SerializeField] private TextMeshProUGUI _textWater;

    [SerializeField] private float _playerHealth;
    [SerializeField] private float _timeHealthLoss;
    [SerializeField] private float _playerFood;
    [SerializeField] private float _timeFoodLoss;
    [SerializeField] private float _playerWater;
    [SerializeField] private float _timeWaterLoss;
    [SerializeField] private bool _playerPoisoned = false;

    private float _lastHealthTime;
    private float _lastFoodTime;
    private float _lastWaterTime;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        _lastHealthTime = Time.time;
        _lastFoodTime = Time.time;
        _lastWaterTime = Time.time;
    }

    private void Update()
    {
        UpdateAttribute();
    }

    private void UpdateAttribute()
    {
        if (_lastHealthTime + _timeHealthLoss < Time.time && (_playerFood == 0 || _playerWater == 0))
        {
            if (_playerFood == 0)
            {
                _playerHealth -= 1;
            }
            if (_playerWater == 0)
            {
                _playerHealth -= 1;
            }
            if (_playerPoisoned) 
                _playerHealth -= 1;
            
            SetText(_playerHealth, _playerFood, _playerWater);
            _lastHealthTime = Time.time;
        }

        if (_lastFoodTime + _timeFoodLoss < Time.time && _playerFood > 0)
        {
            _playerFood -= 1;
            SetText(_playerHealth, _playerFood, _playerWater);
            _lastFoodTime = Time.time;
        }
        if (_lastWaterTime + _timeWaterLoss < Time.time && _playerWater > 0)
        {
            _playerWater -= 1;
            SetText(_playerHealth, _playerFood, _playerWater);
            _lastWaterTime = Time.time;
        }
    }
    
    private void SetText(float health, float food, float water)
    {
        _textHealth.text = health.ToString();
        _textFood.text = food.ToString();
        _textWater.text = water.ToString();
    }
}
