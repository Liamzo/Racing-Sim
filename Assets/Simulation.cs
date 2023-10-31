using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour
{
    public TextAsset raceDataCSV; // Add from inspector
    string[,] raceData;
    public List<GameObject> cars; // Add from inspector or find all through code
    public List<GameObject> carsFinished = new List<GameObject>();

    public float timer = 0f;


    // Start is called before the first frame update
    void Start()
    {
        raceData = CSVReader.SplitCsvGrid(raceDataCSV.ToString());

        // first index is which column of the CSV, second index is the row
        // Last J is all null
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime; // Time since last frame

        int currentTimeStamp = (int)timer + 1; // This assumes 1 second intervals. (int) rounds timer down
        int nextTimeStamp = currentTimeStamp + 1;

        for (int i = 0; i < cars.Count; i++) {
            GameObject car = cars[i];

            // This stops errors when a car is finished
            if (carsFinished.Contains(car)) {
                // Car already finished
                continue;
            }

            if (raceData[1 + (2*i), nextTimeStamp] == null) {
                carsFinished.Add(car);
                continue;
            }


            float currentX = float.Parse(raceData[1 + (2*i), currentTimeStamp]);
            float currentY = float.Parse(raceData[2 + (2*i), currentTimeStamp]);
            Vector2 currentPos = new Vector2(currentX,currentY);

            float nextX = float.Parse(raceData[1 + (2*i), nextTimeStamp]);
            float nextY = float.Parse(raceData[2 + (2*i), nextTimeStamp]);
            Vector2 nextPos = new Vector2(nextX, nextY);

            // Lerp is Linear interpolation. Returns a position between Point A and B, based on the float C
            // E.g. If C = 0 then position is A, C = 0.5 then position is in the middlec C = 1 then position is B
            car.transform.position = Vector2.Lerp(currentPos,nextPos, timer - (currentTimeStamp-1));

            // Look where driving
            Vector2 dir = nextPos - currentPos;
            car.transform.up = dir * -1;
        }
    }
}
