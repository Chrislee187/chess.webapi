using System.Threading.Tasks;

namespace chess.webapi.client.csharp
{
    public interface IChessGameApiClient
    {
        Task<ChessWebApiResult> ChessGameAsync();
        Task<ChessWebApiResult> PlayMoveAsync(string board, string move);
        Task<ChessWebApiResult> ChessGameAsync(string customSerialisedBoard);
    }
}