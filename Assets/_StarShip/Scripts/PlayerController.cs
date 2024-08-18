using System.Collections.Generic;
using UnityEngine;

namespace _StarShip
{
    public class PlayerController : MonoBehaviour
    {
        public static event System.Action PlayerDied;
        static public PlayerController Instance;

        [HideInInspector]
        public float speed;

        public ParticleSystem particle;

        private int countItemSpeedUpTo;//  
        private float timeLast;
        private Rigidbody rigiBody;
        private Color color;
        private float sizeX;
        private float sizeY;
        private float sizeXRoad;
        private float sizeYRoad;
        private Direction direction;
        private float prePosZ;
        private float originalPosZ;
        private bool isRun = false;

        void OnEnable()
        {
            GameManager.GameStateChanged += OnGameStateChanged;
        }

        void OnDisable()
        {
            GameManager.GameStateChanged -= OnGameStateChanged;
        }
        private void Awake()
        {
            if (!Instance)
                Instance = this;
        }
        void Start()
        {
            // Setup
            timeLast = 0;
            speed = 0;
            Character character = transform.GetComponent<Character>();
            particle = GameObject.FindGameObjectWithTag("ParticleSystem").GetComponent<ParticleSystem>();
            var main = particle.main;
            main.startColor = new Color(character.charColor.r, character.charColor.g, character.charColor.b, 1);
            //color = gameObject.GetComponent<Renderer>().material.color;

            countItemSpeedUpTo = 0;
            rigiBody = GetComponent<Rigidbody>();

            sizeX = transform.GetChild(0).GetComponent<Renderer>().bounds.size.x;
            sizeY = transform.GetChild(0).GetComponent<Renderer>().bounds.size.y;
            sizeXRoad = RoadManager.Instance.tunnelWidth;
            sizeYRoad = RoadManager.Instance.tunnelHeight;
            speed = GameManager.Instance.minMoveSpeed;
            for(int i = 0; i < transform.childCount; i++)
            {
                TrailRenderer lineReneder = transform.GetChild(i).GetComponent<TrailRenderer>();
                if (lineReneder != null)
                {
                    lineReneder.startColor = new Color(character.charColor.r, character.charColor.g, character.charColor.b, 0.7f);
                    lineReneder.endColor = new Color(character.charColor.r, character.charColor.g, character.charColor.b, 0);
                }
            }
            Invoke("DelayRun", 0.1f);
        }

        void DelayRun()
        {
            isRun = true;
        }
        // Update is called once per frame
        void Update()
        {
            if (GameManager.Instance.GameState == GameState.Playing && isRun)
            {
                if(GameManager.Instance.ScoreMode == ScoreMode.Distance &&
                        transform.position.z - prePosZ >= GameManager.Instance.DistanceGetScore)
                {
                    ScoreManager.Instance.AddScore(GameManager.Instance.PlusScore);
                    prePosZ = transform.position.z;
                }
                MoveByTouch();
            }
        }
        // Listens to changes in game state
        void OnGameStateChanged(GameState newState, GameState oldState)
        {
            if (newState == GameState.Playing)
            {
                // Do whatever necessary when a new game starts
                rigiBody = GetComponent<Rigidbody>();
                originalPosZ = transform.position.z;
                speed = GameManager.Instance.minMoveSpeed;
                // rigiBody.useGravity = true;
            }
        }
        // Calls this when the player dies and game over
        public void Die()
        {
            // Fire event
            if (PlayerDied != null)
            {
                PlayerDied();
            }

        }
        void MoveByTouch()
        {
            transform.GetChild(0).GetComponent<Renderer>().material.color = Color.white;
            transform.GetChild(1).GetComponent<Renderer>().material.color = Color.white;
            if (GameManager.Instance.AccelMode == AccelerationMode.Distance)
            {
                speed = GameManager.Instance.minMoveSpeed + (transform.position.z - originalPosZ) * GameManager.Instance.accelIndex;
                speed = Mathf.Clamp(speed, GameManager.Instance.minMoveSpeed, GameManager.Instance.maxMoveSpeed);
            }

            //Direction direction = InputManager.Instance.direction;
            transform.position += transform.forward * Time.deltaTime * speed;

            Vector3 temp = transform.position;

            // move left/ right
            float inputs = InputManager.Instance.touchX;
            if (inputs != 0)
                temp += new Vector3(1, 0, 0) * inputs * Time.deltaTime * speed;
            else
                temp += Vector3.zero;

            temp.x = Mathf.Clamp(temp.x, -sizeXRoad / 2 + sizeX / 2, sizeXRoad / 2 - sizeX / 2);
            temp.y = Mathf.Clamp(temp.y, -sizeYRoad / 2 + sizeY, sizeYRoad / 2 - sizeY);
            transform.position = temp;
        }

        private void OnCollisionEnter(Collision collision)
        {
            GameObject obj = collision.gameObject;
            if (obj.tag == "Enemy")
            {
                PlayerDied();
                this.gameObject.transform.position += new Vector3(0, 0, -speed * Time.deltaTime);

                particle.transform.position = transform.position;

                //particle.GetComponent<Renderer>().material.color = color;
                particle.GetComponent<ParticleSystem>().Play();
                Destroy(gameObject, 0.05f);

                CameraController.Instance.ShakeCamera();
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            GameObject obj = other.gameObject;
            if (obj.tag == "ItemSpeedUp")
            {
                Destroy(obj);
                ++countItemSpeedUpTo;
                timeLast = Time.time;
            }
            if (obj.tag == "Gold")
            {
                Destroy(obj);
                SoundManager.Instance.PlaySound(SoundManager.Instance.coin);
                CoinManager.Instance.AddCoins(1);

            }
            if (obj.tag == "Enemy")
            {
                if (GameManager.Instance.ScoreMode == ScoreMode.Obstacle)
                    ScoreManager.Instance.AddScore(1);

                if (GameManager.Instance.AccelMode == AccelerationMode.Obstacle)
                {
                    speed += GameManager.Instance.accelIndex;
                    speed = Mathf.Clamp(speed, GameManager.Instance.minMoveSpeed, GameManager.Instance.maxMoveSpeed);
                }
            }
        }
    }

}