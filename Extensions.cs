using System.Collections.Generic;
using System.IO;
using System.Globalization;

namespace DesktopRenamer
{
	public static class Extensions
	{
		public static List<string> Clean( this List<string> list )
		{
			string testAgainst = Path.GetFileNameWithoutExtension( Program.exe );
			string testee = string.Empty;

			for ( int i=list.Count-1; i >= 0; i-- )
			{
				testee = Path.GetFileName( list[i] );
				if ( testee.StartsWith( testAgainst, false, CultureInfo.CurrentCulture ) ||
					 testee.ToLower().Equals("desktop.ini") )
				{
					list.Remove( list[i] );
				}
			}

			return list;
		}
	}
}
