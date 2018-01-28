using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleManager
{
    public static MoleManager Instance;

    public Action OnListenStart;
    public Action OnListenStop;

    static MoleManager() {
        Instance = new MoleManager();
    }
}
