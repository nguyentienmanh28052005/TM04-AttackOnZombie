
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>, IMessageHandle
{
    [SerializeField] private InventorySlot _mainGunSlot;
    [SerializeField] private InventorySlot _miniGunSlot;
    
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

    public ManhMessageType _currentWeaponType = ManhMessageType.nullWeapon;
    

    [SerializeField] private List<GameObject> _weaponList;
    private Dictionary<Weapon, GameObject> _weapons = new Dictionary<Weapon, GameObject>();
        
    public enum Weapon
    {
        Sniper1,
        AR4,
        nullWeapon,
    }

    protected override void Awake()
    {
        base.Awake();
        _weapons.Add(Weapon.Sniper1, _weaponList[0]);
        _weapons.Add(Weapon.AR4, _weaponList[1]);
    }

    private void Start()
    {
        MessageManager.Instance.AddSubcriber(ManhMessageType.EquipSniper1, this);
        MessageManager.Instance.AddSubcriber(ManhMessageType.EquipAR4, this);
        MessageManager.Instance.AddSubcriber(ManhMessageType.nullWeapon, this);
        _lastHealthTime = Time.time;
        _lastFoodTime = Time.time;
        _lastWaterTime = Time.time;
    }

    private void Update()
    {
        //if (_weaponList[0] != null) Debug.Log(_weaponList[0].name);
        UpdateAttribute();
        // if(_playerHealth < 0) 
    }

    private void EquipWeapon(Weapon weapon)
    {
        if(weapon == Weapon.nullWeapon) Debug.Log("asdasda");
        foreach (var item in _weapons)
        {
            if(item.Key == weapon) item.Value.SetActive(true);
            else
            {
                item.Value.SetActive(false);
            }
        }
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

    public float GetHealth()
    {
        return _playerHealth;
    }

    public void SetHealth(float newHealth)
    {
        _playerHealth = newHealth;
        SetText(_playerHealth, _playerFood, _playerWater);
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        MessageManager.Instance.RemoveSubcriber(ManhMessageType.EquipSniper1,this);
        MessageManager.Instance.RemoveSubcriber(ManhMessageType.EquipAR4,this);
        MessageManager.Instance.RemoveSubcriber(ManhMessageType.nullWeapon,this);
    }

    public void Handle(Message message)
    {
        switch (message.type)
        {
            case ManhMessageType.EquipSniper1:
                Debug.Log("Sniper1");
                _currentWeaponType = ManhMessageType.EquipSniper1;
                EquipWeapon(Weapon.Sniper1);
                break;
            
            case ManhMessageType.EquipAR4:
                Debug.Log("AR4");
                _currentWeaponType = ManhMessageType.EquipAR4;
                EquipWeapon(Weapon.AR4);
                break;
            
            case ManhMessageType.nullWeapon:
                Debug.Log("null");
                //_currentWeaponType = ManhMessageType.nullWeapon;
                EquipWeapon(Weapon.nullWeapon);
                break;
        }
    }
}
