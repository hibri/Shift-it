using System;

namespace FTP_CLClient {
	class Program {
		static void Main (string[] args) {
			Console.WriteLine("This upload tester is fully automatic.");
			Console.WriteLine("Please wait for a line saying \"press [enter] to exit\"");
			Console.WriteLine(" ");

			try {
				Console.WriteLine("Starting...");

				var ff = new FtpFactory();
				ff.setDebug(true);

				ff.setRemoteHost("MY.FTP.SERVER.COM");
				ff.setRemoteUser("USER");
				ff.setRemotePass("PASSWORD");

				ff.ListMode = DirectoryListMode.PlatformList;

				ff.login();
				Console.WriteLine(" ... I think I'm at: " + ff.pwd());


				ff.ensureRemotePath("/test/test/", false);

				ff.chdir("/test/test/");

				ff.ListMode = DirectoryListMode.NameList;
				var fileNames = ff.getFileList("*");
				foreach (string t in fileNames)
				{
					Console.WriteLine(t);
				}

				ff.setBinaryMode(true);
				try {
					ff.PassiveMode = false;
					ff.upload(@"C:\temp\test.txt");
					Console.WriteLine("Uploaded Active");
				} catch {
					ff.PassiveMode = true;
					ff.upload(@"C:\temp\test.txt");
					Console.WriteLine("Uploaded Passive");
				}

				try {
					ff.ShouldOverwrite = true;
					try {
						ff.PassiveMode = false;
						ff.upload(@"C:\temp\test2.txt");
						ff.upload(@"C:\temp\test.txt", @"test2.txt", false);
						Console.WriteLine("Over-write: Uploaded Active");
					} catch {
						ff.PassiveMode = true;
						ff.upload(@"C:\temp\test2.txt");
						ff.upload(@"C:\temp\test.txt", @"test2.txt", false);
						Console.WriteLine("Over-write: Uploaded Passive");
					}
				} catch {
					Console.WriteLine("Over-write DIDN'T work");
				}

				try {
					ff.PassiveMode = false;
					ff.download("test.txt", @"C:\temp\test_result.txt");
					Console.WriteLine("Downloaded Active");
				} catch {
					ff.PassiveMode = true;
					ff.download("test.txt", @"C:\temp\test_result.txt");
					Console.WriteLine("Downloaded Active");
				}
				ff.close();
			} catch (Exception e) {
				Console.WriteLine("Caught Error: " + e.Message);
			}

			Console.WriteLine(" ");
			Console.WriteLine("PRESS [ENTER] TO EXIT");
			Console.ReadLine();
		}
	}
}
