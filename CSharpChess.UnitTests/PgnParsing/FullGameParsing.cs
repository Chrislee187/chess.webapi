﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CsChess.Pgn;
using CSharpChess.System;
using NUnit.Framework;

namespace CSharpChess.UnitTests.PgnParsing
{
    [TestFixture]
    public class FullGameParsing
    {
        #region Test Games
        // https://en.wikipedia.org/wiki/Portable_Game_Notation
        private const string WikiGame = @"
[Event ""F/S Return Match""]
[Site ""Belgrade, Serbia JUG""]
[Date ""1992.11.04""]
[Round ""29""]
[White ""Fischer, Robert J.""]
[Black ""Spassky, Boris V.""]
[Result ""1/2-1/2""]
        
1. e4 e5 2. Nf3 Nc6 3. Bb5 a6 
4. Ba4 Nf6 5. O-O Be7 6. Re1 b5 7. Bb3 d6 8. c3 O-O 9. h3 Nb8 10. d4 Nbd7
11. c4 c6 12. cxb5 axb5 13. Nc3 Bb7 14. Bg5 b4 15. Nb1 h6 16. Bh4 c5 17. dxe5
Nxe4 18. Bxe7 Qxe7 19. exd6 Qf6 20. Nbd2 Nxd6 21. Nc4 Nxc4 22. Bxc4 Nb6
23. Ne5 Rae8 24. Bxf7+ Rxf7 25. Nxf7 Rxe1+ 26. Qxe1 Kxf7 27. Qe3 Qg5 28. Qxg5
hxg5 29. b3 Ke6 30. a3 Kd6 31. axb4 cxb4 32. Ra5 Nd5 33. f3 Bc8 34. Kf2 Bf5
35. Ra7 g6 36. Ra6+ Kc5 37. Ke1 Nf4 38. g3 Nxh3 39. Kd2 Kb5 40. Rd6 Kc5 41. Ra6
Nf2 42. g4 Bd3 43. Re6 1/2-1/2
";

        private const string TestGame = @"[Event ""Candidats sf1""]
[Site ""Linares""]
[Date ""1992.??.??""]
[Round ""3""]
[White ""Karpov, Anatoly""]
[Black ""Short, Nigel D""]
[Result ""1/2-1/2""]
[WhiteElo ""2725""]
[BlackElo ""2685""]
[ECO ""D20""]

1.d4 d5 2.c4 dxc4 3.e4 Nf6 4.e5 Nd5 5.Bxc4 Nb6 6.Bd3 Nc6 7.Be3 Nb4 8.Be4 f5
9.exf6 exf6 10.Nc3 f5 11.Bf3 N4d5 12.Bd2 Be6 13.Nge2 Qd7 14.O-O O-O-O 15.Re1 Rg8
16.Bg5 Re8 17.Nf4 Nxf4 18.Bxf4 g5 19.Be5 Bg7 20.Rc1 Bxe5 21.dxe5 Qxd1 22.Bxd1 a6
23.g3 Re7 24.b3 Rd7 25.f3 Nd5 26.Na4 b6 27.Be2 Kb7 28.Bc4 c5 29.Kf2 Rgd8
30.Re2 Nc7 31.Rcc2 Kc6 32.Nb2 b5 33.Bxe6 Nxe6 34.Rc1 Rd4 35.Ke1 R4d5 36.Kf2 Nd4
37.Re3 f4 38.gxf4 gxf4 39.Re4 Ne6 40.Rc2 Rd2+ 41.Re2 Rxc2 42.Rxc2 Rd4 43.Re2 Kd5
44.Kg2 h5 45.Kf1 h4 46.Kg2 Ng5 47.Kf2 h3 48.Rc2 Ne6 49.Ke2 Kxe5 50.Nd3+ Kd6
51.Nf2 Rd5 52.Rc3 Kc6 53.Nxh3 Rh5 54.Nf2 Rxh2 55.Kf1 Kd5 56.Rd3+ Nd4 57.Kg1 Rh6
58.Ne4 c4 59.bxc4+ bxc4 60.Rd1 Rc6 61.Nc3+ Ke5 62.Kf1 Rh6 63.Re1+ Kf5 64.Re8 Nxf3
65.Ne2 Nh2+ 66.Kg1 f3 67.Rf8+ Ke5 68.Ng3 Rh7 69.Kf2 c3 70.Rc8 Kd4 71.Rd8+ Kc4
72.Nf5 Rc7 73.Ne3+ Kb5 74.Rd1 Ka4 75.Rc1 Rd7 76.Rxc3 Rd2+ 77.Kg3 f2 78.Rc4+ Ka3
79.Rf4 f1=N+ 80.Nxf1 Nxf1+ 81.Rxf1 a5 82.Rf5 a4 83.Rf4 Rxa2 84.Rf3+ Kb4 85.Rf4+ Kc3
86.Rf3+ Kd4 87.Rf4+ Ke5 88.Rb4 a3 89.Rb3 Ke4 90.Kh3 Kd4 91.Rg3 Ra1 92.Kh2 Ke4
93.Rb3 a2 94.Ra3  1/2-1/2";

#endregion

        [Test]
        public void can_parse_wiki_sample_game()
        {
            var pgnGameText = WikiGame;

            var pgnGame = PgnGame.Parse(pgnGameText).First();

            Assert.That(pgnGame.Event, Is.EqualTo("F/S Return Match"));
            Assert.That(pgnGame.Site, Is.EqualTo("Belgrade, Serbia JUG"));
            Assert.That(pgnGame.Date.ToString(), Is.EqualTo("1992.11.04"));
            Assert.That(pgnGame.Round, Is.EqualTo(29));
            Assert.That(pgnGame.White, Is.EqualTo("Fischer, Robert J."));
            Assert.That(pgnGame.Black, Is.EqualTo("Spassky, Boris V."));
            Assert.That(pgnGame.Result, Is.EqualTo(ChessGameResult.Draw));

            Assert.That(pgnGame.TurnQueries.Count(), Is.EqualTo(43));

            Assert.That(pgnGame.TurnQueries.Count(), Is.EqualTo(43));
        }

