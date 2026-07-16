using System.Buffers;
using System.IO;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Cryptography
{
	public static class AES
	{
		private const int NonceSizeBytes = 32;

		private const int MacSizeBits = 128;

		public static byte[] AesGcmEncrypt(byte[] data, byte[] secret, SecureRandom secureRandom)
		{
			byte[] array = ArrayPool<byte>.Shared.Rent(32);
			byte[] array2 = null;
			try
			{
				secureRandom.NextBytes(array, 0, array.Length);
				GcmBlockCipher gcmBlockCipher = new GcmBlockCipher(new AesEngine());
				gcmBlockCipher.Init(forEncryption: true, new AeadParameters(new KeyParameter(secret), 128, array));
				int outputSize = gcmBlockCipher.GetOutputSize(data.Length);
				array2 = ArrayPool<byte>.Shared.Rent(outputSize);
				int outOff = gcmBlockCipher.ProcessBytes(data, 0, data.Length, array2, 0);
				gcmBlockCipher.DoFinal(array2, outOff);
				using MemoryStream memoryStream = new MemoryStream();
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					binaryWriter.Write(array);
					binaryWriter.Write(array2, 0, outputSize);
				}
				return memoryStream.ToArray();
			}
			finally
			{
				ArrayPool<byte>.Shared.Return(array);
				if (array2 != null)
				{
					ArrayPool<byte>.Shared.Return(array2);
				}
			}
		}

		public static byte[] AesGcmDecrypt(byte[] data, byte[] secret)
		{
			using MemoryStream input = new MemoryStream(data);
			using BinaryReader binaryReader = new BinaryReader(input);
			byte[] array = binaryReader.ReadBytes(32);
			GcmBlockCipher gcmBlockCipher = new GcmBlockCipher(new AesEngine());
			gcmBlockCipher.Init(forEncryption: false, new AeadParameters(new KeyParameter(secret), 128, array));
			byte[] array2 = binaryReader.ReadBytes(data.Length - array.Length);
			byte[] array3 = new byte[gcmBlockCipher.GetOutputSize(array2.Length)];
			int outOff = gcmBlockCipher.ProcessBytes(array2, 0, array2.Length, array3, 0);
			gcmBlockCipher.DoFinal(array3, outOff);
			return array3;
		}
	}
}
