using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{

    private Player player;
    private Rigidbody2D rigidBody;
    private Vector3 moveDir;
    private float speed;//= 6f;
    private bool isDash = false;
    private bool isSprint = false;
    private bool dashCooldown = false;
    //private float stamina = 100f;

    //dashowe zmienne
    private float lastClickedTimeW;
    private float lastClickedTimeS;
    private float lastClickedTimeA;
    private float lastClickedTimeD;

    //Wyeliminowanie bugu wielu postaci po za³adowaniu sceny
    private GameObject[] players;
    private void Start()
    {
        player = GetComponent<Player>();
        rigidBody = GetComponent<Rigidbody2D>();
        moveDir = Vector3.zero;
    }
    
    //Update pobiera input z klawiatury odnoœnie poruszania siê
    void Update()
    {
        float stamina = player.getStat("Stamina"); //player.playerStats["Stamina"].GetValue();
        if (stamina < 100f)
        {
            stamina += 0.01f;
            player.setStat("Stamina", stamina);
            //player.playerStats["Stamina"].SetValue(stamina);
        }

        float moveY = 0f;
        float moveX = 0f;
        bool sprint = false;
        bool dash = false;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            sprint = true;
        }

        if (Input.GetKey(KeyCode.W))
        {
            moveY = 1f;
        }

        //Dash dzia³a tak, ¿e mierzy czas naciœniêcia klawisza np. W
        //Od jego ostatniego naciœniêcia
        if (Input.GetKeyDown(KeyCode.W))
        {
            float timeSinceLastClick = Time.time - lastClickedTimeW;
            lastClickedTimeW = Time.time;
            if (timeSinceLastClick <= 0.2f)
                dash = true;
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveY = -1f;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            float timeSinceLastClick = Time.time - lastClickedTimeS;
            lastClickedTimeS = Time.time;
            if (timeSinceLastClick <= 0.2f)
                dash = true;
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1f;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            float timeSinceLastClick = Time.time - lastClickedTimeA;
            lastClickedTimeA = Time.time;
            if (timeSinceLastClick <= 0.2f)
                dash = true;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveX = 1f;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            float timeSinceLastClick = Time.time - lastClickedTimeD;
            lastClickedTimeD = Time.time;
            if (timeSinceLastClick <= 0.2f)
                dash = true;
        }

        Move(new Vector3(moveX, moveY).normalized, sprint, dash);
    }

    //Tutaj sprawdzane warunki maj¹ daæ koñcow¹ wartoœæ speed'a gracza
    //I ustawiæ wektor, w którym siê bêdzie porusza³
    private void Move(Vector3 direction, bool sprint, bool dash)
    {
        moveDir = direction;
        float stamina = player.getStat("Stamina"); //player.playerStats["Stamina"].GetValue();
        if (dash && !dashCooldown && stamina >= 30f)
        {
            isDash = true;
        }
        else if (sprint && stamina >= 0)
        {
            speed = 12f;
            isSprint = true;
        }
        else
        {
            speed = 6f;
        }

    }

    //Tutaj dzieje siê ca³e poruszanie za pomoc¹ dodawania
    //si³y (velocity) do rigidbody gracza
    private void FixedUpdate()
    {
        rigidBody.velocity = moveDir * speed;
        float stamina = player.getStat("Stamina"); //player.playerStats["Stamina"].GetValue();
        if (moveDir != Vector3.zero)
        {
            if (isSprint)
            {
                stamina -= 0.6f;
                isSprint = false;
            }


            if (isDash)
            {
                float dashVelocity = 3f;
                Vector3 dashPosition = transform.position + moveDir * dashVelocity;
                RaycastHit2D raycast = Physics2D.Raycast(transform.position, moveDir, dashVelocity, LayerMask.GetMask("Obstacle"));
                if (raycast.collider != null)
                {
                    dashPosition = raycast.point;
                }

                rigidBody.MovePosition(dashPosition);
                stamina -= 30f;
                StartCoroutine(DashCoolDown());
                isDash = false;
            }
        }
        player.setStat("Stamina", stamina);
        //player.playerStats["Stamina"].SetValue(stamina);
    }

    private IEnumerator DashCoolDown()
    {
        dashCooldown = true;
        player.SetImmunity(dashCooldown);
        yield return new WaitForSeconds(3f);
        dashCooldown = false;
        player.SetImmunity(dashCooldown);
    }

    //Tu odbywaæ siê bêd¹ wszystkie kolizje na zasadzie Trigger
    private void OnTriggerStay2D(Collider2D collision)
    {
        //S¹ sprawdzane przez konkretn¹ warstwê (layer) obiektu z jakim gracz koliduje
        if (collision.gameObject.layer == GameManager.GetLayerNumber("Spiketrap"))
        {
            player.TakeDamage(10f);
        }

    }
    //Ustawienie pozycji bohatera po za³adowaniu nowej sceny
    private void OnLevelWasLoaded(int level)
    {
        transform.position = GameObject.FindWithTag("StartPos").transform.position;
        players = GameObject.FindGameObjectsWithTag("Player");
        if(players.Length > 1)
        {
            Destroy(players[1]);
        }
    }
}
