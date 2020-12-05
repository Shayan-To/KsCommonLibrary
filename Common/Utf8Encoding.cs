using Microsoft.VisualBasic;

namespace Ks
{
    namespace Common
    {
        public class Utf8Encoding
        {
            public static bool Encode(char[] CharsArray, int CharsIndex, int CharsLenght, byte[] BytesArray, int BytesIndex, int BytesLength, bool BeginningOfFile, out int CharsRead, out int BytesWritten)
            {
                CharsLenght += CharsIndex;
                BytesLength += BytesIndex;

                var CharsInitialIndex = CharsIndex;
                var BytesInitialIndex = BytesIndex;
                var T = new byte[6];
                if (BeginningOfFile)
                    CharsIndex -= 1;
                var loopTo = CharsLenght - 1;
                for (CharsIndex = CharsIndex; CharsIndex <= loopTo; CharsIndex++)
                {
                    if (BytesIndex == BytesLength)
                        break;

                    int Ch = default(int);
                    if (BeginningOfFile & (CharsIndex == (CharsInitialIndex - 1)))
                        Ch = 0xFEFF;
                    else
                        Ch = Strings.AscW(CharsArray[CharsIndex]);

                    if (Ch < 128)
                    {
                        BytesArray[BytesIndex] = System.Convert.ToByte(Ch);
                        BytesIndex += 1;
                        continue;
                    }

                    var I = 0;

                    do
                    {
                        T[I] = System.Convert.ToByte(((2 << 6) | (Ch & ((1 << 6) - 1))));
                        Ch >>= 6;
                        I += 1;
                    }
                    while (Ch >= (1 << (6 - I)));

                    // We are having 6 - I bits remaining.
                    // So we have to make 7 - I zeros at the end of the byte.
                    T[I] = System.Convert.ToByte(((255 ^ ((1 << (7 - I)) - 1)) | Ch));
                    I += 1;

                    if ((BytesIndex + I) >= BytesLength)
                        break;

                    for (I = I - 1; I >= 0; I += -1)
                    {
                        BytesArray[BytesIndex] = T[I];
                        BytesIndex += 1;
                    }
                }
                // 12345678
                // 76543210

                CharsRead = CharsIndex - CharsInitialIndex;
                BytesWritten = BytesIndex - BytesInitialIndex;
                return true;
            }

            public static bool Decode(byte[] BytesArray, int BytesIndex, int BytesLength, char[] CharsArray, int CharsIndex, int CharsLength, bool BeginningOfFile, out int BytesRead, out int CharsWritten)
            {
                CharsLength += CharsIndex;
                BytesLength += BytesIndex;

                var CharsInitialIndex = CharsIndex;
                var BytesInitialIndex = BytesIndex;

                try
                {
                var loopTo = BytesLength - 1;
                for (BytesIndex = BytesIndex; BytesIndex <= loopTo; BytesIndex++)
                {
                    if (CharsIndex == CharsLength)
                        break;

                    var B = BytesArray[BytesIndex];

                    if ((B >> 7) == 0)
                    {
                        CharsArray[CharsIndex] = (char)B;
                        CharsIndex += 1;
                        continue;
                    }

                    if (((B >> 6) & 1) == 0)
                        return false;

                    var I = 5;
                    while (((B >> I) & 1) != 0)
                    {
                        I -= 1;
                        if (I == 0)
                            return false;
                    }

                    I = 7 - I;

                    if ((BytesIndex + I) >= BytesLength)
                        break;

                    int Ch = (B << (I + 1)) >> (I + 1);
                    var loopTo1 = I - 1;
                    for (int J = 1; J <= loopTo1; J++)
                    {
                        BytesIndex += 1;
                        B = BytesArray[BytesIndex];
                        if ((B >> 6) != 2)
                            return false;
                        Ch = (Ch << 6) | ((B << 2) >> 2);
                    }

                    // Exclude Byte Order Mark (BOM).
                    if (!(BeginningOfFile & (CharsIndex == CharsInitialIndex) & (Ch == 0xFEFF)))
                    {
                        CharsArray[CharsIndex] = (char)Ch;
                        CharsIndex += 1;
                    }
                }
                // 12345678
                // 76543210
                }
                finally
                {
                BytesRead = BytesIndex - BytesInitialIndex;
                CharsWritten = CharsIndex - CharsInitialIndex;
                }

                return true;
            }
        }
    }
}
