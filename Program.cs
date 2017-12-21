using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace DesktopRenamer
{
	class Program
	{
		readonly static Version version = new Version( 1, 0 );
		readonly static string path = AppDomain.CurrentDomain.BaseDirectory;
		public readonly static string exe = AppDomain.CurrentDomain.FriendlyName;
		readonly static string syntax = string.Format( "{0}{0}{4} [.] [(--dry-run|-dr)]{0}{0}Version {5}{0}{1}{0}{2}{0}{3}{0}{0}",
														Environment.NewLine,
														"- without any arguments, this help is displayed.",
														"- the dot is mandatory to prevent accidentally running of the program.",
														"- '--dry-run' or '-d' just writes logs, but doesn't touch a file or folder (case-insensitive).",
														exe,
														version.ToString() );

		static void Main( string[] args )
		{
			if ( args == null || !( args.Length > 0 ) )
			{
				Console.WriteLine( syntax );
				Console.ReadKey();
				Environment.Exit( 0 );
			}

			if ( args[ 0 ] != "." )
			{
				Console.WriteLine( string.Format( "{0}{0}Annotator '.' is missing!{0}{0}", Environment.NewLine ) );
				Console.ReadKey();
				Environment.Exit( 0 );
			}

			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();

			List<string> files = GetFiles();

			if ( !( files.Count > 0 ) )
			{
				Console.WriteLine( "No files found. Chickening out..." );
				Console.ReadKey();
				Environment.Exit( 0 );
			}

			Statistic( files );

			bool simulate = false;

			if ( args.Length > 1 && ( args[ 1 ].ToUpper().Equals( "-D" ) || args[ 1 ].ToUpper().Equals( "--DRY-RUN" ) ) )
			{
				simulate = true;
			}

			string targetPath, targetFile;
			targetPath = targetFile = string.Empty;
			WriteNewLine( 2 );

			foreach ( string file in files )
			{
				string c = Path.GetFileNameWithoutExtension( file )[ 0 ].ToString().ToUpper();
				targetPath = Path.Combine( path, c );

				if ( !Directory.Exists( targetPath ) )
				{
					if ( !simulate )
					{
						Directory.CreateDirectory( targetPath );
					}
					Console.WriteLine( string.Format( "Created directory '{0}'", c ) );
				}

				targetFile = Path.Combine( targetPath, Path.GetFileName( file ) );

				if ( !File.Exists( targetFile ) )
				{
					if ( !simulate )
					{
						File.Move( file, targetFile );
					}
					Console.WriteLine( string.Format( "{0} => {1}", Path.GetFileName( file ), Path.Combine( c, Path.GetFileName( file ) ) ) );
				}
				else
				{
					File.Delete( file );
					Console.WriteLine( string.Format( "File '{0}\\{1}' already exists in target, deleted.", c, Path.GetFileName( file ) ) );
				}

				targetPath = targetFile = string.Empty;
			}

			stopwatch.Stop();
			Console.WriteLine( "{0}{0}Processed {1} files in {2} seconds.{0}{0}Press any key to exit{0}",
								Environment.NewLine,
								files.Count,
								stopwatch.ElapsedMilliseconds/1000);
			Console.ReadKey();
		}

		/*
		static void ProgressBar( int progress, int total )
		{
			//draw empty progress bar
			Console.CursorLeft = 0;
			Console.Write( "[" ); //start
			Console.CursorLeft = 32;
			Console.Write( "]" ); //end
			Console.CursorLeft = 1;
			float onechunk = 30.0f / total;

			//draw filled part
			int position = 1;
			for ( int i = 0; i < onechunk * progress; i++ )
			{
				Console.BackgroundColor = ConsoleColor.Gray;
				Console.CursorLeft = position++;
				Console.Write( " " );
			}

			//draw unfilled part
			for ( int i = position; i <= 31; i++ )
			{
				Console.BackgroundColor = ConsoleColor.Black;
				Console.CursorLeft = position++;
				Console.Write( " " );
			}

			//draw totals
			Console.CursorLeft = 35;
			Console.BackgroundColor = ConsoleColor.Black;
			Console.Write( progress.ToString() + " of " + total.ToString() + "    " ); //blanks at the end remove any excess
		}
		*/

		static void Statistic( List<string> files )
		{
			Dictionary<char, int> map = new Dictionary<char, int>();

			foreach ( string file in files )
			{
				char c = Path.GetFileName(file)[0];
				if ( map.ContainsKey( c ) )
				{
					map[ c ]++;
				}
				else
				{
					map.Add( c, 1 );
				}
			}

			Console.WriteLine( string.Format("{1}{0} files found:{1}", files.Count, Environment.NewLine ) );
			foreach ( KeyValuePair<char, int> item in map )
			{
				Console.WriteLine( string.Format( "Char '{0}' : {1} files", item.Key, item.Value ) );
			}
			Console.WriteLine( string.Format( "{0}Press any key to continue{0}", Environment.NewLine ) );
			Console.ReadKey();
		}

		static List<string> GetFiles()
		{
			string[] array = Directory.GetFiles( path, "*", SearchOption.TopDirectoryOnly );
			List<string> files = new List<string>( array );
			return files.Clean();
		}

		static void WriteNewLine( int times )
		{
			for ( int i = 0; i < times; i++ )
			{
				Console.WriteLine( Environment.NewLine );
			}
		}

	}
}