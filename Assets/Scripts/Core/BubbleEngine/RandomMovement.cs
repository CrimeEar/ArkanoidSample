using UnityEngine;

public class RandomMovement
{
    private const int MOVABLE_POSITION_COUNT = 3;

    private BubblesHandler bubblesHandler;
    private CircleObject circleObject;

    private Vector2[] randomMovePositions;
    private Vector2 startLocalPosition;
    private Vector2 newLocalPosition;

    private float changeDirectionTime;
    private float moveSpeed;
    private float currentTime;

    private int currentMovePositionIndex;

    public RandomMovement(BubblesHandler bubbleHandler, CircleObject circleObject, float startRadius, float speed, float movementOffset, Vector2 delayRange)
    {
        this.bubblesHandler = bubbleHandler;
        this.circleObject = circleObject;
        this.moveSpeed = speed;

        this.startLocalPosition = circleObject.transform.localPosition;
        this.changeDirectionTime = Random.Range(delayRange.x, delayRange.y);
        this.randomMovePositions = new Vector2[MOVABLE_POSITION_COUNT];

        for (int i = 0; i < randomMovePositions.Length; i++)
        {
            randomMovePositions[i] = new Vector2(Random.Range(-movementOffset, movementOffset), Random.Range(-movementOffset, movementOffset));
        }

        newLocalPosition = startLocalPosition + randomMovePositions[currentMovePositionIndex];
    }

    public void CustomUpdate()
    {
        MoveUpdate();
    }

    public void SetPosition(Vector2 position)
    {
        circleObject.transform.localPosition = position;
    }

    private void MoveUpdate()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= changeDirectionTime)
        {
            currentTime = 0;
            currentMovePositionIndex++;
            currentMovePositionIndex %= randomMovePositions.Length;
            newLocalPosition = startLocalPosition + randomMovePositions[currentMovePositionIndex];
            moveSpeed = 0.5f;
        }

        circleObject.transform.localPosition = Vector2.Lerp(circleObject.transform.localPosition, newLocalPosition, Time.deltaTime * moveSpeed);
    }
}
