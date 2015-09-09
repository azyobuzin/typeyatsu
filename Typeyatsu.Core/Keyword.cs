﻿namespace Typeyatsu.Core
{
    public struct Keyword
    {
        public Keyword(string word, string furigana)
        {
            this.Word = word;
            this.Furigana = furigana;
        }

        public string Word { get; }
        public string Furigana { get; }
    }
}
