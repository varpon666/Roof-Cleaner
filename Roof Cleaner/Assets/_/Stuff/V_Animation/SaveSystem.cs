using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Improved version to C# LibLZF Port:
 * Copyright (c) 2010 Roman Atachiants <kelindar@gmail.com>
 *
 * Original CLZF Port:
 * Copyright (c) 2005 Oren J. Maurice <oymaurice@hazorea.org.il>
 *
 * Original LibLZF Library  Algorithm:
 * Copyright (c) 2000-2008 Marc Alexander Lehmann <schmorp@schmorp.de>
 *
 * Redistribution and use in source and binary forms, with or without modifica-
 * tion, are permitted provided that the following conditions are met:
 *
 *   1.  Redistributions of source code must retain the above copyright notice,
 *       this list of conditions and the following disclaimer.
 *
 *   2.  Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *
 *   3.  The name of the author may not be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR IMPLIED
 * WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MER-
 * CHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO
 * EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPE-
 * CIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS;
 * OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTH-
 * ERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED
 * OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * Alternatively, the contents of this file may be used under the terms of
 * the GNU General Public License version 2 (the "GPL"), in which case the
 * provisions of the GPL are applicable instead of the above. If you wish to
 * allow the use of your version of this file only under the terms of the
 * GPL and not to allow others to use your version of this file under the
 * BSD license, indicate your decision by deleting the provisions above and
 * replace them with the notice and other provisions required by the GPL. If
 * you do not delete the provisions above, a recipient may use your version
 * of this file under either the BSD or the GPL.
 */
using System;

/* Benchmark with Alice29 Canterbury Corpus
        ---------------------------------------
        (Compression) Original CLZF C#
        Raw = 152089, Compressed = 101092
         8292,4743 ms.
        ---------------------------------------
        (Compression) My LZF C#
        Raw = 152089, Compressed = 101092
         33,0019 ms.
        ---------------------------------------
        (Compression) Zlib using SharpZipLib
        Raw = 152089, Compressed = 54388
         8389,4799 ms.
        ---------------------------------------
        (Compression) QuickLZ C#
        Raw = 152089, Compressed = 83494
         80,0046 ms.
        ---------------------------------------
        (Decompression) Original CLZF C#
        Decompressed = 152089
         16,0009 ms.
        ---------------------------------------
        (Decompression) My LZF C#
        Decompressed = 152089
         15,0009 ms.
        ---------------------------------------
        (Decompression) Zlib using SharpZipLib
        Decompressed = 152089
         3577,2046 ms.
        ---------------------------------------
        (Decompression) QuickLZ C#
        Decompressed = 152089
         21,0012 ms.
    */

namespace V_AnimationSystem
{

    /// <summary>
    /// Improved C# LZF Compressor, a very small data compression library. The compression algorithm is extremely fast.
    public static class CLZF2
    {
        private static readonly uint HLOG = 14;
        private static readonly uint HSIZE = (1 << 14);
        private static readonly uint MAX_LIT = (1 << 5);
        private static readonly uint MAX_OFF = (1 << 13);
        private static readonly uint MAX_REF = ((1 << 8) + (1 << 3));

        /// <summary>
        /// Hashtable, that can be allocated only once
        /// </summary>
        private static readonly long[] HashTable = new long[HSIZE];

        // Compresses inputBytes
        public static byte[] Compress(byte[] inputBytes)
        {
            // Starting guess, increase it later if needed
            int outputByteCountGuess = inputBytes.Length * 2;
            byte[] tempBuffer = new byte[outputByteCountGuess];
            int byteCount = lzf_compress(inputBytes, ref tempBuffer);

            // If byteCount is 0, then increase buffer and try again
            while (byteCount == 0)
            {
                outputByteCountGuess *= 2;
                tempBuffer = new byte[outputByteCountGuess];
                byteCount = lzf_compress(inputBytes, ref tempBuffer);
            }

            byte[] outputBytes = new byte[byteCount];
            Buffer.BlockCopy(tempBuffer, 0, outputBytes, 0, byteCount);
            return outputBytes;
        }

