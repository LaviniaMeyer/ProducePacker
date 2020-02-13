using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortObject
{
    public enum SortType { Food, Flower, Gift, Rubbish }
    public SortType objectType;

    public SortObject(SortType Type)
    {
        objectType = Type;
    }
}
