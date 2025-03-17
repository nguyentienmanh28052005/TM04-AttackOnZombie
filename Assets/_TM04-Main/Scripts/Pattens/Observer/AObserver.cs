using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public abstract class AObserver : MonoBehaviour
{
    private List<IObserver> _observers = new List<IObserver>();
    
    
    public void AddObserver(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        _observers.Remove(observer);
    }

    protected void NotifyObservers(Define action)
    {
        _observers.ForEach((_observer) =>
        {
            _observer.OnNotify(action);
        });
    }
}