using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInvoker : MonoBehaviour
{
    public Stack<ICommand> commandHistory = new Stack<ICommand>();
    public Queue<ICommand> commandQueue = new Queue<ICommand>();
    public Queue<ICommand> commandReplay = new Queue<ICommand>();

    public static SkillInvoker instance;

    void Start()
    {
        if (instance == null) { instance = this; }
        else { Object.Destroy(this); }
    }

    void FixedUpdate()
    {
        if (commandQueue.Count > 0)
        {
            switch (commandQueue.Peek().getCommandStatus())
            {
                case CommandStatus.Pending:
                    commandQueue.Peek().execute();
                    break;
                case CommandStatus.Finished:
                    commandReplay.Enqueue(commandQueue.Dequeue());
                    Debug.Log("commandQueue: "+ commandQueue.Count+ " commandReplay: "+ commandReplay.Count);
                    break;
                case CommandStatus.Failed:
                    commandReplay.Enqueue(commandQueue.Dequeue());
                    break;
            }
        }
    }

    public void AddQueue(ICommand command)
    {
        commandQueue.Enqueue(command);
    }
}
