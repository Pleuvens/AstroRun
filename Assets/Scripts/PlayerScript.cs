using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

    public GameObject smoke;

    int maxSpeed = 60;
    public float speed = 18.0F;
    //public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    public long d = 0;
    public bool isGameOver = false;
    private Vector3 moveDirection = Vector3.zero;
    float jumpHeight = 4f;
    public float jumpDistance = 0f;
    public float cooldown = 0f;
    public float yMove = 0f;

    public Vector3 destination = Vector3.zero;

    public Text dist;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("hasStarted", true);
    }

	void Update () {
        if (isGameOver)
            return;
        if (cooldown > 0)
            cooldown -= Time.deltaTime;
        dist.text = this.transform.position.z.ToString("F0");
        jumpDistance = speed+ 1 / 2 * gravity;
        smoke.SetActive(true);
        CharacterController controller = GetComponent<CharacterController>();
        if (speed < maxSpeed && this.transform.position.z > d)
        {
            speed += 1;
            d += 100;
        }
        moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 1);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;
        if (controller.isGrounded)
        {
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = Mathf.Sqrt(2 * gravity * jumpHeight);
                moveDirection.x = 0f;
            }           
        }
        else
        {
            smoke.SetActive(false);
            moveDirection.y = yMove;
        }

        int xMove = Mathf.RoundToInt(moveDirection.x);
        if (xMove > 0)
            xMove = 4;
        else if (xMove < 0)
            xMove = -4;
        
        if (cooldown <= 0)
        {
            destination = Vector3.zero;
            cooldown = 0;
            if (xMove != 0)
                cooldown += 0.2f;
            if (xMove < 0 && this.transform.position.x > -2)
                destination = new Vector3((xMove + this.transform.position.x), 0, this.transform.position.z);
            else if (xMove > 0 && this.transform.position.x < 2)
                destination = new Vector3((xMove + this.transform.position.x), 0, this.transform.position.z);
        }
        else
        {
            if (destination != Vector3.zero && Mathf.Abs(this.transform.position.x - destination.x) > 0.1)
            {
                Vector3 inter = this.transform.position; 
                inter.x = (destination.x + inter.x) / 2;
                this.transform.position = inter;
            }
        }
        moveDirection.x = 0;
        moveDirection.y -= gravity * Time.deltaTime;
        yMove = moveDirection.y;
        controller.Move(moveDirection * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.gameObject.tag == "Obstacle")
        {
            isGameOver = true;
            anim.SetBool("hasStarted", false);
            smoke.SetActive(false);
        }
    }
}
