using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int position;
    public ChessGame game;
    private Renderer tileRenderer;

    public void Initialize(Vector2Int pos, ChessGame chessGame, bool isWhite)
    {
        position = pos;
        game = chessGame;
        tileRenderer = GetComponent<Renderer>();

        // Ÿ�� ���� ����
        tileRenderer.material.color = isWhite ? Color.white : Color.gray;
    }

    private void OnMouseDown()
    {
        if (game != null)
        {
            game.SelectPiece(position);
        }
    }
}
