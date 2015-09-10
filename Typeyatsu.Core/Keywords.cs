using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Typeyatsu.Core
{
    public static class Keywords
    {
        public static Keyword[] Hatena { get; private set; }

        private static bool FilterKeyword(Keyword k)
        {
            if (string.IsNullOrEmpty(k.Furigana)) return false;
            if (k.Furigana.Length > 16) return false;
            if (k.Word.All(x => "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZａｂｃｄｅｆｇｈｉｊｋｌｍｎｏｐｑｒｓｔｕｖｗｘｙｚＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＺ0123456789０１２３４５６７８９ 　!?！？".Contains(x)))
                return false;

            for (var i = 0; i < k.Furigana.Length; i++)
            {
                switch (k.Furigana[i])
                {
                    case 'ゐ':
                    case 'ゑ':
                        return false;
                    case '゛':
                        if (i == 0 || k.Furigana[i - 1] != 'う')
                            return false;
                        break;
                }
            }

            return true;
        }

        public static void Load()
        {
            var list = new List<Keyword>(400000);
            using (var sr = new StringReader(Properties.Resources.keywordlist_furigana))
            {
                while (true)
                {
                    var line = sr.ReadLine();
                    if (line == null) break;
                    var s = line.Split(new[] { '\t' }, StringSplitOptions.None);
                    if (s.Length == 2)
                    {
                        var k = new Keyword(s[1], s[0]);
                        if (FilterKeyword(k))
                            list.Add(k);
                    }
                }
            }
            Hatena = list.ToArray();
        }
    }
}
