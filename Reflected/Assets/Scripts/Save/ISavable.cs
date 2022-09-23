using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISavable 
{
    object SaveState();
    void LoadState(object state);
}
