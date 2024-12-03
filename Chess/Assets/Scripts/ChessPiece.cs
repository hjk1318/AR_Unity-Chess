using UnityEngine;
using System.Collections.Generic;

public enum PieceType { Pawn, Rook, Knight, Bishop, Queen, King }

public class ChessPiece : MonoBehaviour
{
    public bool IsWhite { get; private set; }
    public PieceType Type { get; private set; }
    public int CurrentX { get; private set; }
    public int CurrentY { get; private set; }
    private ChessGame game;

    public void Setup(bool isWhite, ChessGame game, int x, int y, PieceType type)
    {
        IsWhite = isWhite;
        Type = type;
        this.game = game;
        MoveTo(x, y);
    }

    public void MoveTo(int x, int y)
    {
        CurrentX = x;
        CurrentY = y;
        transform.position = new Vector3(x, 0.5f, y);
    }

    public bool IsValidMove(Vector2Int targetPosition)
    {
        List<Vector2Int> validMoves = GetValidMoves();
        return validMoves.Contains(targetPosition);
    }

    public List<Vector2Int> GetValidMoves()
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        switch (Type)
        {
            case PieceType.Pawn:
                moves.AddRange(GetPawnMoves());
                break;
            case PieceType.Rook:
                moves.AddRange(GetStraightMoves());
                break;
            case PieceType.Knight:
                moves.AddRange(GetKnightMoves());
                break;
            case PieceType.Bishop:
                moves.AddRange(GetDiagonalMoves());
                break;
            case PieceType.Queen:
                moves.AddRange(GetStraightMoves());
                moves.AddRange(GetDiagonalMoves());
                break;
            case PieceType.King:
                moves.AddRange(GetKingMoves());
                break;
        }

        // 필터링: 보드 범위 내 이동만 허용
        moves.RemoveAll(pos => pos.x < 0 || pos.x >= 8 || pos.y < 0 || pos.y >= 8);

        // 필터링: 같은 색의 말이 있는 위치는 이동 불가
        moves.RemoveAll(pos => game.GetPieceAt(pos.x, pos.y)?.IsWhite == IsWhite);

        return moves;
    }

    private List<Vector2Int> GetPawnMoves()
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        int direction = IsWhite ? 1 : -1;

        // 직진 이동
        if (game.GetPieceAt(CurrentX, CurrentY + direction) == null)
        {
            moves.Add(new Vector2Int(CurrentX, CurrentY + direction));

            // 첫 이동 시 두 칸 이동 가능
            if ((IsWhite && CurrentY == 1) || (!IsWhite && CurrentY == 6))
            {
                if (game.GetPieceAt(CurrentX, CurrentY + 2 * direction) == null)
                    moves.Add(new Vector2Int(CurrentX, CurrentY + 2 * direction));
            }
        }

        // 대각선 공격
        if (game.GetPieceAt(CurrentX - 1, CurrentY + direction)?.IsWhite != IsWhite)
            moves.Add(new Vector2Int(CurrentX - 1, CurrentY + direction));
        if (game.GetPieceAt(CurrentX + 1, CurrentY + direction)?.IsWhite != IsWhite)
            moves.Add(new Vector2Int(CurrentX + 1, CurrentY + direction));

        return moves;
    }

    private List<Vector2Int> GetStraightMoves()
    {
        return GetMovesInDirections(new Vector2Int[] {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
        });
    }

    private List<Vector2Int> GetDiagonalMoves()
    {
        return GetMovesInDirections(new Vector2Int[] {
            Vector2Int.up + Vector2Int.left, Vector2Int.up + Vector2Int.right,
            Vector2Int.down + Vector2Int.left, Vector2Int.down + Vector2Int.right
        });
    }

    private List<Vector2Int> GetKingMoves()
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx != 0 || dy != 0)
                    moves.Add(new Vector2Int(CurrentX + dx, CurrentY + dy));
            }
        }
        return moves;
    }

    private List<Vector2Int> GetKnightMoves()
    {
        List<Vector2Int> moves = new List<Vector2Int>
        {
            new Vector2Int(CurrentX + 1, CurrentY + 2),
            new Vector2Int(CurrentX - 1, CurrentY + 2),
            new Vector2Int(CurrentX + 2, CurrentY + 1),
            new Vector2Int(CurrentX - 2, CurrentY + 1),
            new Vector2Int(CurrentX + 1, CurrentY - 2),
            new Vector2Int(CurrentX - 1, CurrentY - 2),
            new Vector2Int(CurrentX + 2, CurrentY - 1),
            new Vector2Int(CurrentX - 2, CurrentY - 1)
        };
        return moves;
    }

    private List<Vector2Int> GetMovesInDirections(Vector2Int[] directions)
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        foreach (var dir in directions)
        {
            int x = CurrentX + dir.x;
            int y = CurrentY + dir.y;

            while (game.GetPieceAt(x, y) == null)
            {
                moves.Add(new Vector2Int(x, y));
                x += dir.x;
                y += dir.y;

                if (x < 0 || x >= 8 || y < 0 || y >= 8)
                    break;
            }

            // 적이 있는 경우 이동 가능
            if (game.GetPieceAt(x, y)?.IsWhite != IsWhite)
                moves.Add(new Vector2Int(x, y));
        }

        return moves;
    }
}
