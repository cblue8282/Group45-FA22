using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{
    bool isValidGridPos()
    {
        foreach (Transform child in transform)
        {
            Vector2 v = Playfield.roundVec2(child.position);

            // Check if inside border
            if (!Playfield.insideBorder(v))
                return false;
            if (Playfield.grid[(int)v.x, (int)v.y] != null && Playfield.grid[(int)v.x, (int)v.y].parent != transform)
                return false;
        }
        return true;
    }

    void updateGrid()
    {
        // Older children can be killed
        for (int y = 0; y < Playfield.h; ++y)
            for (int x = 0; x < Playfield.w; ++x)
                if (Playfield.grid[x, y] != null)
                    if (Playfield.grid[x, y].parent == transform)
                        Playfield.grid[x, y] = null;

        // New children needed
        foreach (Transform child in transform)
        {
            Vector2 v = Playfield.roundVec2(child.position);
            Playfield.grid[(int)v.x, (int)v.y] = child;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Is the board too full?
        if (!isValidGridPos())
        {
            Debug.Log("GAME OVER!");
            Destroy(gameObject);
        }
    }

    // Time since last gravity tick
    float lastFall = 0;

    // Update is called once per frame
    void Update()
    {
        // Move left
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //Change pos
            transform.position += new Vector3(-1, 0, 0);

            // is it valid?
            if (isValidGridPos())
                updateGrid();
            else
                //Revertion
                transform.position += new Vector3(1, 0, 0);
        }

        //Slide to the right
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //Change pos
            transform.position += new Vector3(1, 0, 0);

            // is it valid?
            if (isValidGridPos())
                updateGrid();
            else
                //Revertion
                transform.position += new Vector3(-1, 0, 0);
        }

        //Rotation
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //Change pos
            transform.position += new Vector3(0, 0, -90);

            // is it valid?
            if (isValidGridPos())
                updateGrid();
            else
                //Revertion
                transform.position += new Vector3(0, 0, 90);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Time.time - lastFall >= 1)
        {
            //Change pos
            transform.position += new Vector3(0, -1, 0);

            // is it valid?
            if (isValidGridPos())
            {
                updateGrid();
            }
            else
            {
                //Revertion
                transform.position += new Vector3(0, -1, 0);

                //Delete the filled row
                Playfield.deleteFullRows();

                // New block needed
                FindObjectOfType<Spawner>().spawnNext();

                // disable script
                enabled = false;
            }
            lastFall = Time.time;
        }
    }
}
