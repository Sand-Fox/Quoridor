using UnityEngine;

[System.Serializable]
public struct Coup
{
    public string type;
    public Vector3 coord;
    public Orientation orientation;

    public Coup(string _type, Vector3 _coord, Orientation _orientation)
    {
        type = _type;
        coord = _coord;
        orientation = _orientation;
    }
}