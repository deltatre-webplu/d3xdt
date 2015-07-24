using System;
using System.IO;
using System.Text;
using Microsoft.Web.XmlTransform;

namespace d3xdt
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			if (args == null || args.Length < 3)
			{
				ShowUsage();
				return;
			}

			var sourceFile = args[0];
			var transformFile = args[1];
			var destFile = args[2] ?? sourceFile;

			if (!File.Exists(sourceFile))
			{
				throw new FileNotFoundException("sourceFile doesn't exist");
			}
			if (!File.Exists(transformFile))
			{
				throw new FileNotFoundException("transformFile doesn't exist");
			}

			using (var document = new XmlTransformableDocument())
			{
				document.PreserveWhitespace = true;
				document.Load(sourceFile);

				using (var transform = new XmlTransformation(transformFile))
				{
					var success = transform.Apply(document);

					document.Save(destFile);

					Console.WriteLine("\nSaved transformation at '{0}'\n\n", new FileInfo(destFile).FullName);

					var exitCode = (success) ? 0 : 1;
					Environment.Exit(exitCode);
				}
			}
		}

		private static void ShowUsage()
		{
			var sb = new StringBuilder();
			sb.AppendLine("\n\nIncorrect set of arguments");
			sb.AppendLine("\tXdtSample.exe sourceXmlFile transformFile destFile\n\n");
			Console.Write(sb.ToString());
		}
	}
}