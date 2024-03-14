using System;
using UnityEngine;
using TikTokLiveUnity;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    private CharacterController character;
    private Vector3 direction;
    
    public float jumpForce = 8f;
    public float gravity = 9.81f * 2f;
    public float jumpDelay = 0.5f; // Délai de 0.5 seconde entre les sauts
    private float lastJumpTime = -1f; // Quand le dernier saut a eu lieu

    async void Start(){
        var userName="fukuma_mizushi20";
    }
    private void Awake()
    {
        character = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        direction = Vector3.zero;
    }

    private void Update()
    {
        direction += gravity * Time.deltaTime * Vector3.down;

         if (character.isGrounded)
            {
                direction = Vector3.down;

        //     if (Input.GetButton("Jump")) {
        //         direction = Vector3.up * jumpForce;
        //     }
            }
           if (Input.GetButton("Jump")  && transform.position.y <= 3) {
                direction = Vector3.up * jumpForce;
                
            }
        TikTokLiveManager.Instance.OnGift += (liveClient, likeEvent) => {
             if (transform.position.y <= 3) {
                direction = Vector3.up * jumpForce;
            }
        };

        character.Move(direction * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle")) {
            GameManager.Instance.GameOver();
        }
    }

}
