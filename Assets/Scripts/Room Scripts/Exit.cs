﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class Exit {

    //public string myKeyword;
    public DirectionKeyword myKeyword;
    public Room conectedRoom;
    [TextArea] public string exitDescription;
}