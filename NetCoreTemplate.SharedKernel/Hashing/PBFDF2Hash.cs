namespace NetCoreTemplate.SharedKernel.Hashing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;

    public sealed class PBFDF2Hash
    {
        private static int _SaltBytes { get; set; }

        public static int SaltBytes
        {
            get
            {
                if (_SaltBytes == default(int))
                {
                    _SaltBytes = RandomNumber(15, 25);
                }

                return _SaltBytes;
            }
        }

        private static int _PBKDF2Iterations { get; set; }

        public static int PBKDF2Iterations
        {
            get
            {
                if (_PBKDF2Iterations == default(int))
                {
                    _PBKDF2Iterations = RandomNumber(32000, 64000);
                }

                return _PBKDF2Iterations;
            }
        }

        private static int _HashBytes { get; set; }

        public static int HashBytes
        {
            get
            {
                if (_HashBytes == default(int))
                {
                    _HashBytes = RandomNumber(15, 20);
                }

                return _HashBytes;
            }
        }

        public static int _Multiply { get; set; }

        public static int Multiply
        {
            get
            {
                if (_Multiply == default(int))
                {
                    _Multiply = RandomNumber(0, 17);
                }

                return _Multiply;
            }
        }

        public static string Hash(string input)
        {
            string password;
            bool verified;

            do
            {
                password = GenerateHash(input);
                verified = Verify(input, password);
            }
            while (!verified);

            return password;
        }

        public static bool Verify(string input, string storedHash)
        {
            try
            {
                var regex = new Regex(@"!(.+?)!");
                var result = regex.Matches(storedHash);

                var hashLengthIndex = result.First();
                var hashLengthString = hashLengthIndex.Groups[1].Value;
                var hashLength = int.Parse(hashLengthString);

                var multiplyIndex = result.Last();
                var multiplyString = multiplyIndex.Groups[1].Value;

                var multiply = int.Parse(multiplyString);

                var iterationsBase64 = storedHash.Substring(hashLengthIndex.Length, 8);
                var iterationsBytes = Convert.FromBase64String(iterationsBase64);
                var iterationsString = Encoding.UTF8.GetString(iterationsBytes);
                var iterations = int.Parse(iterationsString) / multiply;

                var hashSizeBase64 = storedHash.Substring(hashLengthIndex.Length + 8, 4);
                var hashSizeBytes = Convert.FromBase64String(hashSizeBase64);
                var hashSizeString = Encoding.UTF8.GetString(hashSizeBytes);
                var hashSize = int.Parse(hashSizeString) / multiply;

                var hashIndex = storedHash.Length - (hashLength + multiplyIndex.Length);
                var hashString = storedHash.Substring(hashIndex, hashLength);
                var hash = Convert.FromBase64String(hashString);

                var saltLength = storedHash.Length - (hashString.Length + multiplyIndex.Length + 12 + hashLengthIndex.Length);
                var saltBase64 = storedHash.Substring(hashLengthIndex.Length + 12, saltLength);
                var salt = Convert.FromBase64String(saltBase64);

                var testHash = PBKDF2(input, salt, iterations, hashSize);

                return SlowEquals(hash, testHash);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static string GenerateHash(string input)
        {
            var salt = new byte[SaltBytes];

            try
            {
                using (var rngCryptoProvider = new RNGCryptoServiceProvider())
                {
                    rngCryptoProvider.GetBytes(salt);
                }
            }
            catch (CryptographicException ex)
            {
                throw new CannotPerformOperationException("Random number generator not available", ex);
            }
            catch (ArgumentNullException ex)
            {
                throw new CannotPerformOperationException("Invalid argument given to random number generator", ex);
            }

            var iterations = PBKDF2Iterations;
            var hash = PBKDF2(input, salt, iterations, HashBytes);

            var iterationsHash = Encoding.ASCII.GetBytes($"{iterations * Multiply}");
            var hashSize = Encoding.ASCII.GetBytes($"{hash.Length * Multiply}");

            var hashBase64 = Convert.ToBase64String(hash);
            var hashParts = new List<string>
            {
                $"!{hashBase64.Length}!",
                Convert.ToBase64String(iterationsHash),
                Convert.ToBase64String(hashSize),
                Convert.ToBase64String(salt),
                hashBase64,
                $"!{Multiply}!"
            };

            var hashString = $@"{hashParts[0]}{hashParts[1]}{hashParts[2]}{hashParts[3]}{hashParts[4]}{hashParts[5]}";

            Reset();

            return hashString;
        }

        private static void Reset()
        {
            _PBKDF2Iterations = default(int);
            _HashBytes = default(int);
            _Multiply = default(int);
        }

        private static int RandomNumber(int min, int max)
        {
            var random = new Random();
            return random.Next(min, max);
        }

        private static bool SlowEquals(IReadOnlyList<byte> a, IReadOnlyList<byte> b)
        {
            var diff = (uint)a.Count ^ (uint)b.Count;

            for (var i = 0; i < a.Count && i < b.Count; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }

            return diff == 0;
        }

        private static byte[] PBKDF2(string input, byte[] salt, int iterations, int outputBytes)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(input, salt))
            {
                pbkdf2.IterationCount = iterations;
                return pbkdf2.GetBytes(outputBytes);
            }
        }
    }
}
