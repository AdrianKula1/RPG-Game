using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{

    [SerializeField] private Camera m_Camera;
    [SerializeField] private float cameraLerpSpeed;
    public bool stickCamera = true; // je�li false to kamera zwolniona i nie bedzie sie teleportowac do gracza

    //enumy dla kierunku
    private enum Direction
    {
        Forward,
        Right,
        Left,
        Backward
    }

    private Player player;
    private Rigidbody2D rigidBody;
    private Vector3 moveDir;
    private float speed;//= 6f;
    private bool isDash = false;
    private bool isSprint = false;
    private bool dashCooldown = false;
    private Direction direction;

    //dashowe zmienne
    private float lastClickedTimeW;
    private float lastClickedTimeS;
    private float lastClickedTimeA;
    private float lastClickedTimeD;

    //Wyeliminowanie bugu wielu postaci po za�adowaniu sceny
    private GameObject[] players;
    private void Start()
    {
        DontDestroyOnLoad(m_Camera);
        player = GetComponent<Player>();
        rigidBody = GetComponent<Rigidbody2D>();
        moveDir = Vector3.zero;
    }
    
    //Update pobiera input z klawiatury odno�nie poruszania si�
    void Update()
    {
        if (stickCamera)
        {
            m_Camera.transform.position = Vector3.Lerp(m_Camera.transform.position, new Vector3(player.transform.position.x, player.transform.position.y, -10f), cameraLerpSpeed * Time.deltaTime);
        }
        
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
            direction = Direction.Backward;
        }

        //Dash dzia�a tak, �e mierzy czas naci�ni�cia klawisza np. W
        //Od jego ostatniego naci�ni�cia
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
            direction = Direction.Forward;
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
            direction = Direction.Left;
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
            direction = Direction.Right;
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

    //Tutaj sprawdzane warunki maj� da� ko�cow� warto�� speed'a gracza
    //I ustawi� wektor, w kt�rym si� b�dzie porusza�
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

    //Tutaj dzieje si� ca�e poruszanie za pomoc� dodawania
    //si�y (velocity) do rigidbody gracza
    private void FixedUpdate()
    {
        rigidBody.velocity = moveDir * speed;
        if (rigidBody.velocity != Vector2.zero)
        {
            float stamina = player.getStat("Stamina"); //player.playerStats["Stamina"].GetValue();
            if (moveDir != Vector3.zero)
            {
                if (isSprint)
                {
                    stamina -= 0.6f;
                    isSprint = false;
                    setRunningAnimation();
                }
                else if (isDash)
                {
                    float dashVelocity = 3f;
                    Vector3 dashPosition = transform.position + moveDir * dashVelocity;
                    RaycastHit2D raycast = Physics2D.Raycast(transform.position, moveDir, dashVelocity, LayerMask.GetMask("Obstacle"));
                    if (raycast.collider != null)
                    {
                        dashPosition = raycast.point;
                    }
                    setDashingAnimation();
                    rigidBody.MovePosition(dashPosition);
                    stamina -= 30f;
                    StartCoroutine(DashCoolDown());
                    isDash = false;
                }
                else
                {
                    setWalkingAnimation();
                }
            }
            player.setStat("Stamina", stamina);
            //player.playerStats["Stamina"].SetValue(stamina);
        }
        else
        {
            setIdleAnimation();
        }
    }

    private IEnumerator DashCoolDown()
    {
        dashCooldown = true;
        player.SetImmunity(dashCooldown);
        yield return new WaitForSeconds(3f);
        dashCooldown = false;
        player.SetImmunity(dashCooldown);
    }

    //Tu odbywa� si� b�d� wszystkie kolizje na zasadzie Trigger
    private void OnTriggerStay2D(Collider2D collision)
    {
        //S� sprawdzane przez konkretn� warstw� (layer) obiektu z jakim gracz koliduje
        if (collision.gameObject.layer == GameManager.GetLayerNumber("Spiketrap"))
        {
            player.TakeDamage(10f);
        }

    }
    //Ustawienie pozycji bohatera po za�adowaniu nowej sceny
    private void OnLevelWasLoaded(int level)
    {
        transform.position = GameObject.FindWithTag("StartPos").transform.position;
        m_Camera.transform.position = transform.position;
    }

    private void setWalkingAnimation()
    {
        if (direction == Direction.Forward)
        {
            player.ChangeAnimationState(player.getAnimationName(Player.Animation.MoveForward));
        }
        else if (direction == Direction.Right)
        {
            player.ChangeAnimationState(player.getAnimationName(Player.Animation.MoveRight));
        }
        else if (direction == Direction.Left)
        {
            player.ChangeAnimationState(player.getAnimationName(Player.Animation.MoveLeft));
        }
        else
        {
            player.ChangeAnimationState(player.getAnimationName(Player.Animation.MoveBackward));
        }
    }

    private void setRunningAnimation()
    {
        if (direction == Direction.Forward)
        {
            player.ChangeAnimationState(player.getAnimationName(Player.Animation.SprintForward));
        }
        else if (direction == Direction.Right)
        {
            player.ChangeAnimationState(player.getAnimationName(Player.Animation.SprintRight));
        }
        else if (direction == Direction.Left)
        {
            player.ChangeAnimationState(player.getAnimationName(Player.Animation.SprintLeft));
        }
        else
        {
            player.ChangeAnimationState(player.getAnimationName(Player.Animation.SprintBackward));
        }
    }

    private void setIdleAnimation()
    {
        if (direction == Direction.Forward)
        {
            player.ChangeAnimationState(player.getAnimationName(Player.Animation.IdleFront));
        }
        else if (direction == Direction.Right)
        {
            player.ChangeAnimationState(player.getAnimationName(Player.Animation.IdleRight));
        }
        else if (direction == Direction.Left)
        {
            player.ChangeAnimationState(player.getAnimationName(Player.Animation.IdleLeft));
        }
        else
        {
            player.ChangeAnimationState(player.getAnimationName(Player.Animation.IdleBackward));
        }
    }

    private void setDashingAnimation()
    {
        if (direction == Direction.Forward)
        {
            player.ChangeAnimationState(player.getAnimationName(Player.Animation.DashForward));
        }
        else if (direction == Direction.Right)
        {
            player.ChangeAnimationState(player.getAnimationName(Player.Animation.DashRight));
        }
        else if (direction == Direction.Left)
        {
            player.ChangeAnimationState(player.getAnimationName(Player.Animation.DashLeft));
        }
        else
        {
            player.ChangeAnimationState(player.getAnimationName(Player.Animation.DashBackward));
        }
    }

}
