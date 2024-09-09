using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BordersObject : MonoBehaviour
{
    [SerializeField] private Line[] _borderLines;
    [SerializeField] private int _looseBorder;

    public Line[] BorderLines {  get { return _borderLines; } }

    public int LooseBorderIndex => _looseBorder;
}
