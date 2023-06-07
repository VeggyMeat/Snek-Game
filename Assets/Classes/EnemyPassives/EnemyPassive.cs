using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPassive: MonoBehaviour
{
    private EnemyPassiveHandler handler;
    private int value;
    private string type;

    public void Setup(EnemyPassiveHandler passiveHandler, float duration, int value, string type)
    {
        handler = passiveHandler;
        this.value = value;
        this.type = type;

        Invoke(nameof(OnDestroy), duration);
    }

    public void OnDestroy()
    {
        handler.RemovePassive(type, value);

        Destroy(this);
    }
}
