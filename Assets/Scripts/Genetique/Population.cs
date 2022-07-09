using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;


public class Population : MonoBehaviour
{
    Random rd = new Random();
    public static Population Instance;

    public static List<Vector4> population = new List<Vector4>();
    public static List<Vector4> winner = new List<Vector4>();
    public int maxPop = 20;
    
    private void Awake() => Instance = this;


    public void Launch(int nbGeneration)
    {
        GeneratePopulation();
        SceneSetUpManager.IAName1 = "IAAlphaBeta";
        SceneSetUpManager.IAName2 = "IAAlphaBeta";

        for(int i = 0; i<nbGeneration; i++)
        {
            Match();
            population = NewGeneration();
        }
        Match();
    }



    public void Match()
    {
        Vector4 ia1 = population[rd.Next(0, population.Count)];
        Vector4 ia2 = population[rd.Next(0, population.Count)];

        PrivateRoom.Instance.CreatePrivateRoom();
    }



    public void GeneratePopulation()
    {
        for(int i = 0; i<maxPop; i++)
        {
            Vector4 weight = new Vector4(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
            population.Add(weight);
        }
    }

    public Vector4 Reproduce(Vector4 father, Vector4 mother)
    {
        Vector4 ia = new Vector4((rd.Next(0,2)==0)?father.x:mother.x,
                        (rd.Next(0,2)==0)?father.y:mother.y,
                        (rd.Next(0,2)==0)?father.z:mother.z,
                        (rd.Next(0,1)==0)?father.w:mother.w
                    ); 
        return ia;
    }

    public Vector4 Modif(Vector4 original, int n)
    {
        if(n==0) return new Vector4(original.x *(1+(UnityEngine.Random.value-0.5f)/5), original.y, original.z, original.w);
        if(n==1) return new Vector4(original.x, original.y*(1+(UnityEngine.Random.value-0.5f)/5), original.z, original.w);
        if(n==2) return new Vector4(original.x, original.y, original.z*(1+(UnityEngine.Random.value-0.5f)/5), original.w);
        if(n==3) return new Vector4(original.x, original.y, original.z, original.w*(1+(UnityEngine.Random.value-0.5f)/5));
        Debug.LogWarning("n is not in range of Vector4");
        return default;
    }

    public Vector4 Mutation(Vector4 father)
    {
        Vector4 ia = new Vector4();
        int nbGenes = rd.Next(1,5);
        for(int i = 0; i<nbGenes; i++)
        {
            int gene = rd.Next(0,5);
            ia = Modif(father, gene);
        }
        return ia;
    }

    public void Kill(Vector4 victim) => winner.Remove(victim);

    public List<Vector4> NewGeneration()
    {
        List<Vector4> newGen = new List<Vector4>();
        while(winner.Count>0)
        {
            if (winner.Count == 1)
            {
                newGen.Add(winner[0]);
                winner.Remove(winner[0]);
                break;
            }
            Vector4 father = winner[rd.Next(0, population.Count)];
            Kill(father);
            
            Vector4 mother = winner[rd.Next(0, population.Count)];
            Kill(mother);

            newGen.Add(father);
            newGen.Add(mother);

            Vector4 child1 = Reproduce(father, mother);
            Vector4 child2 = Reproduce(father, mother);

            if(UnityEngine.Random.value < 0.01) child1 = Mutation(child1);
            if(UnityEngine.Random.value < 0.01) child2 = Mutation(child2);
            
            newGen.Add(child1);
            newGen.Add(child2);
        }
        return newGen;
    }
}

