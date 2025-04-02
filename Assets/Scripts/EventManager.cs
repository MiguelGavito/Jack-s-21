using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action ExampleEvent;

    private void Update()
    {
        // if(ExampleEvent != null)
        //    ExampleEvent();

        ExampleEvent?.Invoke();
    }
}
