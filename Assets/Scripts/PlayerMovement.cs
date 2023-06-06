using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {
    public Animator animator;
    [SerializeField] private AudioSource jumpSound;
    public static AudioSource Source { get; private set; }
    public static PlayerMovement _inst;
    bool alive = true;

    public float speed = 5;
    [SerializeField] Rigidbody rb;

    float horizontalInput;
    public float leftBorder = -2.40f;
    public float rightBorder = 2.40f;

    [SerializeField] float horizontalMultiplier = 2;

    [SerializeField]
   float jumpForcee = 400f; //çift e sil
    [SerializeField]
    LayerMask groundMask; 
    public float speedIncreasePerPoint = 0.1f;


    private Vector2 fingerDown;
    private Vector2 fingerUp;
    public bool detectSwipeOnlyAfterRelease = false; //parmak kaldırıldığı zaman hareket etmesi için check
    public float swipeSensitive = 20f; //kaydırma hassasiyeti
    
    //jumpForce
    public Vector3 jump;
    //public float jumpForce = 0.02f;   
    public float jumpTime;
  

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 10.0f, 0.0f);
    }
                 
   
    private void FixedUpdate()
    {
        if(jumpTime>0 && jumpTime != 0)
        {
            jumpTime-= Time.deltaTime;

        }

        Mathf.Max(transform.position.y,5f);
        if (!alive) return;
        //bu ne amınağ
        //Vector3 forwardMove = transform.forward * speed * Time.fixedDeltaTime;
        transform.Translate(0, 0, speed * Time.deltaTime);

        //Vector3 horizontalMove = transform.right * horizontalInput * speed * Time.fixedDeltaTime * horizontalMultiplier;
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (touch.position.x < Screen.width / 2)
            {
                if (transform.position.x > -3.20f)
                {
                    Vector3.Lerp(transform.position, transform.position += new Vector3(-0.1f, 0, 0), 1f);
                }

            }
            else if (touch.position.x > Screen.width / 2)
            {
                if (transform.position.x < 3.20f)
                {
                    Vector3.Lerp(transform.position,transform.position +=new Vector3(0.1f,0,0),1f);
                    
                }
            }          
        }                
    }    
    
    //Parmağın ekran üzerindeki basıldığı noktayı ve kaldırıldığı noktayı baz alarak hesaplama yapar.
    void checkSwipe()
    {
        //Check Vertical swipe
        if (verticalMove() > swipeSensitive && verticalMove() > horizontalValMove())
        {
            //Debug.Log("Vertical");
            if (fingerDown.y - fingerUp.y > 0)//up swipe
            {
                OnSwipeUp();
             
         
            }
            else if (fingerDown.y - fingerUp.y < 0)//Down swipe
            {
                OnSwipeDown();
            }
            fingerUp = fingerDown;
        }

        //Check Horizontal swipe
        else if (horizontalValMove() > swipeSensitive && horizontalValMove() > verticalMove())
        {
            //Debug.Log("Horizontal");
            if (fingerDown.x - fingerUp.x > 0)//Right swipe
            {
                OnSwipeRight();
            }
            else if (fingerDown.x - fingerUp.x < 0)//Left swipe
            {
                OnSwipeLeft();
            }
            fingerUp = fingerDown;
        }    
        else
        {
            Debug.Log("No Swipe");
        }
    }

    float verticalMove()
    {
        return Mathf.Abs(fingerDown.y - fingerUp.y);
    }

    float horizontalValMove()
    {
        return Mathf.Abs(fingerDown.x - fingerUp.x);
    }
  
    void OnSwipeUp()
    {
        Debug.Log("Swipe UP");
           if(jumpTime==0 || jumpTime<0)
                {
                rb.AddForce(jump, ForceMode.Impulse);               
                jumpTime=1f;                 
          }
    }

    void OnSwipeDown()
    {
        Debug.Log("Swipe Down");
    }

    void OnSwipeLeft()
    {
        //Debug.Log("Swipe Left");
    }

    void OnSwipeRight()
    {
       // Debug.Log("Swipe Right");
    }


     void Update () {
        horizontalInput = Input.GetAxis("Horizontal");

        //Invoke("checkWalls",1);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Jump();
            //animator.SetBool("jump", true);
        }

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerUp = touch.position;
                fingerDown = touch.position;
            }
           
            if (touch.phase == TouchPhase.Moved)
            {
                if (!detectSwipeOnlyAfterRelease)
                {
                    fingerDown = touch.position;
                    checkSwipe();
                }
            }

            //Detects swipe after finger is released
            if (touch.phase == TouchPhase.Ended)
            {
                fingerDown = touch.position;
                checkSwipe();
            }
        }

        if (transform.position.y < -5) {
            Die();
        }
	}

    public void checkWalls()
    {
       Vector3 p = transform.position;
       //Debug.Log("Deger: x " + p.x);
       //Debug.Log("Deger: y " + p.y);
       if (p.x < leftBorder)
       {
            
           p.x = leftBorder  ;
           rb.MovePosition(p);
       }

       if (p.x >= rightBorder)
       {
    
           p.x = rightBorder;
           rb.MovePosition(p);
       }
    }

    public void Die ()
    {
        alive = false;

        //Debug.Log("OYUN BITER");
        
        // Restart the game
        Invoke("Restart", 2);
    }

    void Restart ()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void StartCreditSong()
    {
        jumpSound.Play();
    }

     private void Awake()
    {
        _inst = this;
        Source = jumpSound;
        
    }
   
    

}