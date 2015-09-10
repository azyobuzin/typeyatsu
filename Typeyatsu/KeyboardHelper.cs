using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Typeyatsu
{
    static class KeyboardHelper
    {
        public static bool TryKeyToChar(Key key, out char result)
        {
            switch (key)
            {
                case Key.OemMinus:
                case Key.Subtract:
                    result = '-';
                    return true;
                default:
                    if (char.TryParse(key.ToString(), out result))
                    {
                        result = char.ToLowerInvariant(result);
                        return true;
                    }
                    return false;
            }
        }

        public static bool IsSpaceOrEnter(Key key)
        {
            switch (key)
            {
                case Key.Space:
                case Key.Enter:
                    return true;
            }

            return false;
        }
    }
}
