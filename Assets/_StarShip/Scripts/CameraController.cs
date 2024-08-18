using System.Collections;
using UnityEngine;

namespace _StarShip
{
    public class CameraController : MonoBehaviour
    {
        static public CameraController Instance;
        public Transform playerTransform;
        private Vector3 velocity = Vector3.zero;
        private Vector3 originalDistance;
        private float timeLast;
        //private float distanceTime = 0.2f;

        [Header("Camera Follow Smooth-Time")]
        public float smoothTime = 0.01f;

        [Header("Shaking Effect")]
        // How long the camera shaking.
        public float shakeDuration = 0.1f;
        // Amplitude of the shake. A larger value shakes the camera harder.
        public float shakeAmount = 0.2f;
        public float decreaseFactor = 0.3f;
        [HideInInspector]
        public Vector3 originalPos;

        private float currentShakeDuration;
        private float currentDistance;

        //[Header("Camera compared to Player")]
        //Elevation compared to Player
        //private float elvationCTP;

        //Distance compared to Player
        //private float distanceCTP;
        void OnEnable()
        {
            CharacterScroller.ChangeCharacter += ChangeCharacter;
        }
        private void Awake()
        {
            if (!Instance)
                Instance = this;
        }
        void OnDisable()
        {
            CharacterScroller.ChangeCharacter -= ChangeCharacter;
        }

        void Start()
        {
            StartCoroutine(WaitingPlayerController());
        }

        private void LateUpdate()
        {
            if (GameManager.Instance.GameState == GameState.Playing && playerTransform != null)
            {
                Vector3 pos = playerTransform.position + originalDistance;
                Vector3 posHor = new Vector3(transform.position.x, transform.position.y, pos.z);
                GameObject obj = GameObject.Find("RoadManager");
                if (obj != null)
                {
                    transform.position = posHor;
                    transform.position = Vector3.SmoothDamp(transform.position, new Vector3(Mathf.Clamp(pos.x, (-RoadManager.Instance.tunnelWidth / 2) + 0.5f, (RoadManager.Instance.tunnelWidth / 2) - 0.5f),
                        Mathf.Clamp(pos.y, -RoadManager.Instance.tunnelHeight / 4, RoadManager.Instance.tunnelHeight / 4), pos.z), ref velocity, smoothTime);
                }
                else
                {
                    transform.position = Vector3.SmoothDamp(transform.position, pos, ref velocity, smoothTime);
                }
            }
        }

        public void FixPosition()
        {
            transform.position = playerTransform.position + originalDistance;
        }

        public void ShakeCamera()
        {
            StartCoroutine(Shake());
        }

        IEnumerator Shake()
        {
            originalPos = transform.position;
            currentShakeDuration = shakeDuration;
            while (currentShakeDuration > 0)
            {
                transform.position = originalPos + Random.insideUnitSphere * shakeAmount;
                currentShakeDuration -= Time.deltaTime * decreaseFactor;
                yield return null;
            }
            transform.position = originalPos;
        }

        void ChangeCharacter(int cur)
        {
            StartCoroutine(WaitingPlayerController());
        }

        IEnumerator WaitingPlayerController()
        {
            yield return new WaitForEndOfFrame();
            while (GameManager.Instance.playerController == null)
            {
                yield return null;
            }

            playerTransform = GameManager.Instance.playerController.transform;
            originalDistance = transform.position - playerTransform.transform.position;
        }

    }
}