using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    private int chestAmount=50;
    private static List<Chest> chests = new List<Chest>();
    public GameObject chestPrefab;
   
    
    void Start()
    {
        for (int i = 0; i < chestAmount; i++)
        {
            Chest chest = chestPrefab.GetComponent<Chest>();
            chest.obj = chestPrefab;
          
            SetChest(chest);
            chests.Add(chest);
            chest.obj.gameObject.tag = "chest";
            chest.obj.transform.position = SetRandomPosition();
            Instantiate(chest.obj);
            chest.apparency = true;
       
        }
    }
    public void SetChest(Chest chest)
    {
        float ratio= Random.Range(0, 5);

        if (ratio < 2)
            chest.feature = 10;
        if (ratio == 2)
            chest.feature = 30;
        if (ratio > 2)
            chest.feature = -500;

    }
    private static bool IsThere(Vector3 pos)
    {
        bool result = false;
        foreach (var item in chests)
        {
            if (pos == item.obj.transform.position)
            {
                result = true;

            }
        }

        return result;
    }
    public static Vector3 SetRandomPosition()
    {
        float x =Random.Range(-Arena.range, Arena.range),
        y=-24f,
        z= Random.Range(-Arena.range, Arena.range);
      
        Vector3 randPos = new Vector3(x, y,z);
        while(IsThere(randPos))
        {
            x = Random.Range(-Arena.range, Arena.range);
            z= Random.Range(-Arena.range, Arena.range);
            randPos = new Vector3(x, y, z);
        
        }

        return randPos;
    }

    void Update()
    {
        foreach (var item in chests)
        {
            if (!item.apparency)
            {
                item.obj.transform.position = SetRandomPosition();

            }
        }
    }
}
