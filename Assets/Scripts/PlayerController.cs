using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Experimental.UIElements;


public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private bool inverse = true;

    private bool playerOverTarget = false;

    // The speed at which the character moves, in units per second.
    public float moveSpeed = 2f;

    // How wide/tall each grid square is, in units. This can only be an integer, so grid sizes such as 1.5 aren't possible. (The grid isn't actually shown; that would be the function of whatever graphics are being used.)
    public float gridSize = 1.0f;

    public int steps = 0;

    private bool isMoving = false;
    private float factor = 1f;

    // This Tilemap should be a seperate Tilemap from the ground/background one, place here the tiles where wall and collidable objects should be
    public Tilemap tilemap_Obstacle;


    public Vector3Int GetCurrentGridPosition()
    {
        return new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
    }

    public Vector3Int CalculateNextTile(Vector2 input, out bool sliding)
    {
        sliding = false;

        if (inverse)
            input *= -1;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(startPosition.x + System.Math.Sign(input.x) * gridSize,
            startPosition.y + System.Math.Sign(input.y) * gridSize, startPosition.z);

        Vector3Int gridPos = new Vector3Int((int)endPosition.x, (int)endPosition.y, (int)endPosition.z);

        CustomTile endTile = tilemap_Obstacle.GetTile<CustomTile>(gridPos);
        if (endTile.type == "obstacle")
        {
            gridPos = new Vector3Int((int)startPosition.x, (int)startPosition.y, (int)startPosition.z);
        }
        else if (endTile.type == "slider")
        {
            sliding = true;

            Vector3Int startGridPos = tilemap_Obstacle.WorldToCell(startPosition);
            Vector3Int direction = gridPos - startGridPos;
            do
            {
                gridPos += direction;
                endTile = tilemap_Obstacle.GetTile<CustomTile>(gridPos);
            }
            while (!(endTile.type == "obstacle") && !(endTile.type == "trap"));
            if (endTile.type == "obstacle")
                gridPos -= direction;
        }

        return gridPos;
    }

    public void move(Vector3Int endGridPos, bool comeback, bool sliding)
    {
        if (!isMoving)
        {
            isMoving = true;
            Vector3 startPosition = transform.position;
            Vector3 endPosition = tilemap_Obstacle.CellToLocal(endGridPos);
            CustomTile endTile = tilemap_Obstacle.GetTile<CustomTile>(endGridPos);

            if (sliding && comeback)
            {
                Vector3 direction = (endPosition - startPosition).normalized;
                endGridPos -= Vector3Int.FloorToInt(direction);
                endPosition = tilemap_Obstacle.CellToLocal(endGridPos);
                endTile = tilemap_Obstacle.GetTile<CustomTile>(endGridPos);
                comeback = false;
            }

            if (startPosition != endPosition)
            {
                playerOverTarget = endTile.type == "target";
                StartCoroutine(LerpMove(startPosition, endPosition, endTile, comeback));
            }
            else
            {
                EventManager.TriggerEvent("ready");
                isMoving = false;
            }
        }
    }

    public IEnumerator LerpMove(Vector3 start, Vector3 end, CustomTile endTile, bool comeback)
    {
        float t = 0;
        if(comeback)
        {
            while (t < 0.5f)
            {
                t += Time.deltaTime * (moveSpeed / gridSize) * factor;
                transform.position = Vector3.Lerp(start, end, t);
                yield return null;
            }
            while (t < 1f)
            {
                t += Time.deltaTime * (moveSpeed / gridSize) * factor;
                transform.position = Vector3.Lerp(end, start, t);
                yield return null;
            }
        } else
        {
            while (t < 1f)
            {
                t += Time.deltaTime * (moveSpeed / gridSize) * factor;
                transform.position = Vector3.Lerp(start, end, t);
                yield return null;
            }
        }

        if (endTile.type == "trap")
        {
            EventManager.TriggerEvent("die");
        }
        if (endTile.type == "target")
        {
            EventManager.TriggerEvent("finish");
        }

        isMoving = false;
        steps++;
        EventManager.TriggerEvent("step");
        EventManager.TriggerEvent("ready");

        yield return null;
    }

    public bool IsPlayerOverTarget()
    {
        return playerOverTarget;
    }
}