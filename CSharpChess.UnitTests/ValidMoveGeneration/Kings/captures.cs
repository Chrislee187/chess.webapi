﻿using System.Linq;
using CSharpChess.Extensions;
using CSharpChess.Movement;
using CSharpChess.UnitTests.Helpers;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CSharpChess.UnitTests.ValidMoveGeneration.Kings
{
    [TestFixture]
    public class captures : BoardAssertions
    {
        [Test]
        public void can_take()
        {
            const string asOneChar = ".......k" +
                                     "........" +
                                     "..r....." +
                                     "...Kb..." +
                                     "..ppp..." +
                                     "........" +
                                     "........" +
                                     "........";

            var board = BoardBuilder.CustomBoard(asOneChar, Colours.White);
            var expectedTakes = BoardLocation.List("D4", "C4", "E4", "C6", "E5");

            var generator = new KingMoveGenerator();
            var chessMoves = generator.All(board, BoardLocation.At("D5")).Takes().ToList();

            AssertMovesContainsExpectedWithType(chessMoves, expectedTakes, MoveType.Take);
        }
    }
}