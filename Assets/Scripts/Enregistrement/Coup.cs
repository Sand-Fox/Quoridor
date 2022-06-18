using UnityEngine;

[System.Serializable]
public class Coup
{
    public string type;
    public float[] coord;
    public Orientation orientation;

    public Coup(string _type, Vector2 _coord, Orientation _orientation)
    {
        type = _type;
        coord = new float[2];
        coord[0] = _coord.x;
        coord[1] = _coord.y;
        orientation = _orientation;
    }
}