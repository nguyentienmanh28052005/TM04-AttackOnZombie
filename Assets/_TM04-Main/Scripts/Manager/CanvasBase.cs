using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CanvasBase : MonoBehaviour
{
    [Header("Default variables")]
    [SerializeField] private Button _closeButton;
    [SerializeField] private float _timeToOpenCanvas = 0f;
    [SerializeField] private GameObject _cammeraCanvas;

    public virtual void Awake()
    {
        if(_closeButton != null)
            _closeButton.onClick.AddListener(Hide);
    }

    public virtual void Show(object data = null)
    {
        //Start some show up animation
        _cammeraCanvas.SetActive(true);
        transform.localScale = Vector3.one * 0.2f;
        transform.DOScale(1, _timeToOpenCanvas);
        
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        _cammeraCanvas.SetActive(false);
        gameObject.SetActive(false);
    }
}
