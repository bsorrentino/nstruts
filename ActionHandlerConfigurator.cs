using System;
using System.Web;
using System.Xml;
using System.Configuration;
using System.Reflection;


namespace nstruts
{
	/// <summary>
	/// Summary description for ActionHandlerConfigurator.
	/// </summary>
	public class ActionHandlerConfigurator : IConfigurationSectionHandler
	{
		private string file;

		public ActionHandlerConfigurator() 
		{
		}
		
		public string Pattern 
		{
			//get { return "(/[A-Za-z]\\w*).do"; }
			get { return "(/[A-Za-z]\\w*)Action.(aspx|do)"; }
		}
		public string File 
		{
			get { return file; } 
			//set { file = value; } 
		}
		
		public ActionHandlerConfigurator( XmlNode section )
		{

			file = section.SelectSingleNode("property[@name='file']").Attributes["value"].Value;

	    }
		#region IConfigurationSectionHandler Members

		public object Create(object parent, object configContext, XmlNode section)
		{
			return new ActionHandlerConfigurator(section);
		}

		#endregion

		public override string ToString() 
		{
			return "nstruts configurator";
		}
	}
}
