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
    private bool iCanJump = false;

    async void Start(){
        var userName= "okamexile";
        TikTokLiveManager.Instance.OnGift += (liveClient, giftEvent) => {
            iCanJump = true;
            Debug.Log(message: $"Thank you for Gift! {giftEvent.Gift.Name}{giftEvent.Sender.NickName}");
        };
        await TikTokLiveManager.Instance.ConnectToStream(userName);
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
        
        if (iCanJump == true){
            jump();
            iCanJump = false;
        }
        character.Move(direction * Time.deltaTime);
    }

    private void jump()
    {/*
        if (Time.time - lastJumpTime >= jumpDelay && transform.position.y <= 3)
        {*/
            direction = Vector3.up * jumpForce;
            
         /*   Debug.Log(direction);
            lastJumpTime = Time.time; // Mettre à jour le temps du dernier saut
        }*/
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle")) {
            GameManager.Instance.GameOver();
        }
    }

}
