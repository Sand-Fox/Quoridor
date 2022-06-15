using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PartieSO : ScriptableObject
{
    public int current;
    public List<Coup> ListCoups = new List<Coup>();
    public Coup[] ArrayCoups;

    public static PartieSO CreatePartie()
    {
        PartieSO partie = CreateInstance<PartieSO>();
        #if UNITY_EDITOR
        int count = Resources.LoadAll("Parties").Length;
        AssetDatabase.CreateAsset(partie, "Assets/Resources/Parties/partie"+ count + ".asset" );
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = partie;
        #endif
        return partie;
    }

    public void SavePartie()
    {
        ArrayCoups = ListCoups.ToArray();
    }
}