using UnityEngine;
using Unity.MLAgentsExamples;

public class Arena : Area
{
    public GameObject gem;
    public GameObject mutant;
    public int numGem;
    public int numMutant;
    public bool respawnObject;
    public float range;

    void CreateObject(int num, GameObject type)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject f = Instantiate(type, new Vector3(
                    Random.Range(-range, range),
                    -23.2f,
                    Random.Range(-range, range)) + transform.position,
                Quaternion.Euler(new Vector3(0f, Random.Range(0f, 360f), 0f)));
            f.GetComponent<ObjectLogic>().respawn = respawnObject;
            f.GetComponent<ObjectLogic>().myArena = this;
        }
    }

    public void ResetArena(GameObject[] agents)
    {
        foreach (GameObject agent in agents)
        {
            if (agent.transform.parent == gameObject.transform)
            {
                agent.transform.position = new Vector3(
                                               Random.Range(-range, range), 
                                               -23.2f,
                                               Random.Range(-range, range))
                    + transform.position;
                agent.transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));
            }
        }

        CreateObject(numGem, gem);
        CreateObject(numMutant, mutant);
    }

    public override void ResetArea()
    {
    }
}
