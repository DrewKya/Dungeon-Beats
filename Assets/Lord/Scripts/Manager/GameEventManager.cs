using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public void TriggerGameEvent(GameEvent gameEvent)
    {
        gameEvent.TriggerEvent();
    }
}
