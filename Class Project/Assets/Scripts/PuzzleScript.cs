using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PuzzleScript : MonoBehaviour
{
    //SCRIPT FOR PUZZLES, CONNECT IT TO THE CHARACTER SCRIPTS WITH PUZZLES
    //PUT IT INTO THE CHANGE/TURNIN SECTION PROBABLY

    //using a guide online for a lot of this section
    //specifically videos:
    //https://www.youtube.com/watch?v=OFC_UUaS4gs Let's Create: Jigsaw Puzzle in Unity(Part 2: Creating Pieces)
    //https://www.youtube.com/watch?v=2ZH0GaYbrc8 Let's Create: Jigsaw Puzzle in Unity(Part 3: Scattering the pieces)
    //https://www.youtube.com/watch?v=bNBS8ZuzgZo Let's Create: Jigsaw Puzzle in Unity(Part 4: Dragging and snapping)

    [Header("Game Elements")]
    [SerializeField] int numOfPieces = 4;//for 16 puzzle pieces, a 64 by 64 puzzle needs to be split into 16 by 16 chunks which will give 4 by 4 across the whole image
    [SerializeField] Transform gameHolder;
    [SerializeField] Transform piecePrefab;
    [SerializeField] GameObject basePicture;
    public bool finishedPuzzle = false;//use to interact with the other scripts as needed
    [SerializeField] TimerScript timer;
    [SerializeField] GameObject timerText;//set it to true
    [SerializeField] Player player;
    [SerializeField] string questObject;//set to the same thing in the specified character
    //so for Green Lamb, it will be "Completed Tile"

    [Header("Puzzle Pieces")]
    [SerializeField] Texture2D jigsawTexture;
    public List<Transform> pieces;
    private Vector2Int dimensions;
    private float width;
    private float height;
    private Transform draggingPiece = null;//use for dragging pieces around
    private Vector3 offset;
    private int piecesCorrect;

    public void StartGame()
    {
        pieces = new List<Transform>();

        dimensions = GetDimensions(jigsawTexture, numOfPieces);

        CreateJigsawPieces(jigsawTexture);
        Scatter();//scatter is part 3, but only used small portion of video compared to other 2 videos
        
        piecesCorrect = 0;//starting a new game, so should be at 0
    
        timerText.SetActive(true);
        StartCoroutine(timer.CountDownRoutine("puzzle"));
    }

    Vector2Int GetDimensions(Texture2D jigsawTexture, int difficulty)
    {
        //don't technically need this for my puzzle as the x and y dimensions are the same, but might as well put it here for now
        //WILL PROBABLY CHANGE ALOT OF THESE THINGS AS THE PUZZLE SCRIPT PROGRESSES
        Vector2Int dimensions = Vector2Int.zero;
        if(jigsawTexture.width < jigsawTexture.height)
        {
            dimensions.x = difficulty;
            dimensions.y = (difficulty * jigsawTexture.height) / jigsawTexture.width;
        }
        else if(jigsawTexture.width > jigsawTexture.height)
        {
            dimensions.x = (difficulty * jigsawTexture.width) / jigsawTexture.height;
            dimensions.y = difficulty;
        }
        else
        {
            dimensions.x = difficulty;
            dimensions.y = difficulty;
        }

        return dimensions;
    }

    void CreateJigsawPieces(Texture2D jigsawTexture)
    {
        height = 1f/dimensions.y;
        width = 1f/dimensions.x;//in this case, the puzzle will always be same x and y dimensions

        for(int row = 0; row < dimensions.y; row++)
        {
            for(int col = 0; col < dimensions.x; col++)
            {
                Transform piece = Instantiate(piecePrefab, gameHolder);
                piece.localPosition = new Vector3((-width*dimensions.x/2)+(width*col)+(width/2),(-height*dimensions.y/2)+(height*row)+(height/2),-1);
                piece.localScale = new Vector3(width,height,1f);
                //useful for debugging, may keep or may not line below
                piece.name = $"Piece {(row*dimensions.x)+col}";
                pieces.Add(piece);

                //need to assign correct part of texture to correct jigsaw piece
                float width1 = 1f/dimensions.x;
                float height1 = 1f/dimensions.y;

                Vector2[] uv = new Vector2[4];
                uv[0] = new Vector2(width1 * col, height1 * row);
                uv[1] = new Vector2(width1*(col+1), height1*row);
                uv[2] = new Vector2(width1*col, height1*(row+1));
                uv[3] = new Vector2(width1 * (col+1), height1 *(row+1));

                Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                mesh.uv = uv;

                piece.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", jigsawTexture);
            }
        }
    }

    void Scatter()
    {   //place pieces randomly in visible area

        foreach(Transform piece in pieces)
        {
            float x = Random.Range(gameHolder.position.x-5, gameHolder.position.x+5);
            float y = Random.Range(gameHolder.position.y-5, gameHolder.position.y+5);
            piece.position = new Vector3(x,y,-1);
        }

    }

    //for the mouse button movements and the like
    void Update()
    {

        if(finishedPuzzle)
        {
            timerText.SetActive(false);//just disable the time
        }
        else
        {
            //cannot for the life of me get mousebuttondown working, so just getting this working as best I can with getmousebutton
            //FIXED ABOVE COMMENT, had everything under the if statement below so it wasn't working correctly
            if(Input.GetMouseButtonDown(0))//the left button specifically
            {
                //use raycast to see if click on any of the pieces
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if(hit && hit.transform.CompareTag("Puzzle Piece"))
                {//do need to make sure it is a puzzle piece, so only move the ones with a tag of "Puzzle Piece"
                    draggingPiece = hit.transform;
                    offset = draggingPiece.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    offset += Vector3.back;
                }
            }

            if(draggingPiece && Input.GetMouseButtonUp(0))
            {
                draggingPiece.position += Vector3.forward;
                SnapAndDisableIfCorrect();
                draggingPiece = null;
            }

            if(draggingPiece)//ie draggingPiece is not null
            {
                Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //newPosition.z = draggingPiece.position.z;
                //above line to make sure z position is correct so piece doesn't vanish from view
                newPosition += offset;
                //above line includes z position of offset, so newPosition.z line not needed
                draggingPiece.position = newPosition;
            }


        }

        
        
    }
    void SnapAndDisableIfCorrect()
    {
        //index ofpiece to determine correct position
        int pieceIndex = pieces.IndexOf(draggingPiece);
        //coordinates of piece in puzzle
        int col = pieceIndex % dimensions.x;
        int row = pieceIndex / dimensions.x;
        //target position in non scaled coordinates
        Vector2 targetPos = new((-width*dimensions.x/2)+(width*col)+(width/2), (-height*dimensions.y/2)+(height*row)+(height/2));
        //check if in correct position
        if(Vector2.Distance(draggingPiece.localPosition, targetPos) < (width/2))
        {
            //PUT IN A CLICKING SOUND HERE FOR WHEN PIECE IS PUT IN CORRECTLY
            //snap to destination
            draggingPiece.localPosition = targetPos;
            //disable collider so can't click on object anymore
            draggingPiece.GetComponent<BoxCollider2D>().enabled = false;
            draggingPiece.GetComponent<AudioSource>().Play();
            piecesCorrect++;//to make sure it is added so one step closer to finishing puzzle
            if(piecesCorrect == pieces.Count)
            {
                //if all pieces in place, then can let timer know to stop and disable the pieces and such
                //that way puzzle is finished, item is gained, and timer stops so scene doesn't start over
                finishedPuzzle = true;
                player.RegisterItem(questObject);
            }
        }
    }

    public void FinishGame()
    {
        //call if finishedPuzzle is true
        foreach(Transform piece in pieces)
        {
            Destroy(piece.gameObject);
        }
        pieces.Clear();//clear the List
        //could disable the puzzle base, but would be best to do it in the character script
    }

}
