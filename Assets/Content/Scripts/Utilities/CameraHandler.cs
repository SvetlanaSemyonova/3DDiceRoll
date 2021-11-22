using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Content.Scripts.Utilities
{
    public class CameraPoint
    {
        public Transform posTransform;
    }
    
    public class CameraHandler : MonoBehaviour
    {
        [SerializeField] private List<Transform> cameraPoints = new List<Transform>();

        private int currentCameraPosIndex = 0;

        public Transform GetNextPoint()
        {
            if (currentCameraPosIndex + 1 < cameraPoints.Count)
            {
                return cameraPoints[currentCameraPosIndex + 1];
            }
            else
            {
                return cameraPoints[0];
            }
        }

        public Transform GetLastPoint()
        {
            if (currentCameraPosIndex > cameraPoints.Count)
            {
                return cameraPoints[currentCameraPosIndex ];
            }
            return cameraPoints[0];
        }
        
        public void IncreaseCurrentCameraPosIndex()
        {
            currentCameraPosIndex +=1;
        }

        public void DecreaseCurrentCameraPosIndex()
        {
            if (currentCameraPosIndex > 0)
            {
                currentCameraPosIndex -=1;
            }
        }
        
        public void SetCurrentCameraPosIndex(int posIndex)
        {
            currentCameraPosIndex = posIndex;
        }
    }
}
