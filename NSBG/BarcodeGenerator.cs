using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Nancy;
using BarcodeLib;

namespace NancyServer

{
	public class GenerateCode : NancyModule
	{
		public Image GenerateIt(string bdata, TYPE btype, int bwight, int bheight)
		{
			BarcodeLib.Barcode imgcode = new BarcodeLib.Barcode(bdata, btype);
			imgcode.Width = bwight;
			imgcode.Height = bheight;
			Image img = imgcode.Encode(btype, bdata);
			return img;
		}
		public GenerateCode()
		{
			Get["/gen"] = x =>
			{
				var bdata = this.Request.Query["data"];
				var btype = this.Request.Query["type"];
				var bwight = this.Request.Query["wight"];
				var bheight = this.Request.Query["height"];
				var parsedType = (TYPE) Enum.Parse(typeof(TYPE), btype);
                Image b = GenerateIt(bdata, parsedType, bwight, bheight);
				var ms = new MemoryStream();
				b.Save(ms, ImageFormat.Png);
				ms.Position = 0;
				string ContentType = "image/png";
				return Response.FromStream(ms, ContentType);
			};
		}
	}
}