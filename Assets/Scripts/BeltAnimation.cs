using UnityEngine;

public class BeltAnimation : MonoBehaviour
{
  
        [SerializeField] private float scrollSpeed = 0.4f;
        private Material mat;
        private void Start()
        {
            mat = GetComponent<Renderer>().material;
        }

        private void Update()
        {
            mat.mainTextureOffset -= new Vector2(scrollSpeed*Time.deltaTime, 0);
        }
    
}
