using System;

public class CustomMD5
{
    private const int ChunkSize = 64;

    // s specifies the per-round shift amounts
    private readonly UInt32[] S =
    {
        7, 12, 17, 22,  7, 12, 17, 22,  7, 12, 17, 22,  7, 12, 17, 22,
        5,  9, 14, 20,  5,  9, 14, 20,  5,  9, 14, 20,  5,  9, 14, 20,
        4, 11, 16, 23,  4, 11, 16, 23,  4, 11, 16, 23,  4, 11, 16, 23,
        6, 10, 15, 21,  6, 10, 15, 21,  6, 10, 15, 21,  6, 10, 15, 21,
    };

    // constants
    private readonly UInt32[] K =
    {
        0xd76aa478, 0xe8c7b756, 0x242070db, 0xc1bdceee,
        0xf57c0faf, 0x4787c62a, 0xa8304613, 0xfd469501,
        0x698098d8, 0x8b44f7af, 0xffff5bb1, 0x895cd7be,
        0x6b901122, 0xfd987193, 0xa679438e, 0x49b40821,
        0xf61e2562, 0xc040b340, 0x265e5a51, 0xe9b6c7aa,
        0xd62f105d, 0x02441453, 0xd8a1e681, 0xe7d3fbc8,
        0x21e1cde6, 0xc33707d6, 0xf4d50d87, 0x455a14ed,
        0xa9e3e905, 0xfcefa3f8, 0x676f02d9, 0x8d2a4c8a,
        0xfffa3942, 0x8771f681, 0x6d9d6122, 0xfde5380c,
        0xa4beea44, 0x4bdecfa9, 0xf6bb4b60, 0xbebfbc70,
        0x289b7ec6, 0xeaa127fa, 0xd4ef3085, 0x04881d05,
        0xd9d4d039, 0xe6db99e5, 0x1fa27cf8, 0xc4ac5665,
        0xf4292244, 0x432aff97, 0xab9423a7, 0xfc93a039,
        0x655b59c3, 0x8f0ccc92, 0xffeff47d, 0x85845dd1,
        0x6fa87e4f, 0xfe2ce6e0, 0xa3014314, 0x4e0811a1,
        0xf7537e82, 0xbd3af235, 0x2ad7d2bb, 0xeb86d391,
    };

    private byte[] PadInput(byte[] buffer)
    {
        long padding = ChunkSize - (buffer.LongLength % ChunkSize);
        if (padding == 0)
        {
            padding = ChunkSize;
        }

        long padLen = buffer.LongLength + padding;
        byte[] paddedInput = new byte[padLen];
        buffer.CopyTo(paddedInput, 0);

        paddedInput[buffer.LongLength] = 0x10;
        for (int i = 0; i < padding - sizeof(UInt64) - 1; ++i)
        {
            paddedInput[buffer.LongLength + 1 + i] = 0;
        }

        byte[] lenBytes = BitConverter.GetBytes(buffer.LongLength);
        lenBytes.CopyTo(paddedInput, paddedInput.LongLength - sizeof(UInt64));

        return paddedInput;
    }

    private UInt32[] GetChunkWords(byte[] input, int chunkIdx)
    {
        UInt32[] words = new UInt32[16];

        for (int i = 0; i < words.Length; ++i)
        {
            words[i] = BitConverter.ToUInt32(input, chunkIdx + (i * sizeof(UInt32)));
        }

        return words;
    }

    private UInt32 LeftRotate(UInt32 x, UInt32 c)
    {
        int ci = (int)c;
        return (x << ci) | (x >> (32 - ci));
    }

    public byte[] ComputeHash(byte[] buffer)
    {
        byte[] paddedInput = PadInput(buffer);

        // initialize variables
        UInt32 a0 = 0x67452301;
        UInt32 b0 = 0xefcdab89;
        UInt32 c0 = 0x98badcfe;
        UInt32 d0 = 0x10325476;

        for (int chunkIdx = 0; chunkIdx < paddedInput.Length; chunkIdx += ChunkSize)
        {
            UInt32[] m = GetChunkWords(paddedInput, chunkIdx);

            UInt32 a = a0;
            UInt32 b = b0;
            UInt32 c = c0;
            UInt32 d = d0;

            for (UInt32 i = 0; i < 64; ++i)
            {
                UInt32 f;
                UInt32 g;

                if (i <= 15)
                {
                    f = (b & c) | (~b & d);
                    g = i;
                }
                else if (i <= 31)
                {
                    f = (d & b) | (~d & c);
                    g = (5 * i + 1) % 16;
                }
                else if (i <= 47)
                {
                    f = b ^ c ^ d;
                    g = (3 * i + 5) % 16;
                }
                else
                {
                    f = c ^ (b | ~d);
                    g = (7 * i) % 16;
                }

                f += a + K[i] + m[g];
                a = d;
                d = c;
                c = b;
                b += LeftRotate(f, S[i]);
            }

            a0 += a;
            b0 += b;
            c0 += c;
            d0 += d;
        }

        byte[] hash = new byte[16];
        BitConverter.GetBytes(a0).CopyTo(hash, 0);
        BitConverter.GetBytes(b0).CopyTo(hash, 4);
        BitConverter.GetBytes(c0).CopyTo(hash, 8);
        BitConverter.GetBytes(d0).CopyTo(hash, 12);

        return hash;
    }
}
