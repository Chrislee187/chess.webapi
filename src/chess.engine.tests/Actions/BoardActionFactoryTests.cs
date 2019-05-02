﻿using System;
using chess.engine.Actions;
using chess.engine.Movement;
using NUnit.Framework;

namespace chess.engine.tests.Actions
{
    [TestFixture]
    public class BoardActionFactoryTests
    {
        private BoardActionFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new BoardActionFactory();
        }
        [Test]
        public void FactorySupportsAllMoveTypes()
        {
            foreach (ChessMoveType type in Enum.GetValues(typeof(ChessMoveType)))
            {
                Assert.DoesNotThrow(() => _factory.Create(type, null), $"{type} is not support");
            }
        }
    }
}