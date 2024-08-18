using UnityEngine;

namespace _StarShip
{
    public class RoadManager : MonoBehaviour
    {
        static public RoadManager Instance { get; private set; }

        public GameObject partOfRoad;
        [Header("Reference Objects")]
        public int lenghtRoad;

        [Range(2, 5)]
        public float tunnelWidth = 4f;// for move

        [Range(2, 5)]
        public float tunnelHeight = 4f;// for move

        [HideInInspector]
        public float posX;

        [HideInInspector]
        public float posY;

        [HideInInspector]
        public Vector3 posBegin = Vector3.zero;

        private bool flagX = true;
#pragma warning disable 0414
        private bool flagY = true;
        private float timeLast;
#pragma warning restore 0414

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                DestroyImmediate(Instance.gameObject);
                Instance = this;
            }
        }

        void Start()
        {
            // position of first plane have instance
            posBegin = new Vector3(0, 0, -5);
            CreateRoad();
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.Instance.GameState == GameState.Playing)
            {
                if (Time.time - timeLast > 0.03f)
                {
                    timeLast = Time.time;
                    
                    // move to Left
                    if (!flagX)
                        MovePosXLeft();
                    
                    // move to Right
                    if (flagX)
                        MovePosXRight();
                }

            }
        }
        void CreateRoad()
        {
            posX = Random.Range(-20, 20);

            posY = 5;

            for (int i = 0; i < lenghtRoad; ++i)
            {
                GameObject obj = Instantiate(partOfRoad, posBegin, Quaternion.identity);
                obj.transform.parent = this.transform;
                obj.name = "partOfRoad_" + i;

                // Instance Enemy
                if (i > 5)
                {
                    if (Random.Range(0, 1.01f) <= GameManager.Instance.coinFrequency)
                        obj.transform.GetChild(0).GetComponent<PlaneController>().CreateItem();
                    else
                        obj.transform.GetChild(0).GetComponent<PlaneController>().CreateEnemy();
                }

                posBegin.z += 10;
            }
        }
        void MovePosXLeft()
        {
            posX -= Time.deltaTime;

            if (posX < -20)
                flagX = true;// collided with Left
        }
        void MovePosXRight()
        {
            posX += Time.deltaTime;

            if (posX > 20)
                flagX = false;
        }
        void MovePosYTop()
        {
            posY += Time.deltaTime;

            if (posY > 3)
                flagY = true;// collided with Top
        }
        void MovePosYDow()
        {
            posY -= Time.deltaTime;

            if (posY < -4)
                flagY = false;// collided with Top
        }

    }
}
