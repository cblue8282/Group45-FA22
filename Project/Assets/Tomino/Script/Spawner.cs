using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] groups;

    public void spawnNext()
    {
        int i = Random.Range(0, groups.Length); //Random index to select a random group

        Instantiate(groups[i],
                    transform.position,
                    Quaternion.identity);
    }
    // Start is called before the first frame update
    void Start()
    {
        spawnNext();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
