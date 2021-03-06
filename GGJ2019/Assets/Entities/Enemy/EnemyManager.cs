﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public LayerMask layerDetected;

    public float distance;
    public GameObject player;
    [SerializeField] float attack1Range = 1.0f;
    [SerializeField] float speed = 0.5f;
    [SerializeField] float speedRest = 0.05f;
    float lastColliderCount = 0;
    //original Position
    Vector3 originalPos;
    AudioManager audioManager;

    // Perlin Noise
    float tx;
    float tz;
    public float txinc;
    public float tzinc;


    public bool isChasing = false;
    //to send to shader
    Renderer rend;
    public GameObject cube;

    private void Awake()
    {
        originalPos = transform.position;
    }

    private void OnEnable()
    {
        transform.position = originalPos;
    }

    private void Start()
    {
        //to get access to shader
        rend = cube.GetComponent<Renderer>();
        rend.material.shader  = Shader.Find("Custom/EnemyShader");
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        DoDetect(transform.position, distance);
        //Rest();

        if (transform.position.x>10 || transform.position.x <-10
            || transform.position.z > 10 || transform.position.z < -10){
            transform.position = originalPos;
        }
    }


    void DoDetect(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, layerDetected);
        int colliderCount = hitColliders.Length;

        if (colliderCount > 0) { // player detected
            MoveToPlayer();
            isChasing = true;
            audioManager.SetAudioState(AudioManager.audioStates.ghost);
            rend.material.SetFloat("_Chasing", 1.0f);
        } else
        {
            rend.material.SetFloat("_Chasing", 0f);
            isChasing = false;
            if (lastColliderCount > 0)
            {
                audioManager.SetAudioState(AudioManager.audioStates.walking);
            }
            Rest();
        }
        lastColliderCount = colliderCount;
        //int i = 0;
        //while (i < colliderCount)
        //{
        //    if (hitColliders[i].tag.Equals("Player"))
        //    {
        //        Debug.Log("hitColliders: " + hitColliders[i].tag);
        //        MoveToPlayer();
        //    }
        //    i++;
        //}

    }

    void Rest()
    {
        //Vector3 movement = new Vector3(
        //    (xmax-xmin)*Mathf.PerlinNoise( tx, 0 )+xmin,
        //    0.0f,
        //    (zmax-zmin) * Mathf.PerlinNoise(0, tz)+zmin);

        transform.Translate(new Vector3(speedRest * (Mathf.PerlinNoise(tx, 0)-0.5f) ,
            0.0f,
            speedRest * (Mathf.PerlinNoise(0, tz) - 0.5f)));
        tx += txinc;
        tz += tzinc;
    }

    void MoveToPlayer()
    {
        Transform target = player.transform;
        //rotate to look at player
        if (Mathf.Abs(target.position.y - transform.position.y) < 0.5f)
        {


            transform.LookAt(target.position);
            transform.Rotate(new Vector3(0, -90, 0), Space.Self);

            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        }
        //move towards player
        if (Vector3.Distance(transform.position, target.position) > attack1Range)
        {
            //transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            player.GetComponent<PlayerLogic>().DealDamage(1f);
        }
    }

}