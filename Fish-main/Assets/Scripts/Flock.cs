using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    //
    public FlockManager myManager;
    float speed;
    Vector3 groupCenter;
    bool turning = false;

    //aleatoriza a velocidade 
    void Start()
    {
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
    }
    //
    void Update()
    {
        //cria o limite do myManager
        Bounds b = new Bounds(myManager.transform.position, myManager.swinLimits * 2);
        //instancia o RaycastHit 
        RaycastHit hit = new RaycastHit();
        //Cria o vector3 apontando para o myManager
        Vector3 direction = myManager.transform.position - transform.position;

        //detecta a colisão se o peixe entrar dentro do limite
        if (!b.Contains(transform.position))
        {
            //
            turning = true;
            direction = myManager.transform.position - transform.position;
        }
        //gera um Raycast na frente do peixe 
        else if (Physics.Raycast(transform.position, this.transform.forward * 50, out hit))
        {
            //atribui a direção oposta no direct
            turning = true;
            direction = Vector3.Reflect(this.transform.forward, hit.normal);
        }
        else 
            //se não detectar nada ele desvia 
            turning = false;

        //se estiver ativa ele rotaciona até uma das posições apresentadas nos ifs anteriores 
        if (turning)
        {
            
            transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(direction),
            myManager.rotationSpeed * Time.deltaTime);
        }
        else
        {
            //aleatoriza a velocidade
            if (Random.Range(0, 100) < 10)
                speed = Random.Range(myManager.minSpeed,
                myManager.maxSpeed);
            if (Random.Range(0, 100) < 20)
                ApplyRules();
        }
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    //
    void ApplyRules()
    {
        GameObject[] gos;
        gos = myManager.allFish;

        Vector3 vcenter = Vector3.zero;
        Vector3 vavoid = Vector3.zero;

        float gSpeed = 0.01f;
        float nDistance;
        int groupSize = 0;

        foreach (GameObject go in gos)
        {
            if (go != this.gameObject)
            {
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);
                if (nDistance <= myManager.neighbourDistance)
                {
                    vcenter += go.transform.position;
                    groupSize++;

                    if (nDistance < 1.0f)
                    {
                        vavoid = vavoid + (this.transform.position - go.transform.position);

                    }

                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }
        if (groupSize > 0)
        {
            vcenter = vcenter / groupSize;
            speed = gSpeed / groupSize;

            Vector3 direction = (vcenter + vavoid) - transform.position;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);
            }
        }

        groupCenter = vcenter;
    }
}
