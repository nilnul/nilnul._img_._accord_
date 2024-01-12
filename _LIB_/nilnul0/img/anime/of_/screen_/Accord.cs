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

using Accord.Video.FFMPEG;

namespace nilnul.img.anime.of_.screen_
{
	/// <summary>
	/// collection all information needed and initiate; make it ready to record a block of the window.
	/// </summary>
	public class Accord
	{

		public string framesParentFolder;// = "";
		public string animeAddress;

		//Video variables:
		private Rectangle block;

		static private string ___workingDir = "screenRec";
		//private string folder4frames;
		private int fileIndex = 1;

		/// <summary>
		/// the docs
		/// </summary>
		private List<string> address4frameS = new List<string>();
		private int framesPerSecond = 10;

		//File variables:

		//private string __animeAddress
		//{
		//	get
		//	{
		//		return Path.Combine(framesParentFolder, "anime", miliSecondsElapsed + ".mp4");
		//	}
		//}




		System.Timers.Timer timer;
		//ScreenRecorder Object:
		//private Blocked(Rectangle block__, nilnul.fs.Folder workingFolder, int fr)
		//{



		//	timer = new System.Timers.Timer(
		//		1000/framesPerSecond
		//	);

		//	timer.Elapsed += Timer_Elapsed;



		//}


		static public (string, fs.address_.spear_.ParentDoc) SetupWorkspace(nilnul.fs.Folder basis)
		{



			var framesParentFolder = nilnul.fs.folder.denote_.vered_.next_.subIfNeed._CreateFolderX.Folder(basis, "_frames_").ToString();

			var animeSpear = nilnul.fs.address_.spear_.ParentDoc.Create_ofContainerAsAddress(basis.ToString(), "output.mpr");

			return (framesParentFolder, animeSpear);

		}

		static public (string, fs.address_.spear_.ParentDoc) SetupWorkspace_ofDenote(string denote)
		{

			var basis = nilnul.fs.folder_.tmp.denote_.ver_.next_.subIfNeed._CreateFolderX.Folder(denote);


			return SetupWorkspace(basis);

		}


		object _lock = new object();



		public Accord(Rectangle block__, int fr, string framesContainer___, string outputFile)

		{
			//Set variables:
			block = block__;
			framesPerSecond = fr;

			timer = new System.Timers.Timer();
			timer.Interval = 1000 / framesPerSecond;
			timer.AutoReset = true;
			timer.Elapsed += Timer_Elapsed;

			// workingFolder.ToString();
			this.framesParentFolder = framesContainer___;
			this.animeAddress = outputFile;

			//resetWorkspace();

			//timer.SynchronizingObject =// new System.Windows.Forms.Form();
			//resetWorkspace();
		}
		public Accord(Rectangle block__, int frameRate__, (string, string) basis) : this(
			block__
			,
			frameRate__
			,
			 basis.Item1, basis.Item2

		)
		{
		}
		public Accord(Rectangle block__, int frameRate__, (string, nilnul.fs.address_.SpearI) basis)
			: this(
			block__
			,
			frameRate__
			,
			 basis.Item1, basis.Item2.ToString()

		)
		{
		}

		public Accord(Rectangle block__, int frameRate__, nilnul.fs.Folder basis) : this(
			block__
			,
			frameRate__
			,
			 SetupWorkspace(basis)

		)
		{ }
		public Accord(Rectangle block__, int frameRate__, string basis) : this(
			block__
			,
			frameRate__
			,
			nilnul.fs.folder._EnsureX.Folder_ofAddress(basis)

		)
		{

		}

		public Accord(Rectangle block__, int frameRate__) : this(
			block__,
			 frameRate__,

			 SetupWorkspace_ofDenote(
			"screenRecorder"
				 )
		)
		{


		}




		//Clean up program on crash:
		public void dropFrames()
		{
			if (Directory.Exists(framesParentFolder))
			{
				nilnul.fs.folder.drop_._RecyclableX.Del(framesParentFolder);
			}
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

					address4frameS.Add(frameAddressS);
					bitmap.Save(frameAddressS, ImageFormat.Png);
					fileIndex++;

					//Dispose of bitmap:
					bitmap.Dispose();
				}

			}
		}

		bool _saving = false;
		//Save video file:
		private void save()
		{
			lock (_lock)
			{
				_saving = true;
				miliSecondsElapsed = stopwatch.ElapsedMilliseconds;


				using (VideoFileWriter vFWriter = new VideoFileWriter())
				{
					//Create new video file:
					vFWriter.Open(
						animeAddress
						, block.Width
						, block.Height,
						//new Accord.Math.Rational(
						address4frameS.Count * 1000 / miliSecondsElapsed
						//)
						//framesPerSecond
						,
						VideoCodec.MPEG4
					);

					//Make each screenshot into a video frame:

					foreach (string frame in address4frameS)
					{
						Bitmap imageFrame = System.Drawing.Image.FromFile(frame) as Bitmap;
						vFWriter.WriteVideoFrame(imageFrame);
						imageFrame.Dispose();
					}

					//Close:
					vFWriter.Close();
				}

			}
		}


		public void startEvt()
		{

			//resetWorkspace();

			stopwatch.Start();


			timer.Start();
		}

		Stopwatch stopwatch = new Stopwatch();

		private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			recordFrame();

		}

		private long miliSecondsElapsed;
		public void stop()
		{

			timer.Stop();


			stopwatch.Stop();

			/*Even if the SynchronizingObject property is not null, Elapsed events can occur after the Dispose or Stop method has been called or after the Enabled property has been set to false, because the signal to raise the Elapsed event is always queued for execution on a thread pool thread. One way to resolve this race condition is to set a flag that tells the event handler for the Elapsed event to ignore subsequent events.*/

			//Save video:


			save();


			//Delete the screenshots and temporary folder:
			dropFrames();


		}
	}
}