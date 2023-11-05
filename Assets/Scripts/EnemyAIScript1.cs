using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class EnemyAIScript1 : MonoBehaviour
{
    [Header("Enemy Object and its components")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] AudioSource source;

    [Header("Enemy Sounds")]
    [SerializeField] Sound[] sfxSounds;

    [Header("Enemy states")]
    [SerializeField] EnemyStates aggroState;   
    public enum EnemyStates { Idle, Patrol, Chase }
    public EnemyStates AggroState { get => aggroState; set => aggroState = value; }

    [Header("Enemy information")]
    [SerializeField] GameObject target;
    [SerializeField] GameObject[] patrolPoints;
    public float speed;


    // Start is called before the first frame update
    public void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        anim = gameObject.GetComponent<Animator>();
        source = gameObject.GetComponent<AudioSource>();
        aggroState = EnemyStates.Chase;
        target = GameObject.Find("Player");
        agent.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            agent.destination = target.transform.position;
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, ss => ss.clipName == name);

        if (s == null)
        {
            Debug.Log("Sound not found.");
        }
        else
        {
            source.PlayOneShot(s.clip);
        }
    }

    // Set the target to player and begin chasing
    public void StartChasing(GameObject _target)
    {
        target = _target;
        agent.speed = speed;
        aggroState = EnemyStates.Chase;
    }

    // Clear target and begin to idle
    public void StartIdle()
    {
        target = null;
        agent.speed = 0;
        aggroState = EnemyStates.Idle;
    }

    // Get next patrol point and and start moving
    public void StartPatrol()
    {
        // Not yet implemented
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !other.gameObject.GetComponent<CharacterScript>().IsProjection)
        {
            other.gameObject.GetComponent<CharacterScript>().Kill();
        }

        if (other.gameObject.CompareTag("Projection"))
        {
            GameManager.instance.DeActivateProjection();
        }
    }
}
