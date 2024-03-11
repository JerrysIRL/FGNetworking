using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Extensions
{
    public static class TransformHelpers
    {
        public static void SetRandomPositionOnScreen(this Transform transform)
        {
            var camera = Camera.main;
            if (camera == null)
                throw new NullReferenceException("Main camera is null!");
            
            float screenHorizontalSize = camera.orthographicSize * camera.aspect - 0.2f;
            float screenVerticalSize = camera.orthographicSize - 0.2f;
            
            float xPosition = Random.Range(-screenHorizontalSize, screenHorizontalSize);
            float yPosition = Random.Range(-screenVerticalSize, screenVerticalSize);

            transform.position = new Vector3(xPosition, yPosition, 0);
        }
    }
}
