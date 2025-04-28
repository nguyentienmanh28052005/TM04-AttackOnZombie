using TMPro;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private TextMeshProUGUI _textHealth;
    [SerializeField] private TextMeshProUGUI _textFood;
    [SerializeField] private TextMeshProUGUI _textWater;

    public int _playerHealth;
    public int _playerFood;
    public int _playerWater;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        SetText(_playerHealth, _playerFood, _playerWater);
    }

    private void SetText(int health, int food, int water)
    {
        _textHealth.text = health.ToString();
        _textFood.text = food.ToString();
        _textWater.text = water.ToString();
    }
}
