using System;

namespace TinkLibraryJniForNet
{
    class Program
    {
        private const string _paymentSampleToken = "{\"signature\":\"MEUCIQCovQ0tGptXIXWG2Dmz4Rj3nJ0L2c9lj9j9CC5yGvv0BAIgTi9s1Qgs6GrIe5bpI/emThUuu5keOcVxwWyx8eNXLYU\\u003d\",\"intermediateSigningKey\":{\"signedKey\":\"{\\\"keyValue\\\":\\\"MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAEKyoSUKDpyCbf0xB484CC+iXHzmNQ/qOttFKlePfuOo0QT7neC9O0pROUHGnDougKpR/TMjQjYbzE0yg1SgRdJw\\\\u003d\\\\u003d\\\",\\\"keyExpiration\\\":\\\"1588362031469\\\"}\",\"signatures\":[\"MEUCIEePwtLdWxjKh6LeMLh/Nvi1yYAh+poJm9M7VKwdomQTAiEA1+Hf8JOHOyrtKlaaRtNxWI7zVt9eQQMZUspoaO2Zcks\\u003d\"]},\"protocolVersion\":\"ECv2\",\"signedMessage\":\"{\\\"encryptedMessage\\\":\\\"3Q++43GmAn+8OXqYn8UU6M4rZT0IjlSqDlGabPMg+s5Z1YkM3xEjR3VS51rFQmuhnprHe4LJPTptwQhlQGKSJhZwsvBktPzNXm3qoP2JHwYQwAWMLxm+kk+qxxa6mf6e89Vcts3fbPDht0B3wEh2kHqYjYt9DF5s5hwRGxNDBo5RYn0Xyh1exOpoNoJwQurppgsc3Nq3ijuQQFlB3GBWDwdlCcZE2zJNzkvvXpZ6MOwMszmRpFX7cMduaJnYJuzmajAGiZ5h3GVyfkGc0QWj1ayutwQh8A84NA/89n4uMkFicuCiw5Mi6QTpxFeT/C1xs0P89MBkI44HrP1KMMXdgay564I36ZKgGr/HQm45IGpeLUn2y+EsqWBhmJyQUTA+sgqaQL8Xq23f2FYh9lzZ7jB/nQIryHQ0yACpv2P08gBLCFSltmKEeK4AIM0\\\\u003d\\\",\\\"ephemeralPublicKey\\\":\\\"BBv1CAD8F1m7sPE3PHv87p5pq2Osmw2c+UB8q1YbCJrCQPVRSdnkYnYOq9/tULyfM7uyHODz3MgzC1lpUU6m6Ko\\\\u003d\\\",\\\"tag\\\":\\\"s59nweJsTXwV1qSs5nYpT0B2OStHNOtVeTNcdTrQQ4c\\\\u003d\\\"}\"}";
        static void Main(string[] args)
        {
            var decryptor = new TinkDecryptor();
            // print out payment sample text
            Console.Write("Sample Token: ");
            Console.WriteLine(_paymentSampleToken);
            Console.WriteLine();
            // decrypt and print out plain text
            Console.Write("Plain Text: ");
            Console.WriteLine(decryptor.Decrypt(_paymentSampleToken));
            Console.ReadLine();
        }
    }
}
