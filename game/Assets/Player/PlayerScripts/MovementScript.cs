using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{

    [SerializeField] private Camera m_Camera;
    [SerializeField] private float cameraLerpSpeed;
    public bool stickCamera = true; // jeœli false to kamera zwolniona i nie bedzie sie teleportowac do gracza

    //enumy dla kierunku
    private enum Direction
    {
        Forward,
        Right,
        Left,
        Backward
    }

    private Player player;
    private Rigidbody2D Rigidbody;
    private Vector3 MoveDirection;
    private float speed;
    private bool IsDash = false;
    private bool IsSprint = false;
    private bool DashCooldown = false;
    public bool CanMove = true;
    private Direction direction;

    //dashowe zmienne
    private float lastClickedTimeW;
    private float lastClickedTimeS;
    private float lastClickedTimeA;
    private float lastClickedTimeD;

    //Wyeliminowanie bugu wielu postaci po za³adowaniu sceny
    private GameObject[] players;
    private void Start()
    {
        DontDestroyOnLoad(m_Camera);
        player = GetComponent<Player>();
        Rigidbody = GetComponent<Rigidbody2D>();
        MoveDirection = Vector3.zero;
    }
    
    //Update pobiera input z klawiatury odnoœnie poruszania siê
    void Update()
    {
        StickCamera();
        player.ChangeStamina(0.01f);
        ReadInput();
    }



    private void FixedUpdate()
    {
        if (CanMove)
        {
            MoveTo();
        }
    }

    //OpóŸniacze
    private IEnumerator Dash(Vector3 dashPosition)
    {
        CanMove = false;
        Rigidbody.MovePosition(Vector3.Lerp(transform.position, dashPosition, 0.5f));
        setDashingAnimation();
        yield return new WaitForSeconds(0.30f);
        Rigidbody.MovePosition(dashPosition);
        CanMove = true;
    }

    private IEnumerator DashCoolDown()
    {
        DashCooldown = true;
        player.SetImmunity(DashCooldown);
        yield return new WaitForSeconds(3f);
        DashCooldown = false;
        player.SetImmunity(DashCooldown);
    }

    //Do wywalenia
    /*private void OnTriggerStay2D(Collider2D collision)
    {
        //S¹ sprawdzane przez konkretn¹ warstwê (layer) obiektu z jakim gracz koliduje
        if (collision.gameObject.layer == GameManager.GetLayerNumber("Spiketrap"))
        {
            player.TakeDamage(10f);
        }

    }*/
    //Ustawienie pozycji bohatera po za³adowaniu nowej sceny
    private void OnLevelWasLoaded(int level)
    {
        transform.position = GameObject.FindWithTag("StartPos").transform.position;
        m_Camera.transform.position = transform.position;
    }

    //Ustawianie konkretnych animacji
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

    private void ReadInput()
    {
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

    //Tutaj dzieje siê ca³e poruszanie za pomoc¹ dodawania
    //si³y (velocity) do rigidbody gracza
    private void MoveTo()
    {

        if (MoveDirection != Vector3.zero)
        {
            if (IsDash)
            {
                float dashVelocity = 2.5f;
                Vector3 dashPosition = transform.position + MoveDirection * dashVelocity;
                RaycastHit2D raycast = Physics2D.Raycast(transform.position, MoveDirection, dashVelocity, LayerMask.GetMask("Obstacle"));
                if (raycast.collider != null)
                {
                    dashPosition = raycast.point;
                }

                StartCoroutine(Dash(dashPosition));
                StartCoroutine(DashCoolDown());
                player.ChangeStamina(-30f);
                IsDash = false;
            }
            else
            {
                if (IsSprint)
                {
                    player.ChangeStamina(-0.6f);
                    IsSprint = false;
                    setRunningAnimation();
                }

                else
                {
                    setWalkingAnimation();
                }

                Rigidbody.velocity = MoveDirection * speed;
            }
        }
        else
        {
            Rigidbody.velocity = Vector3.zero;
            setIdleAnimation();
        }
    }

    //Tutaj sprawdzane warunki maj¹ daæ koñcow¹ wartoœæ speed'a gracza
    //I ustawiæ wektor, w którym siê bêdzie porusza³
    private void Move(Vector3 direction, bool sprint, bool dash)
    {
        MoveDirection = direction;
        float stamina = player.GetStats().GetValue(PlayerStatistics.Stat.Stamina);
        if (dash && !DashCooldown && stamina >= 30f)
        {
            IsDash = true;
        }
        else if (sprint && stamina >= 0)
        {
            speed = 12f;
            IsSprint = true;
        }
        else
        {
            speed = 6f;
        }

    }

    private void StickCamera()
    {
        if (stickCamera)
        {
            m_Camera.transform.position = Vector3.Lerp(m_Camera.transform.position, new Vector3(player.transform.position.x, player.transform.position.y, -10f), cameraLerpSpeed * Time.deltaTime);
        }
    }
}
