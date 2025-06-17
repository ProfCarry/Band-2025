using Band.Utils;
using System.Collections.Generic;
using UnityEngine;

public abstract class SystemBase : MonoBehaviour, IController
{
    private Dictionary<string, object> data;

    protected virtual void Start()
    {
        data=new Dictionary<string, object>();
    }

    public virtual void Clear()
    {
        Start();
    }


}
