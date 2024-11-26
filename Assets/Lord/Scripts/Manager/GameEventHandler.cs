using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventHandler : MonoBehaviour
{
    public void TriggerGameEvent(GameEvent gameEvent)
    {
        gameEvent.TriggerEvent();
    }
}
