using System.Collections.Generic;

public interface IObserver
{
    public void OnNotify(NotifyAction notifyAction);
    public void OnNotifyParams(NotifyAction notifyAction, Dictionary<string, object> notifyParams);
}
