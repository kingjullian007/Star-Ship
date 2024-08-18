using UnityEngine;

namespace _StarShip
{
    public class SetShaderForPartRoad : MonoBehaviour
    {
        private float poX;
        private MeshRenderer meshRenderer;
        private float OffShaderX;
        private float OffShaderY;
        public Vector4 vector;
        private void Start()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            OffShaderX = -20;  
        }
        private void Update()
        {
            OffShaderX = RoadManager.Instance.posX;
            OffShaderY = RoadManager.Instance.posY;
            vector = new Vector4(OffShaderX, OffShaderY, 0, 0);
            if (meshRenderer != null)
            {
                meshRenderer.material.SetVector("_OffSetV", vector);
            }
        }

    }
}
