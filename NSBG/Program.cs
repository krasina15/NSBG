using Nancy.Hosting.Self;
using System;
using Mono.Unix;

namespace NSBG
{
	class Program
	{
		static void Main(string[] args)
		{
			var uri = "http://localhost:8888";
			Console.WriteLine("Starting Nancy on " + uri);

			// initialize an instance of NancyHost
			var host = new NancyHost(new Uri(uri));
			host.Start();  // start hosting

			// check if we're running on mono
			if (Type.GetType("Mono.Runtime") != null)
			{
				// on mono, processes will usually run as daemons - this allows you to listen
				// for termination signals (ctrl+c, shutdown, etc) and finalize correctly

				UnixSignal.WaitAny(new[] {
										new UnixSignal(Mono.Unix.Native.Signum.SIGINT),
										new UnixSignal(Mono.Unix.Native.Signum.SIGTERM),
										new UnixSignal(Mono.Unix.Native.Signum.SIGQUIT),
										new UnixSignal(Mono.Unix.Native.Signum.SIGHUP)
								});
			}
			else
			{
				Console.ReadLine();
			}

			Console.WriteLine("Stopping Nancy");
			host.Stop();  // stop hosting
		}
	}
}