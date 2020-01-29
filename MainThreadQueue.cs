using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Diagnostics;


// Created using modifyied example from https://jacksondunstan.com/articles/3930
public class MainThreadQueue
{

    // Enum Containing the definition for all avaliable commands
    private enum CommandType
    {
        SetPosition,
        GetTransform
    }


    // Internal abstract class representing commands to be performed on the main thread
    private abstract class Command
    {
        public CommandType Type;
    }


    // Position used to update the position of a supplied object
    private class SetPositionCommand : Command
    {
        public Transform transform;
        public int id;

        public SetPositionCommand()
        {
            Type = CommandType.SetPosition;
        }
    }

    public class Results
    {

    }


    public class SetPositionResults: Results
    {
        public int id;
        public Transform position;

        public SetPositionResults(int id, Transform position)
        {
            this.id = id;
            this.position = position;
        }
    }


    private Stack<SetPositionCommand> setPositionPool;
    private Queue<Command> commandQueue;

    public Queue<Results> resultsQueue;

    //public bool resultsReady = false;
    

    public MainThreadQueue()
    {
        commandQueue = new Queue<Command>();
        resultsQueue = new Queue<Results>();
    }

    /*
    // Retrieves an elemnet from the pool of commands of generic types
    private static T GetFromPool<T>(Stack<T> pool) where T : new()
    {
        if(pool.Count > 0)
        {
            return pool.Pop();
        }
        return new T();
    }


    // Returns a space from the pool of messages
    private static void ReturnToPool<T>(Stack<T> pool, T obj)
    {
        pool.Push(obj);
    } */


    // Adds a subclass of type command to the queue of messages to be completed
    private void QueueCommand(Command command)
    {
        commandQueue.Enqueue(command);
    }


    // Adds a set position command to the queue
    public void SetPosition(Transform transform, int id)
    {
        //SetPositionCommand command = GetFromPool(setPositionPool);

        SetPositionCommand command = new SetPositionCommand();
        command.transform = transform;
        command.id = id;
        QueueCommand(command);
       
    }


    public void Execute()
    {
        Command baseCommand;

        if(commandQueue.Count == 0)
        {
            UnityEngine.Debug.Log("RETURN");
            return;
        }

        baseCommand = commandQueue.Dequeue();

        switch (baseCommand.Type)
        {
            case CommandType.SetPosition:
                {
                    SetPositionCommand setPositionCommand = (SetPositionCommand)baseCommand;
                    Transform newTransform = setPositionCommand.transform;
                    
                    SetPositionResults result = new SetPositionResults(setPositionCommand.id, setPositionCommand.transform);

                    resultsQueue.Enqueue(result);

                    //ReturnToPool(setPositionPool, setPositionCommand);
                    
                    break;
                }
        }
    }
 
}
