using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Trains.Infrastructure.Extensions
{
	public class FormUrlEncodedContent : ByteArrayContent
	{
		private static readonly Windows1251 Windows1251 = new Windows1251();
		public FormUrlEncodedContent(IEnumerable<KeyValuePair<string, string>> nameValueCollection)
			: base(EncodeContent(nameValueCollection))
		{
			Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
		}

		static byte[] EncodeContent(IEnumerable<KeyValuePair<string, string>> nameValueCollection)
		{
			if (nameValueCollection == null)
				throw new ArgumentNullException(nameof(nameValueCollection));

			var sb = new List<byte>();
			foreach (var item in nameValueCollection)
			{
				if (sb.Count != 0)
					sb.Add((byte)'&');

				var data = SerializeValue(item.Key);
				if (data != null)
					sb.AddRange(data);
				sb.Add((byte)'=');

				data = SerializeValue(item.Value);
				if (data != null)
					sb.AddRange(data);
			}

			return sb.ToArray();
		}

		static byte[] SerializeValue(string value)
		{
			if (value == null)
				return null;
			value = ConvertToUtf8(value);
			return new Windows1251().GetBytes(value);
		}

		public static string ConvertToUtf8(string str)
		{
			if (str == string.Empty) return str;
			var originalByteString = Encoding.UTF8.GetBytes(str);
			var convertedByteString = Encoding.Convert(Encoding.UTF8,
				Windows1251, originalByteString);
			var finalString = Windows1251.GetString(convertedByteString, 0, convertedByteString.Length - 1);
			return finalString;
		}
	}
}