using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Population : MonoBehaviour
{
    public static Population Instance;

    public static List<Vector4> population = new List<Vector4>();
    public static List<Vector4> winner = new List<Vector4>();

    private int nbIndividus = 4;
    private int nbGenerations = 2;

    public static int indexIndividus = 0;
    public static int indexGenerations = 0;

    private void Awake()
    {
        Instance = this;
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.Win && SceneSetUpManager.playMode == "Algo Genetique") OnEndGame(true);
        if (newState == GameState.Loose && SceneSetUpManager.playMode == "Algo Genetique") OnEndGame(false);
    }

    public void Play()
    {
        indexIndividus = 0;
        indexGenerations = 0;
        GeneratePopulation();
        Match();
    }

    public void OnEndGame(bool stateIsWin)
    {
        indexIndividus += 2;

        IAAlphaBeta IAWinner;
        if (stateIsWin) IAWinner = ReferenceManager.Instance.player as IAAlphaBeta;
        else IAWinner = ReferenceManager.Instance.enemy as IAAlphaBeta;
        winner.Add(IAWinner.weight);

        if (indexIndividus == nbIndividus)
        {
            indexGenerations++;
            indexIndividus = 0;

            if (indexGenerations == nbGenerations)
            {
                string messageFinal = "Fin de l'algorithme génétique : \n";
                foreach (Vector4 weight in winner) messageFinal += weight + "\n";
                Debug.Log(messageFinal);
                return;
            }

            population = NewGeneration();
            Match();
            return;
        }

        Match();
    }

    private void GeneratePopulation()
    {
        for(int i = 0; i < nbIndividus; i++)
        {
            Vector4 weight = new Vector4(Random.value, Random.value, Random.value, Random.value);
            population.Add(weight);
        }
    }

    public void Match()
    {
        Vector4 weight1 = population[Random.Range(0, population.Count)];
        population.Remove(weight1);

        Vector4 weight2 = population[Random.Range(0, population.Count)];
        population.Remove(weight2);

        SceneSetUpManager.IAWeight1 = weight1;
        SceneSetUpManager.IAWeight2 = weight2;

        if (PhotonNetwork.InRoom) PhotonNetwork.LoadLevel("Game");
        else PrivateRoom.Instance.CreatePrivateRoom();
    }

    private Vector4 Reproduce(Vector4 father, Vector4 mother)
    {
        Vector4 son = new Vector4(
            (Random.value < 0.5) ? father.x : mother.x,
            (Random.value < 0.5) ? father.y : mother.y,
            (Random.value < 0.5) ? father.z : mother.z,
            (Random.value < 0.5) ? father.w : mother.w); 
        return son;
    }

    private Vector4 Modify(Vector4 father, int geneIndex)
    {
        if (geneIndex == 0) return new Vector4(father.x * (1 + Random.Range(-0.1f, 0.1f)), father.y, father.z, father.w);
        if (geneIndex == 1) return new Vector4(father.x, father.y * (1 + Random.Range(-0.1f, 0.1f)), father.z, father.w);
        if (geneIndex == 2) return new Vector4(father.x, father.y, father.z * (1 + Random.Range(-0.1f, 0.1f)), father.w);
        if (geneIndex == 3) return new Vector4(father.x, father.y, father.z, father.w * (1 + Random.Range(-0.1f, 0.1f)));
        Debug.LogWarning("n is not in range of Vector4");
        return default;
    }

    private Vector4 Mutation(Vector4 father)
    {
        Vector4 son = new Vector4();
        int nbGenes = Random.Range(1, 5);

        for(int i = 0; i < nbGenes; i++)
        {
            int geneIndex = Random.Range(0, 4);
            son = Modify(father, geneIndex);
        }
        return son;
    }

    private List<Vector4> NewGeneration()
    {
        List<Vector4> newGen = new List<Vector4>();
        while(winner.Count > 0)
        {
            if (winner.Count == 1)
            {
                newGen.Add(winner[0]);
                winner.Remove(winner[0]);
                break;
            }

            Vector4 father = winner[Random.Range(0, winner.Count)];
            winner.Remove(father);

            Vector4 mother = winner[Random.Range(0, winner.Count)];
            winner.Remove(mother);

            newGen.Add(father);
            newGen.Add(mother);

            Vector4 child1 = Reproduce(father, mother);
            Vector4 child2 = Reproduce(father, mother);


            if (Random.value < 0.01) child1 = Mutation(child1);
            if (Random.value < 0.01) child2 = Mutation(child2);
            
            newGen.Add(child1);
            newGen.Add(child2);
        }
        return newGen;
    }
}

