using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Enregistrement")]
public class PartieSO : ScriptableObject
{
    public List<Coup> partie = new List<Coup>();
    public int current; // Coup courrant
    public static int count = 0;
    

    public static PartieSO CreatePartie()
    {
        PartieSO partie =  ScriptableObject.CreateInstance<PartieSO>();
        AssetDatabase.CreateAsset(partie, "Assets/Scripts/Enregistrement/Parties/partie"+ count + ".asset" );
        AssetDatabase.SaveAssets();
        count++;
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = partie;

        return partie;
    }
}