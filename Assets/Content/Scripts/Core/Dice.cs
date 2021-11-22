using Content.Scripts.Utilities;
using UnityEngine;

namespace Content.Scripts.Core
{
    public class Dice : MonoBehaviour
    {
        [SerializeField] private float startRaycastUpDistance = 10;

        public Rigidbody diceRigidbody;
        
        public int GetCurrentNumber()
        {
            RaycastHit hitInfo;
            var raycastPos = transform.position + new Vector3(0, startRaycastUpDistance);
            Physics.Raycast(new Ray(raycastPos,Vector3.down), out hitInfo, 100);
            if (hitInfo.collider)
            {
                var cubeFacet = hitInfo.collider.gameObject.GetComponent<CubeFacet>();
                if (cubeFacet != null)
                {
                    return cubeFacet.number;
                }
            }

            return 0;
        }
    }
}
