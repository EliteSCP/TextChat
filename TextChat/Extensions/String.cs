namespace TextChat.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public static class String
	{
		public static string GetRawUserId(this string userId)
		{
			int index = userId.LastIndexOf('@');

			if (index == -1)
				return userId;

			return userId.Substring(0, index);
		}

		public static string Sanitize(this string stringToSanitize, IEnumerable<string> badWords, char badWordsChar)
		{
			foreach (string badWord in badWords)
				stringToSanitize = Regex.Replace(stringToSanitize, badWord, new string(badWordsChar, badWord.Length), RegexOptions.IgnoreCase);

			return stringToSanitize;
		}

		public static string GetMessage(this ArraySegment<string> args, int skips = 0, string separator = " ")
		{
			return string.Join(separator, skips == 0 ? args : args.Skip(skips).Take(args.Count - skips));
		}
	}
}
