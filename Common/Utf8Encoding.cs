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
                for (; CharsIndex < CharsLenght; CharsIndex++)
                {
                    if (BytesIndex == BytesLength)
                        break;

                    var Ch = default(int);
                    if (BeginningOfFile & (CharsIndex == (CharsInitialIndex - 1)))
                        Ch = 0xFEFF;
                    else
                        Ch = CharsArray[CharsIndex];

                    if (Ch < 128)
                    {
                        BytesArray[BytesIndex] = (byte)Ch;
                        BytesIndex += 1;
                        continue;
                    }

                    var I = 0;

                    do
                    {
                        T[I] = (byte)((2 << 6) | (Ch & ((1 << 6) - 1)));
                        Ch >>= 6;
                        I += 1;
                    } while (Ch >= (1 << (6 - I)));

                    // We are having 6 - I bits remaining.
                    // So we have to make 7 - I zeros at the end of the byte.
                    T[I] = (byte)((255 ^ ((1 << (7 - I)) - 1)) | Ch);
                    I += 1;

                    if ((BytesIndex + I) >= BytesLength)
                        break;

                    for (I -= 1; I >= 0; I--)
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
                for (; BytesIndex < BytesLength; BytesIndex++)
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

                    var Ch = (B << (I + 1)) >> (I + 1);
                    for (var J = 1; J < I; J++)
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
