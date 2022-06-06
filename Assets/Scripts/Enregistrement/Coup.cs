using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coup
{
    public string type {get; private set;}
    public Vector3 coord {get; private set;}
    public bool orientation {get; private set;}

    public Coup(string _type, Vector3 _coord, bool _orientation)
    {
        type = _type;
        coord = _coord;
        orientation = _orientation;
    }
}