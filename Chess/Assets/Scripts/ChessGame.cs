using UnityEngine;

public class ChessGame : MonoBehaviour
{
    public GameObject tilePrefab; // 큐브 기반의 타일 프리팹
    private ChessPiece[,] board = new ChessPiece[8, 8];
    private Tile[,] tiles = new Tile[8, 8];

    public ChessPiece[] whitePiecePrefabs;
    public ChessPiece[] blackPiecePrefabs;

    private bool isWhiteTurn = true;
    private ChessPiece selectedPiece;

    void Start()
    {
        GenerateBoard();
        SetupBoard();
    }

    private void GenerateBoard()
    {
        // 8x8 보드 생성
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                // 타일 생성
                GameObject tileObject = Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.identity);
                tileObject.name = $"Tile ({x}, {y})";

                // Tile 스크립트 초기화
                Tile tile = tileObject.GetComponent<Tile>();
                tiles[x, y] = tile;
                tile.Initialize(new Vector2Int(x, y), this, (x + y) % 2 == 0);
            }
        }
    }

    private void SetupBoard()
    {
        PlacePieces(whitePiecePrefabs, true);
        PlacePieces(blackPiecePrefabs, false);
    }

    private void PlacePieces(ChessPiece[] piecePrefabs, bool isWhite)
    {
        int yPawn = isWhite ? 1 : 6;
        int yBackRow = isWhite ? 0 : 7;

        // 폰 배치
        for (int x = 0; x < 8; x++)
        {
            CreatePiece(piecePrefabs[0], x, yPawn, isWhite, PieceType.Pawn);
        }

        // 백 라인 배치
        CreatePiece(piecePrefabs[1], 0, yBackRow, isWhite, PieceType.Rook);
        CreatePiece(piecePrefabs[1], 7, yBackRow, isWhite, PieceType.Rook);
        CreatePiece(piecePrefabs[2], 1, yBackRow, isWhite, PieceType.Knight);
        CreatePiece(piecePrefabs[2], 6, yBackRow, isWhite, PieceType.Knight);
        CreatePiece(piecePrefabs[3], 2, yBackRow, isWhite, PieceType.Bishop);
        CreatePiece(piecePrefabs[3], 5, yBackRow, isWhite, PieceType.Bishop);
        CreatePiece(piecePrefabs[4], 3, yBackRow, isWhite, PieceType.Queen);
        CreatePiece(piecePrefabs[5], 4, yBackRow, isWhite, PieceType.King);
    }

    private void CreatePiece(ChessPiece piecePrefab, int x, int y, bool isWhite, PieceType type)
    {
        ChessPiece piece = Instantiate(piecePrefab, new Vector3(x, 0.5f, y), Quaternion.identity);
        piece.Setup(isWhite, this, x, y, type);
        board[x, y] = piece;
    }

    public void SelectPiece(Vector2Int position)
    {
        ChessPiece piece = GetPieceAt(position.x, position.y);

        if (selectedPiece == null)
        {
            if (piece != null && piece.IsWhite == isWhiteTurn)
            {
                selectedPiece = piece;
                Debug.Log($"Selected {selectedPiece.Type} at {position}");
            }
            else
            {
                Debug.Log("Invalid selection");
            }
        }
        else
        {
            if (selectedPiece.IsValidMove(position))
            {
                MovePiece(selectedPiece, position.x, position.y);
                isWhiteTurn = !isWhiteTurn;
            }
            selectedPiece = null;
        }
    }

    private void MovePiece(ChessPiece piece, int x, int y)
    {
        board[piece.CurrentX, piece.CurrentY] = null;

        if (board[x, y] != null)
        {
            // 이동 위치에 상대 말 있을시 파괴 하기전에 애니메이션 재생 있어야함 (이동 말의 공격 애니메이션과, 파괴말의 다이 애니메이션)
            // 애니메이션 시간 확보 및 순차진행을 위해 애니메이션 재생과 아래 기존 위치 체스말 파괴(Destroy) 코드를 
            //  코루틴 함수에 묶어 만들어서 처리 되게 수정 필요)

            Destroy(board[x, y].gameObject);            
        }

        piece.MoveTo(x, y);
        board[x, y] = piece;
    }

    public ChessPiece GetPieceAt(int x, int y)
    {
        if (x < 0 || x >= 8 || y < 0 || y >= 8) return null;
        return board[x, y];
    }
}