        // Decompress outputBytes
        public static byte[] Decompress(byte[] inputBytes)
        {
            // Starting guess, increase it later if needed
            int outputByteCountGuess = inputBytes.Length * 2;
            byte[] tempBuffer = new byte[outputByteCountGuess];
            int byteCount = lzf_decompress(inputBytes, ref tempBuffer);

            // If byteCount is 0, then increase buffer and try again
            while (byteCount == 0)
            {
                outputByteCountGuess *= 2;
                tempBuffer = new byte[outputByteCountGuess];
                byteCount = lzf_decompress(inputBytes, ref tempBuffer);
            }

            byte[] outputBytes = new byte[byteCount];
            Buffer.BlockCopy(tempBuffer, 0, outputBytes, 0, byteCount);
            return outputBytes;
        }

        /// <summary>
        /// Compresses the data using LibLZF algorithm
        /// </summary>
        /// <param name="input">Reference to the data to compress</param>
        /// <param name="output">Reference to a buffer which will contain the compressed data</param>
        /// <returns>The size of the compressed archive in the output buffer</returns>
        public static int lzf_compress(byte[] input, ref byte[] output)
        {
            int inputLength = input.Length;
            int outputLength = output.Length;

            Array.Clear(HashTable, 0, (int)HSIZE);

            long hslot;
            uint iidx = 0;
            uint oidx = 0;
            long reference;

            uint hval = (uint)(((input[iidx]) << 8) | input[iidx + 1]); // FRST(in_data, iidx);
            long off;
            int lit = 0;

            for (; ; )
            {
                if (iidx < inputLength - 2)
                {
                    hval = (hval << 8) | input[iidx + 2];
                    hslot = ((hval ^ (hval << 5)) >> (int)(((3 * 8 - HLOG)) - hval * 5) & (HSIZE - 1));
                    reference = HashTable[hslot];
                    HashTable[hslot] = (long)iidx;


                    if ((off = iidx - reference - 1) < MAX_OFF
                        && iidx + 4 < inputLength
                        && reference > 0
                        && input[reference + 0] == input[iidx + 0]
                        && input[reference + 1] == input[iidx + 1]
                        && input[reference + 2] == input[iidx + 2]
                        )
                    {
                        /* match found at *reference++ */
                        uint len = 2;
                        uint maxlen = (uint)inputLength - iidx - len;
                        maxlen = maxlen > MAX_REF ? MAX_REF : maxlen;

                        if (oidx + lit + 1 + 3 >= outputLength)
                            return 0;

                        do
                            len++;
                        while (len < maxlen && input[reference + len] == input[iidx + len]);

                        if (lit != 0)
                        {
                            output[oidx++] = (byte)(lit - 1);
                            lit = -lit;
                            do
                                output[oidx++] = input[iidx + lit];
                            while ((++lit) != 0);
                        }

                        len -= 2;
                        iidx++;

                        if (len < 7)
                        {
                            output[oidx++] = (byte)((off >> 8) + (len << 5));
                        }
                        else
                        {
                            output[oidx++] = (byte)((off >> 8) + (7 << 5));
                            output[oidx++] = (byte)(len - 7);
                        }

                        output[oidx++] = (byte)off;

                        iidx += len - 1;
                        hval = (uint)(((input[iidx]) << 8) | input[iidx + 1]);

                        hval = (hval << 8) | input[iidx + 2];
                        HashTable[((hval ^ (hval << 5)) >> (int)(((3 * 8 - HLOG)) - hval * 5) & (HSIZE - 1))] = iidx;
                        iidx++;

                        hval = (hval << 8) | input[iidx + 2];
                        HashTable[((hval ^ (hval << 5)) >> (int)(((3 * 8 - HLOG)) - hval * 5) & (HSIZE - 1))] = iidx;
                        iidx++;
                        continue;
                    }
                }
                else if (iidx == inputLength)
                    break;

                /* one more literal byte we must copy */
                lit++;
                iidx++;

                if (lit == MAX_LIT)
                {
                    if (oidx + 1 + MAX_LIT >= outputLength)
                        return 0;

                    output[oidx++] = (byte)(MAX_LIT - 1);
                    lit = -lit;
                    do
                        output[oidx++] = input[iidx + lit];
                    while ((++lit) != 0);
                }
            }

            if (lit != 0)
            {
                if (oidx + lit + 1 >= outputLength)
                    return 0;

                output[oidx++] = (byte)(lit - 1);
                lit = -lit;
                do
                    output[oidx++] = input[iidx + lit];
                while ((++lit) != 0);
            }

            return (int)oidx;
        }


