using System;
using System.Collections;
using System.Xml;

namespace nstruts
{
	/// <summary>
	/// class that contains the parameters read in the xml config
	/// </summary>
	public class Parameters
	{
		Hashtable parameters = null;
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="paramList"></param>
		public Parameters( XmlNodeList paramList )
		{
			if( paramList==null) 
			{
				return;
			}

			parameters = new Hashtable();
			
			foreach( XmlNode n in paramList ) 
			{
				parameters.Add( 
					n.Attributes["name"].Value, 
					n.Attributes["value"].Value );
			}
		}

		/// <summary>
		/// 
		/// 
		/// </summary>
		public string this[ string name ] 
		{
			get { return (parameters!=null) ? parameters[name] as string : null ; }
		}

	}
}
