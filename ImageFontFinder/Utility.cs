using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageFontFinder
{
	public static class Utility
	{
		private static readonly Regex cjkCharRegex = new Regex(@"\p{IsCJKUnifiedIdeographs}");
		public static bool IsChinese(this char c)
		{
			return cjkCharRegex.IsMatch(c.ToString());
		}
	}
}