        /// <summary>
        /// Decompresses the data using LibLZF algorithm
        /// </summary>
        /// <param name="input">Reference to the data to decompress</param>
        /// <param name="output">Reference to a buffer which will contain the decompressed data</param>
        /// <returns>Returns decompressed size</returns>
        public static int lzf_decompress(byte[] input, ref byte[] output)
        {
            int inputLength = input.Length;
            int outputLength = output.Length;

            uint iidx = 0;
            uint oidx = 0;

            do
            {
                uint ctrl = input[iidx++];

                if (ctrl < (1 << 5)) /* literal run */
                {
                    ctrl++;

                    if (oidx + ctrl > outputLength)
                    {
                        //SET_ERRNO (E2BIG);
                        return 0;
                    }

                    do
                        output[oidx++] = input[iidx++];
                    while ((--ctrl) != 0);
                }
                else /* back reference */
                {
                    uint len = ctrl >> 5;

                    int reference = (int)(oidx - ((ctrl & 0x1f) << 8) - 1);

                    if (len == 7)
                        len += input[iidx++];

                    reference -= input[iidx++];

                    if (oidx + len + 2 > outputLength)
                    {
                        //SET_ERRNO (E2BIG);
                        return 0;
                    }

                    if (reference < 0)
                    {
                        //SET_ERRNO (EINVAL);
                        return 0;
                    }

                    output[oidx++] = output[reference++];
                    output[oidx++] = output[reference++];

                    do
                        output[oidx++] = output[reference++];
                    while ((--len) != 0);
                }
            }
            while (iidx < inputLength);

            return (int)oidx;
        }

    }

}

namespace V_AnimationSystem {

    /*
     * Animation Save System
     * */
    public static class SaveSystem {


        public struct HeaderData {

            private static string[] prevVersions = new[] { "0.01" };
            private const string version = "1.00";
            private const string versionDate = "12-05-2018";
            private static byte versionByte = (byte)prevVersions.Length;


            public int saveByteLength;
            public string animationVersion;
            public string headerDataVersion;
            public string date;


            public static HeaderData Generate(string saveContent) {
                HeaderData headerData;
                headerData.saveByteLength = System.Text.Encoding.UTF8.GetBytes(saveContent).Length;
                headerData.animationVersion = V_Animation.version;
                headerData.headerDataVersion = version;
                headerData.date = System.DateTime.Now.ToString("dd-MM-yy_HH-mm-ss");
                return headerData;
            }
            public static string Save(string saveContent) {
                return Save(Generate(saveContent));
            }
            public static string Save(HeaderData headerData) {
                string[] content = new string[]{
                headerData.saveByteLength+"",
                headerData.animationVersion,
                headerData.date,
                headerData.headerDataVersion,
            };
                return string.Join("#HEADER#", content);
            }
            public static HeaderData Load(string header) {
                string[] content = V_Animation.SplitString(header, "#HEADER#");
                HeaderData headerData;
                headerData.saveByteLength = int.Parse(content[0]);
                headerData.animationVersion = content[1];
                headerData.date = content[2];
                headerData.headerDataVersion = content[3];
                return headerData;
            }

            public override string ToString() {
                return
                    "FILE HEADER" + "\n" +
                    "saveByteLength: " + saveByteLength + "\n" +
                    "gameVersion: " + animationVersion + "\n" +
                    "headerDataVersion: " + headerDataVersion + "\n" +
                    "date: " + date + "\n" +
                    "";
            }
        }


        public struct FileData {

            public enum FileType {
                Animation,
                AnimationType,
                UnitInfo,
                MapSpawns,
                GameState_Stats,
                NinjaTycoonSave,
            }

            public FileType fileType;
            public HeaderData headerData;
            public string save;

