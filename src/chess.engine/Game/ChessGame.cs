﻿using System.Linq;
using board.engine;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Entities;
using chess.engine.SAN;
using Microsoft.Extensions.Logging;

namespace chess.engine.Game
{
    public class ChessGame
    {
        public static bool OutOfBounds(int value) => value < 1 || value > 8;

        private readonly BoardEngine<ChessPieceEntity> _engine;

        public Colours CurrentPlayer { get; private set; }
        private Colours NextPlayer() => CurrentPlayer == Colours.White ? Colours.Black : Colours.White;

        public PlayerState PlayerState { get; private set; }
        public bool InProgress => PlayerState == PlayerState.InProgress;

        public LocatedItem<ChessPieceEntity>[,] Board => _engine.Board;

        public IBoardState<ChessPieceEntity> BoardState => _engine.BoardState;

        private readonly ILogger<ChessGame> _logger;
        private readonly IBoardEntityFactory<ChessPieceEntity> _entityFactory;
        private readonly IPlayerStateService _gameStateService;
        private SanMoveFinder _sanMoveFinder;

        public ChessGame(
            ILogger<ChessGame> logger,
            IBoardEngineProvider<ChessPieceEntity> boardEngineProvider,
            IBoardEntityFactory<ChessPieceEntity> entityFactory,
            IPlayerStateService gameStateService
        )
            : this(logger, boardEngineProvider, entityFactory, gameStateService, new ChessBoardSetup(entityFactory))
        {
        }

        public ChessGame(
            ILogger<ChessGame> logger,
            IBoardEngineProvider<ChessPieceEntity> boardEngineProvider,
            IBoardEntityFactory<ChessPieceEntity> entityFactory,
            IPlayerStateService gameStateService,
            IBoardSetup<ChessPieceEntity> setup,
            Colours whoseTurn = Colours.White)
        {

            _logger = logger;
            _logger?.LogInformation("Initialising new chess game");

            _engine = boardEngineProvider.Provide(setup);
            _entityFactory = entityFactory;
            _gameStateService = gameStateService;
            CurrentPlayer = whoseTurn;
            PlayerState = _gameStateService.CurrentPlayerState(BoardState, CurrentPlayer);
        }

        public string Move(string input)
        {
            _logger?.LogDebug($"Attempting move {input}");

            if (!StandardAlgebraicNotation.TryParse(input, out var san))
            {
                // TODO: More detailed error
                return $"Error: invalid move {input}, are you using upper-case for Files?";
            }


            _sanMoveFinder = new SanMoveFinder(_engine.BoardState);

            var move = _sanMoveFinder.Find(san, CurrentPlayer);

            if (move == null)
            {
                return $"Error: No matching move found: {input}";
            }

            return PlayValidMove(move);

        }

        private string PlayValidMove(BoardMove move)
        {
            _engine.Move(move);


            CurrentPlayer = NextPlayer();
            PlayerState = _gameStateService.CurrentPlayerState(BoardState, CurrentPlayer);

            return PlayerState == PlayerState.Check || PlayerState == PlayerState.Checkmate
                ? PlayerState.ToString()
                : "";

        }
 
        #region Meta Info

        public static int DirectionModifierFor(Colours player) => player == Colours.White ? +1 : -1;
        public static int EndRankFor(Colours colour) => colour == Colours.White ? 8 : 1;

        #endregion
    }

    public enum PlayerState
    {
        InProgress,
        Check,
        Checkmate
    }
}