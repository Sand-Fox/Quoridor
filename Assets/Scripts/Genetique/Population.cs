using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;


public class Population : MonoBehaviour
{
    Random rd = new Random();
    public static Population Instance;

    public List<IAAlphaBeta> population = new List<IAAlphaBeta>();

    private void Awake() => Instance = this;


    public void GeneratePopulation(int number)
    {
        for(int i = 0; i<number; i++)
        {
            IAAlphaBeta ia = new IAAlphaBeta();
            ia.weight = new float[]{rd.NextDouble(), rd.NextDouble(), rd.NextDouble(), rd.NextDouble()};
            population.Add(ia);
        }
    }

    public IAAlphaBeta reproduce(IAAlphaBeta father, IAAlphaBeta mother)
    {
        new ia = new IAAlphaBeta(); 
        for(int i = 0; i<4; i++)
        {
            ia.weight[i] = (rd.Next(0,1)=0)?father.weight[i]:mother.weight[i];
        }
        return ia;
    }

    public IAAlphaBeta mutation(IAAlphaBeta father)
    {
        new ia = new IAAlphaBeta();

        for(int i = 0; then i<(rd.Next(0,5)); i++)
        {
            int car = Next(0,4);
            ia.weight[car] = ia.weight.car *(1+ (rd.NextDouble()-1)/5);
        }
    }




}

