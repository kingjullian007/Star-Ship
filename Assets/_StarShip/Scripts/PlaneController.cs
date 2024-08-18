using UnityEngine;

namespace _StarShip
{
    public class PlaneController : MonoBehaviour
    {
        private float timeLast;
        public GameObject[] listEnemys;
        public GameObject coin;

        public int maxInstanceEnemys;
        // Use this for initialization

        public void CreateEnemy()
        {
            int indexEnemy = Random.Range(0, listEnemys.Length);
            Vector3 posIns = new Vector3(transform.position.x, 0.2f, transform.position.z);
            posIns.x = Random.Range(-RoadManager.Instance.tunnelWidth / 2 + 0.5f, RoadManager.Instance.tunnelWidth / 2 - 0.5f);
            posIns.y = Random.Range(-RoadManager.Instance.tunnelHeight / 2 + 0.5f, RoadManager.Instance.tunnelHeight / 2 - 0.5f); 
            GameObject obj = Instantiate(listEnemys[indexEnemy], posIns, Quaternion.identity);
            obj.name = "Enemy_" + indexEnemy;
            obj.transform.parent = this.transform;
        }

        public void CreateItem()
        {
            Vector3 posIns = new Vector3(transform.position.x, 0.1f, transform.position.z);
            posIns.y = Random.Range(-0.2f, 0.5f);
            posIns.x = Random.Range(-1.0f, 1.0f);
            GameObject obj = Instantiate(coin, posIns, Quaternion.identity);
            obj.name += "object";
            obj.transform.parent = this.transform;
        }
    }

}