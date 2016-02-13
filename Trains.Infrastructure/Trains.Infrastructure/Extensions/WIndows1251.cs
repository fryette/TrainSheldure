using System.Text;

namespace Trains.Infrastructure.Extensions
{
	public class Windows1251 : Encoding
	{
		private const string Alpha = "\0\u0001\u0002\u0003\u0004\u0005\u0006\a\b\t\n\v\f\r\u000e\u000f\u0010\u0011\u0012\u0013\u0014\u0015\u0016\u0017\u0018\u0019\u001a\u001b\u001c\u001d\u001e\u001f !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~\u007fЂЃ‚ѓ„…†‡€‰Љ‹ЊЌЋЏђ‘’“”•–—\u0098™љ›њќћџ ЎўЈ¤Ґ¦§Ё©Є«¬­®Ї°±Ііґµ¶·ё№є»јЅѕїАБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдежзийклмнопрстуфхцчшщъыьэюя";

		public override int GetByteCount(char[] chars, int index, int count)
		{
			return count;
		}

		public override string GetString(byte[] bytes, int index, int count)
		{
			var newBytes = new byte[bytes.Length + 1];
			for (var i = 0; i < bytes.Length; i++)
			{
				newBytes[i] = bytes[i];
			}
			newBytes[bytes.Length] = 0;
			return base.GetString(newBytes, index, count + 1);
		}

		public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			var questionIndex = (byte)Alpha.IndexOf('?');
			for (var i = 0; i < charCount; i++)
			{
				var toIndex = byteIndex + i;
				var index = Alpha.IndexOf(chars[charIndex + i]);
				if (index == -1)
					bytes[toIndex] = questionIndex;
				else
					bytes[toIndex] = (byte)index;
			}
			return charCount;
		}

		public override int GetCharCount(byte[] bytes, int index, int count)
		{
			return count;
		}

		public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
		{
			for (var i = 0; i < bytes.Length - 1; i++)
			{
				chars[i + charIndex] = Alpha[bytes[byteIndex + i]];
			}
			return byteCount;
		}

		public override int GetMaxByteCount(int charCount)
		{
			return charCount;
		}

		public override int GetMaxCharCount(int byteCount)
		{
			return byteCount;
		}
	}
}
