using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Runtime.InteropServices;

//using Accord.Video.FFMPEG;

namespace nilnul.img.anime.of_.screen_.accord_
{
	/// <summary>
	/// collection all information needed and initiate; make it ready to record a block of the window.
	/// </summary>
	public class Frames
	{
		static private string ___WorkingDir = "screenRec";



		public string framesParentFolder;// = "";

		private Rectangle block;

		private int fileIndex = 1;

		/// <summary>
		/// </summary>
		private int framesPerSecond = 10;

		object _lock = new object();
		bool _saving = false;

		System.Timers.Timer timer;

		Stopwatch stopwatch = new Stopwatch();
		public long miliSecondsElapsed;
	
	

		static public  fs.Folder SetupWorkspace_ofDenote(string denote)
		{
			var basis = nilnul.fs.folder_.tmp.denote_.ver_.next_.subIfNeed._CreateFolderX.Folder(denote);
			return (basis);
		}


		static public  fs.Folder SetupWorkspace_ofDenote()
		{
			return SetupWorkspace_ofDenote(___WorkingDir);
		}

		public Frames(Rectangle block__, int fr, nilnul.fs.FolderI framesContainer___)

		{
			block = block__;
			framesPerSecond = fr;

			timer = new System.Timers.Timer();
			timer.Interval = 1000 / framesPerSecond;
			timer.AutoReset = true;
			timer.Elapsed += Timer_Elapsed;

			this.framesParentFolder = framesContainer___.ToString();
		}
		public Frames(Rectangle block__, int fr, string framesContainer___)
			:this(

			block__
			,
			fr
			,
			nilnul.fs.folder._EnsureX.Folder_ofAddress(
			 framesContainer___
			)

		)
		{
		}

	

		public Frames(Rectangle block__, int frameRate__) : this(
			block__,
			 frameRate__,

			 SetupWorkspace_ofDenote(
			___WorkingDir
				 )
		)
		{


		}





		//Record video:
		public void recordFrame()
		{
			lock (_lock)
			{
				/*When SynchronizingObject is null, the method that handles the Elapsed event is called on a thread from the system-thread pool. For more information on system-thread pools, see ThreadPool.*/

				//Keep track of time:
				//watch.Start();
				if (_saving)
				{
					return;
				}

				using (Bitmap bitmap = new Bitmap(block.Width, block.Height))
				{
					using (Graphics g = Graphics.FromImage(bitmap))
					{
						//Add screen to bitmap:
						g.CopyFromScreen(new Point(block.Left, block.Top), Point.Empty, block.Size);
					}
					//Save screenshot:
					string frameAddressS = Path.Combine(framesParentFolder, "frame" + fileIndex + ".png");

					bitmap.Save(frameAddressS, ImageFormat.Png);
					fileIndex++;

					//Dispose of bitmap:
					bitmap.Dispose();
				}

			}
		}



		public void startEvt()
		{


			stopwatch.Start();


			timer.Start();
		}


		private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			recordFrame();

		}

		public void stop()
		{

			timer.Stop();


			stopwatch.Stop();

			/*Even if the SynchronizingObject property is not null, Elapsed events can occur after the Dispose or Stop method has been called or after the Enabled property has been set to false, because the signal to raise the Elapsed event is always queued for execution on a thread pool thread. One way to resolve this race condition is to set a flag that tells the event handler for the Elapsed event to ignore subsequent events.*/



			lock (_lock)
			{
				_saving = true;
				miliSecondsElapsed = stopwatch.ElapsedMilliseconds;

			}





		}
	}
}