using UnityEngine;

public class RandomPoints : MonoBehaviour, IInitializeVariables
{
    //Creat Point Random Around this Object
    private Vector3 pointRandomAroundThisObject;
    private float radiusOut;
    private float radiusIn;
    // instalize Area Clamp Position
    private float maxPosX;
    private float maxPosZ;
    private float minPosX;
    private float minPosZ;
    private bool isFindDone;
    private float RadiusObstacle;
    // Start is called before the first frame update
    void Start()
    {
        InitializeVariables();
    }
    public Vector3 GetPointRandomAroundThisObject()
    {
        while (!isFindDone)
        {
            Vector3 directionRandom = new Vector3(Random.Range(-1000, 1000), 0, Random.RandomRange(-1000, 1000)).normalized;
            float rangeRandom = Random.Range(radiusIn, radiusOut);
            pointRandomAroundThisObject = transform.position + rangeRandom * directionRandom;
            // Clamp Position
            var pos = pointRandomAroundThisObject;
            pos.x = Mathf.Clamp(pointRandomAroundThisObject.x, minPosX, maxPosX);
            pos.z = Mathf.Clamp(pointRandomAroundThisObject.z, minPosZ, maxPosZ);
            pointRandomAroundThisObject = pos;
            // check if Point into range Obstacle--> find Point again
            #region check if Point into range Obstacle
            if (GameManager.Instance.ListObstacle.Count > 0)
            {
                bool isContinue = false;
                foreach (var _obstacle in GameManager.Instance.ListObstacle)
                {
                    float distancePointToObstacle = Vector3.Distance(pointRandomAroundThisObject, _obstacle.transform.position);
                    if (distancePointToObstacle < RadiusObstacle)
                    {
                        isContinue = true;
                        break;
                    }
                }
                if (isContinue)
                {
                    continue;
                }
                isFindDone = true;
            }
            #endregion
        }
        // reset isFindDone for next times
        isFindDone = false;
        return pointRandomAroundThisObject;
    }

    public void InitializeVariables()
    {
        radiusOut = 10;
        radiusIn = 3;
        maxPosX = 20;
        maxPosZ = 16;
        minPosX = -20;
        minPosZ = -16;
        isFindDone = false;
        RadiusObstacle = 3;
    }
}