        [Test]
        public void can_parse_lots_of_games()
        {
            
            var pgnText = File.ReadAllText(@"C:\Src\Chess\CSharpChess.UnitTests\bin\Debug\short.pgn");

            var pgnGames = PgnGame.Parse(pgnText).ToList();

            Assert.That(pgnGames.Count(), Is.GreaterThan(0));
            Console.WriteLine($"{pgnGames.Count()} parsed.");

        }


        [Test, Explicit]
        public void can_play_lots_of_games()
        {
            var stream = File.OpenRead(@"C:\Src\Chess\CSharpChess.UnitTests\bin\Debug\short.pgn");

            int count = 0;

            using (var reader = new PgnReader(stream))
            {
                var game = reader.ReadGame();

                while (game != null)
                {

//                    Console.WriteLine($"Game index: {++count}");
//                    if (count < 1037) continue;
                    PlayGame(game);

                    game = reader.ReadGame();

                    Console.Out.FlushAsync();
                }
            }
            Console.WriteLine(count);
            DumpMetrics();
        }

        [Test, Explicit]
        public void can_play_test_game()
        {
            PlayGame(TestGame);
            DumpMetrics();
        }

        [Test, Explicit]
        public void perf_test()
        {
            for (int i = 0; i < 10; i++)
            {
                PlayGame(TestGame);
            }
            DumpMetrics();
        }

        private void DumpMetrics()
        {
            if (_gameTimes.Any())
            {
                var minMs = _gameTimes.Values.Min();
                var maxMs = _gameTimes.Values.Max();

                var fastest = _gameTimes.First(kvp => kvp.Value == minMs);
                var slowest = _gameTimes.First(kvp => kvp.Value == maxMs);
                var averageMs = _gameTimes.Average(kvp => kvp.Value);

                Console.WriteLine($"Slowest: {maxMs:######} : {slowest}");
                Console.WriteLine($"Fastest: {minMs:######} : {fastest}");
                Console.WriteLine($"Total  :  {TimeSpan.FromMilliseconds(_gameTimes.Sum(kvp => kvp.Value))}");
                Console.WriteLine($"Average: {averageMs:######}");
                _gameTimes.Clear();
            }
        }


        [Test]
        public void can_play_pgn_with_piece_blocking_check()
        {
            var stream = File.OpenRead(@"C:\Src\Chess\CSharpChess.UnitTests\bin\Debug\has-piece-blocking-check.pgn");
            
            using (var reader = new PgnReader(stream))
            {
                var game = reader.ReadGame();

                while (game != null)
                {
                    PlayGame(game);

                    game = reader.ReadGame();
                }
            }
        }

        private IDictionary<string, long> _gameTimes = new ConcurrentDictionary<string, long>();
        private void PlayGame(string game)
        {

            var sw = new Stopwatch();
            sw.Start();

            var pgnGame = PgnGame.Parse(game).Single();
            Console.WriteLine($"Processing game: {pgnGame}");

            var board = new PgnGameResolver().Resolve(pgnGame);

            var result = ChessGameResultToPgnResult(board.GameState);

            if (result != ChessGameResult.Unknown) // Resignation & Draws can't be determined from the board state alone
            {
                Assert.That(pgnGame.Result, Is.EqualTo(result), pgnGame + "\n" + game);
            }

            var key = pgnGame.ToString() + Guid.NewGuid();
            _gameTimes.Add(key, sw.ElapsedMilliseconds);
            Console.WriteLine($"  Game took {_gameTimes[key]}ms");
        }

        [Test]
        public void can_play_pgn_with_pawn_promotion()
        {
            var stream = File.OpenRead(@"C:\Src\Chess\CSharpChess.UnitTests\bin\Debug\has-black-pawn-to-queen-promotion.pgn");


            using (var reader = new PgnReader(stream))
            {
                var game = reader.ReadGame();

                while (game != null)
                {
                    var pgnGame = PgnGame.Parse(game).Single();
                    var board = new PgnGameResolver().Resolve(pgnGame);

                    var result = ChessGameResultToPgnResult(board.GameState);

                    if (IsInCheckmate(board.GameState))
                    {
                        Assert.That(pgnGame.Result, Is.EqualTo(result), pgnGame.ToString());
                    }

                    game = reader.ReadGame();
                }
            }
        }
        private bool IsInCheckmate(GameState boardGameState)
        {
            return new[]
                {
                    GameState.CheckMateBlackWins,
                    GameState.CheckMateWhiteWins,
                }
                .Any(a => a == boardGameState);
        }

        private static ChessGameResult ChessGameResultToPgnResult(GameState pgnGame)
        {
            switch (pgnGame)
            {
                case GameState.CheckMateBlackWins:
                    return ChessGameResult.BlackWins;
                case GameState.CheckMateWhiteWins:
                    return ChessGameResult.WhiteWins;
                case GameState.Stalemate:
                case GameState.Draw:
                    return ChessGameResult.Draw;
                case GameState.WhiteKingInCheck:
                case GameState.BlackKingInCheck:
                case GameState.WaitingForMove:
                    return ChessGameResult.Unknown;
                default:
                    throw new ArgumentOutOfRangeException(pgnGame.ToString());
            }
        }
    }
}
