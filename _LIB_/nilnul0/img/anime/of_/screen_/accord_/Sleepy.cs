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

namespace nilnul.img.anime.recorder_.accord_
{
	/// <summary>
	/// use not timer, but thread.Sleep
	/// collection all information needed and initiate; make it ready to record a block of the window.
	/// </summary>
	public class Doze
	{


		//Video variables:
		private Rectangle block;
		public string workingFolder;// = "";

		private string workingDir="screenRec";
		private string folder4frames;
		private int fileCount = 1;

		/// <summary>
		/// the docs
		/// </summary>
		private  List<string> address4frameS = new List<string>();
		private int framesPerSecond = 10;

		//File variables:

		public string animeAddress {
			get {
				return Path.Combine(workingFolder, miliSecondsElapsed + ".mp4");
			}
		}


		System.Timers.Timer timer;

		System.Threading.Timer timer1;
		//ScreenRecorder Object:
		//private Blocked(Rectangle block__, nilnul.fs.Folder workingFolder, int fr)
		//{
			

		
		//	timer = new System.Timers.Timer(
		//		1000/framesPerSecond
		//	);

		//	timer.Elapsed += Timer_Elapsed;



		//}

		public void resetWorkspace() {

			var __workingFolder = nilnul.fs.folder_.tmp.denote_.ver_.next_.subIfNeed._CreateFolderX.Folder(this.workingDir);
			this.workingFolder=__workingFolder.ToString();

			folder4frames = nilnul.fs.folder.denote_.vered_.next_.subIfNeed._CreateFolderX.Folder(__workingFolder, "_frames_").ToString();

			address4frameS = new  List<string>();


		}

		object _lock;
		public Doze(Rectangle block__, int fr, string outWorkingDir)
			
		{
		//Set variables:
			block = block__;
			framesPerSecond = fr;
			// workingFolder.ToString();
			this.workingDir = outWorkingDir;

			timer = new System.Timers.Timer();
			timer.Interval = 1000 / framesPerSecond;
			timer.AutoReset = true;
			timer.Elapsed += Timer_Elapsed;

			//timer.SynchronizingObject =// new System.Windows.Forms.Form();
			//resetWorkspace();
		}

		public Doze(Rectangle block__, int frameRate__) : this(
			block__,
			 frameRate__,
			"screenRecorder"
		)
		{


		}

		


		//Clean up program on crash:
		public void dropFrames()
		{
			if (Directory.Exists(folder4frames))
			{
				nilnul.fs.folder.drop_._RecyclableX.Del( folder4frames);
			}
		}



		//Record video:
		public void recordFrame()
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
				string frameAddressS = Path.Combine( folder4frames , "frame" + fileCount + ".png" );
				if (_saving)
				{
					return;
				}
				address4frameS.Add(frameAddressS);
				bitmap.Save(frameAddressS, ImageFormat.Png);
				fileCount++;

				//Dispose of bitmap:
				bitmap.Dispose();
			}
		}

		bool _saving=false;
		//Save video file:
		private void save()
		{
			_saving = true;
			
			using (VideoFileWriter vFWriter = new VideoFileWriter())
			{
				//Create new video file:
				vFWriter.Open(
					animeAddress
					, block.Width
					, block.Height,
					//new Accord.Math.Rational(
					address4frameS.Count*1000 / miliSecondsElapsed
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


		public void start() {

			resetWorkspace();

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

		

			miliSecondsElapsed = stopwatch.ElapsedMilliseconds;
			//Save video:
			save();


			//Delete the screenshots and temporary folder:
			dropFrames();

			
		}
	}
}