using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    // 
    public GameObject fishPrefab;
    public int numFish = 20;
    public GameObject[] allFish;
    public Vector3 swinLimits = new Vector3(5, 5, 5);
    public Vector3 goalPos;

    //
    [Header("Configuraçoes do Cardume")]
    [Range(0.0f, 5.0f)]
    public float minSpeed;
    [Range(0.0f, 5.0f)]
    public float maxSpeed;
    [Range(1.0f, 10.0f)]
    public float neighbourDistance;
    [Range(1.0f, 5.0f)]
    public float rotationSpeed;
    
    private void Start()
    {
        //define o tamanho da lista dos peixes
        allFish = new GameObject[numFish];

        //instancia os peixes em uma posição aleatoria dentro alcanse do pos 
        for (int i = 0; i < numFish; i++)
        {

            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x),
                                                                Random.Range(-swinLimits.y, swinLimits.y),
                                                                Random.Range(-swinLimits.z, swinLimits.z));
            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
            
            //atribui o componente Flock em cada peixe instanciado
            allFish[i].GetComponent<Flock>().myManager = this;
        }

        //pega a posição do objeto com esse script e colocar no goalPos 
        goalPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //pega a posição do objeto com esse script e colocar no goalPos
        goalPos = this.transform.position;
        if (Random.Range(0, 100) < 10)
            goalPos = this.transform.position + new Vector3(Random.Range(-swinLimits.x,
            swinLimits.x),

            //oscila a posição goalPos pra não ficar sempre no mesmo lugar 
            Random.Range(-swinLimits.y, swinLimits.y),
            Random.Range(-swinLimits.z, swinLimits.z));
    }
}
