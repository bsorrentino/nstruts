using System;
using System.Xml;

namespace nstruts
{
	/// <summary>
	/// 
	/// </summary>
	public class Forward
	{
		private string name;
		private string path;
		private bool redirect = false;


		/// <summary>
		/// CREATE A UNNAMED AND VOLATILE FORWARD 
		/// </summary>
		/// <param name="path"></param>
		/// <param name="redirect"></param>
		public static Forward CreateForward( string path, bool redirect ) 
		{
			Forward result = new Forward();
			result.path = path;
			result.redirect = redirect;
			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		private Forward() 
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="node"></param>
		internal Forward( XmlNode node )
		{
			this.name = node.Attributes["name"].Value;
			this.path = node.Attributes["path"].Value;
			
			XmlAttribute redirectAttrs = node.Attributes["redirect"];
			if( redirectAttrs!=null ) 
			{
				this.redirect = bool.Parse( redirectAttrs.Value );			
			}

		}

		public string Name 
		{
			get { return name; }
		}

		public string Path 
		{
			get { return path; }
		}
		
		public bool Redirect 
		{
			get { return redirect; }
		}
	}
}
