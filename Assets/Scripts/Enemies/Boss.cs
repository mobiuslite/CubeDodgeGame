using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    [Range(1.0f, 10000.0f)]
    protected float health;

    [SerializeField]
    [Range(1.0f, 100.0f)]
    protected float speed;

    [SerializeField]
    GameObject prjPrefab;

    protected BossStateMachine stateMachine;
    void Start()
    {
        stateMachine = new BossStateMachine();
        stateMachine.SetProjectile(prjPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update(Time.deltaTime);
    }
}