            public static FileData Load(byte[] byteArr) {
                FileType fileType = (FileType)byteArr[0];
                int fileTypeByteAmount = 1;
                byte[] headerSizeBytes = new byte[] { byteArr[1], byteArr[2] };
                int headerSizeByteAmount = headerSizeBytes.Length;
                short headerSize = System.BitConverter.ToInt16(headerSizeBytes, 0);

                byte[] headerBytes = new byte[headerSize];
                System.Array.Copy(byteArr, fileTypeByteAmount + headerSizeByteAmount, headerBytes, 0, headerSize);
                string headerCompressed = System.Text.Encoding.UTF8.GetString(headerBytes);
                string header = GetUnCompressedString(headerCompressed);

                HeaderData headerData = HeaderData.Load(header);

                byte[] saveBytes = new byte[headerData.saveByteLength];
                System.Array.Copy(byteArr, fileTypeByteAmount + headerSizeByteAmount + headerBytes.Length, saveBytes, 0, headerData.saveByteLength);
                string saveCompressed = System.Text.Encoding.UTF8.GetString(saveBytes);

                string save = GetUnCompressedString(saveCompressed);

                FileData fileData;
                fileData.fileType = fileType;
                fileData.headerData = headerData;
                fileData.save = save;

                return fileData;
            }

            public static byte[] Save(FileType fileType, string header, string save) {
                // Convert header into bytes
                byte fileTypeByte = (byte)fileType;
                byte[] headerBytes = System.Text.Encoding.UTF8.GetBytes(header);

                // Save 2 bytes for header length
                short headerSize = (short)headerBytes.Length;
                byte[] headerSizeBytes = System.BitConverter.GetBytes(headerSize);

                // Get save bytes
                byte[] saveBytes = System.Text.Encoding.UTF8.GetBytes(save);

                // Merge all together
                List<byte> totalBytes = new List<byte>();
                totalBytes.Add(fileTypeByte);
                totalBytes.AddRange(headerSizeBytes);
                totalBytes.AddRange(headerBytes);
                totalBytes.AddRange(saveBytes);

                return totalBytes.ToArray();
            }
        }









        public static string GetCompressedString(string save) {
            return System.Convert.ToBase64String(CLZF2.Compress(System.Text.Encoding.ASCII.GetBytes(save)));
        }
        public static string GetUnCompressedString(string save) {
            return System.Text.Encoding.ASCII.GetString(CLZF2.Decompress(System.Convert.FromBase64String(save)));
        }





        public static void Save(string folderPath, string saveName, FileData.FileType fileType, string saveString) {
            byte[] fileSaveBytes;
            Save(folderPath, saveName, fileType, saveString, out fileSaveBytes);
        }
        public static void Save(string folderPath, string saveName, FileData.FileType fileType, string saveString, out byte[] fileSaveBytes) {
            // Save name containing extension
            string saveFile = folderPath + saveName;
            Save(saveFile, fileType, saveString, out fileSaveBytes);
        }

        public static void Save(string fullSavePath, FileData.FileType fileType, string saveString, out byte[] fileSaveBytes) {
            // Save name containing extension
            string saveFile = fullSavePath;
            //Compress Save
            string saveStringCompressed = GetCompressedString(saveString);
            string header = GetCompressedString(Save_Header(saveStringCompressed));

            fileSaveBytes = FileData.Save(fileType, header, saveStringCompressed);

            File.WriteAllBytes(saveFile, fileSaveBytes);
        }

        public static bool Load(string folderPath, string file, out FileData fileData) {
            // Byte save
            return Load(folderPath + file, out fileData);
        }
        public static bool Load(string fullFilePath, out FileData fileData) {
            // File name containing extension
            // Assumes extension is 3 characters long
            byte[] readAllBytes = File.ReadAllBytes(fullFilePath);

            return Load(readAllBytes, out fileData);
        }
        public static bool Load(byte[] byteArr, out FileData fileData) {
            try {
                fileData = FileData.Load(byteArr);
                return true;
            } catch (Exception e) {
                Debug.Log("Load Failed: " + e);
                fileData = default(FileData);
                return false;
            }
        }



        private static string Save_Header(string saveContent) {
            //Returns a string to be used in savefiles
            return HeaderData.Save(saveContent);
        }
        private static HeaderData Load_Header(string header) {
            HeaderData headerData = HeaderData.Load(header);
            return headerData;
        }
    }

}