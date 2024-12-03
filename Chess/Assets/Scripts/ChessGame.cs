using UnityEngine;

public class ChessGame : MonoBehaviour
{
    public GameObject tilePrefab; // ť�� ����� Ÿ�� ������
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
        // 8x8 ���� ����
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                // Ÿ�� ����
                GameObject tileObject = Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.identity);
                tileObject.name = $"Tile ({x}, {y})";

                // Tile ��ũ��Ʈ �ʱ�ȭ
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

        // �� ��ġ
        for (int x = 0; x < 8; x++)
        {
            CreatePiece(piecePrefabs[0], x, yPawn, isWhite, PieceType.Pawn);
        }

        // �� ���� ��ġ
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
            // �̵� ��ġ�� ��� �� ������ �ı� �ϱ����� �ִϸ��̼� ��� �־���� (�̵� ���� ���� �ִϸ��̼ǰ�, �ı����� ���� �ִϸ��̼�)
            // �ִϸ��̼� �ð� Ȯ�� �� ���������� ���� �ִϸ��̼� ����� �Ʒ� ���� ��ġ ü���� �ı�(Destroy) �ڵ带 
            //  �ڷ�ƾ �Լ��� ���� ���� ó�� �ǰ� ���� �ʿ�)

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
