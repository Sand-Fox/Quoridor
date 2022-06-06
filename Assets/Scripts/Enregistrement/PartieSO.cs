using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Enregistrement")]
public class PartieSO : ScriptableObject
{
    public List<Coup> partie = new List<Coup>();
    public int current; // Coup courrant
    public static int count = 0;
    

    public static PartieSO CreatePartie()
    {
        PartieSO partie =  CreateInstance<PartieSO>();
        #if UNITY_EDITOR
        AssetDatabase.CreateAsset(partie, "Assets/Scripts/Enregistrement/Parties/partie"+ count + ".asset" );
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = partie;
        #endif
        count++;
        return partie;
    }
}