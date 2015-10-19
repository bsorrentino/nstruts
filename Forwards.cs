using System;
using System.Xml;
using System.Collections;

namespace nstruts
{
	/// <summary>
	/// 
	/// </summary>
	public class Forwards
	{
		Hashtable forwardTable = null;

		internal Forwards( XmlNodeList forwardList )
		{
			forwardTable = new Hashtable();
			
			Forward item;

			foreach( XmlNode n in forwardList ) 
			{
				item = new Forward( n ) ;
				
				forwardTable.Add( item.Name, item );

			}

		}
		internal void Append( Forwards forwards ) 
		{
			foreach( Forward f in forwards.forwardTable.Values )
			{
				forwardTable.Add( f.Name, f );
			}
		}

		public Forward this[ string name ] 
		{
			get { return forwardTable[name] as Forward; }
		}
	}
}
