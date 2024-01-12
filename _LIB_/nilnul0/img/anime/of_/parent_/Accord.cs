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

namespace nilnul.img.anime.of_.parent_
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
		TimeSpan timeSpan;


		/// <summary>
		/// the docs
		/// </summary>




		public Accord( string framesContainer___, TimeSpan timeSpan__, string outputFile, Rectangle block__)
		{

			this.framesParentFolder = framesContainer___;
			this.animeAddress = outputFile;
			this.timeSpan = timeSpan__;
			this.block = block__;
		}

		static string _This(string framesContainer___) {
			var animeParent=nilnul.fs.folder.denote_.vered_.next_.subIfNeed._CreateFolderX.Folder_ofAddress(framesContainer___, "animed");
			return new nilnul.fs.address_.spear_.ParentDoc(animeParent, "out.mp4").ToString();

		}

		public Accord( string framesContainer___, TimeSpan timeSpan__,  Rectangle block__)
			:this(framesContainer___,timeSpan__, _This(framesContainer___), block__)
		{
		
			
		}


		/// <summary>
		/// drop frames
		/// </summary>
		public void dropFrames()
		{
			if (Directory.Exists(framesParentFolder))
			{
				nilnul.fs.folder.drop_._RecyclableX.Del(framesParentFolder);
			}
		}



		//Save video file:
		public void save()
		{

			var miliSecondsElapsed =(long) timeSpan.TotalMilliseconds;//.ElapsedMilliseconds;

			var address4frameS = nilnul.fs.folder._DocsX.Spears_ofAddress(framesParentFolder).OrderBy(
				x=> nilnul.txt_.vered._VerX.Num(
					nilnul.fs._address.doc_.exted._MainX.Main( x.sprig.document.doc).ToString()
				)
				,
				nilnul.num.Comparer2.Singleton
			).Select(
				s=>s.ToString()
			);


				using (VideoFileWriter vFWriter = new VideoFileWriter())
				{
					//Create new video file:
					vFWriter.Open(
						animeAddress
						, block.Width
						, block.Height,
						//new Accord.Math.Rational(
						address4frameS.Count() * 1000 / miliSecondsElapsed
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
}