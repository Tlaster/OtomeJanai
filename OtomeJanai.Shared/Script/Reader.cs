using System;
using System.Collections.Generic;
using System.Text;

namespace OtomeJanai.Shared.Script
{
    internal static class Reader
    {
        private static IList<IToken> GetBlock(string fileString)
        {
            var block = new List<IToken>();
            var lexer = new Lexer();
            var parser = new Parser();
            var tokenList = lexer.ReadLine(fileString, 0);
            foreach (var item in tokenList)
            {
                do
                {
                    parser.Push(item);
                } while (parser.IsLoopingForReduce);
                if (!parser.IsAccepted) continue;
                block.Add(parser.Block);
                parser.Reset();
                if (block[block.Count - 1].Type == TokenType.Setting)
                /// ACC will not push current item,
                /// it will cause error
                {
                    parser.Push(item);
                }
            }
            return block;
        }
    }
}
