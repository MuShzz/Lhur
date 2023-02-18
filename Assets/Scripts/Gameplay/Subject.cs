using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class Subject : NetworkBehaviour
{

    private List<IObserver> observers = new List<IObserver>();
    
    public void AddObserver(IObserver observer){ observers.Add(observer); }
    public void RemoveObserver(IObserver observer) { observers.Remove(observer); }
    protected void NotifyObservers(NotifyAction notifyAction)
    {
        observers.ForEach((observer) => { observer.OnNotify(notifyAction); });
    }
    protected void NotifyObservers(NotifyAction notifyAction, Dictionary<string, object> notifyParams)
    {
        observers.ForEach((observer) => { observer.OnNotifyParams(notifyAction, notifyParams); });
    }
}
