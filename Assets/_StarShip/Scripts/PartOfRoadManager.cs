using UnityEngine;

namespace _StarShip
{
    public class PartOfRoadManager : MonoBehaviour
    {
        public GameObject left, right, top, bot;

        private void Start()
        {
            // set up bound of path
            left.transform.localPosition = new Vector3(-RoadManager.Instance.tunnelWidth / 2, 0, 0);
            right.transform.localPosition = new Vector3(RoadManager.Instance.tunnelWidth / 2, 0, 0);
            top.transform.localPosition = new Vector3(0, RoadManager.Instance.tunnelHeight / 2, 0);
            bot.transform.localPosition = new Vector3(0, -RoadManager.Instance.tunnelHeight / 2, 0);
        }
        private void FixedUpdate()
        {
            if (GameManager.Instance.GameState == GameState.Playing && GameManager.Instance.playerController != null)
            {
                if (GameManager.Instance.playerController.transform.position.z - transform.position.z > 20)
                {
                    Vector3 temp = transform.position;
                    temp.z = RoadManager.Instance.posBegin.z;
                    transform.position = temp;
                    GameObject obj;
                    if (transform.GetChild(0).childCount != 0)
                    {
                        obj = transform.GetChild(0).GetChild(0).gameObject;
                        Destroy(obj);
                    }

                    if (Random.Range(0, 0.99f) <= GameManager.Instance.coinFrequency)
                    {
                        transform.GetChild(0).gameObject.GetComponent<PlaneController>().CreateItem();
                    }
                    else
                    {
                        transform.GetChild(0).gameObject.GetComponent<PlaneController>().CreateEnemy();
                    }

                    RoadManager.Instance.posBegin.z += 9.97f;
                }
            }
        }
    }
}
