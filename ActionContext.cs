using System;
using System.Collections;
using System.Xml;

namespace nstruts
{
	/// <summary>
	/// 
	/// </summary>
	public class ActionContext
	{
		Forwards forwards ;
		Parameters parameters;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="globalForwards"></param>
		/// <param name="actionNode"></param>
		internal ActionContext( Forwards globalForwards, XmlNode actionNode )
		{
			XmlElement elem = actionNode as XmlElement;

			//////////////////////////
			// LOAD INIT PARAMETERS
			//////////////////////////
			XmlNodeList paramList = elem.SelectNodes( "init-params/param");

			parameters = new Parameters( paramList );

			//////////////////////////
			// LOAD FORWARDS
			//////////////////////////
			XmlNodeList forwardList = elem.SelectNodes( "forward");

			//forwards = new Forwards( actionNode.ChildNodes );
			forwards = new Forwards( forwardList );
			forwards.Append( globalForwards );
		}

		/// <summary>
		/// 
		/// </summary>
		public Forwards Forwards 
		{
			get { return forwards; }
		}

		/// <summary>
		/// 
		/// </summary>
		public Parameters Parameters
		{
			get { return parameters; }
		}

	}
}
