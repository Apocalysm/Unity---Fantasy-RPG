﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    // The characters player controller and animator components
    private CharacterController controller;
    private Animator _anim;

    // How fast the player will be moving
    [Range(1f, 50f)]
    public float moveSpeed = 0f;
    // Modifier to adjust animation speed
    public float speedMod = 0.3f;

    // Vector for movement input
    Vector3 inputVec = Vector3.zero;
    // For applyng gravity to the character
    private float m_ySpeed = 0;

    private Vector3 lastPos;

    void Awake ()
    {
        controller = GetComponent<CharacterController>();

        _anim = GetComponent<Animator>();

        if (!OverworldEnemySingleton.instance.backFromCombat && PlayerSingleton.instance.entryPos != Vector3.zero)
        {
            transform.position = PlayerSingleton.instance.entryPos;
            transform.rotation = Quaternion.Euler(PlayerSingleton.instance.entryRot);
        }

        PlayerSingleton.instance.currentScene = Application.loadedLevel;

        lastPos = transform.position;
	}
	
	void FixedUpdate ()
    {
        if (PlayerSingleton.instance.canMove)
        {
            // Creates a vector for input            
            inputVec.x = Input.GetAxis("Horizontal");
            inputVec.z = Input.GetAxis("Vertical");

            // Calculates the magnitude of the two input values we created
            float inputMagnitude = Vector3.Magnitude(new Vector3(inputVec.x, 0, inputVec.z));

            _anim.SetFloat("WalkModifier", speedMod * moveSpeed);

            // Sets the parameter for the player's walk animation
            _anim.SetFloat("Velocity", inputMagnitude);

            // If the player is moving we change their rotation
            if (inputMagnitude > 0)
            {
                transform.rotation = Quaternion.LookRotation(new Vector3(inputVec.x, 0, inputVec.z));
            }
            
            // Moves the character by lerping the velocity variable in rigidbody
            GetComponent<CharacterController>().SimpleMove(new Vector3(Mathf.Lerp(0, inputVec.x * moveSpeed, 0.8f), 0,
            Mathf.Lerp(0, inputVec.z * moveSpeed, 0.8f)));

            //Debug.Log(Vector3.Distance(transform.position, lastPos));
        }
        else
        {
            // Sets the parameter for the player's walk animation to make it stop whilst not moving
            _anim.SetFloat("Velocity", 0);
        }
         lastPos = transform.position;
    }

    /*
    // Code for playing a footstep sound on an animation event, currently unused
    void FootStep()
    {
        source.pitch = Random.Range(0.9f, 1.1f);
        source.PlayOneShot(grassStep, 0.4f);

    }
    */
}
