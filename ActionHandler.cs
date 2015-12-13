using System;
using System.Web;
using System.Configuration;
using System.Diagnostics;
using System.Xml;
using System.Text.RegularExpressions;
using System.Text;
using System.Reflection;
using System.IO;
using System.Runtime.Remoting;

namespace nstruts
{
	/// <summary>
	/// Summary description for ActionHandler.
	/// 
	/// Implements System.Web.SessionState.IRequiresSessionState is necessary for
	/// propagate the session in the forwarded context
	///  
	/// </summary>
	public class ActionHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
	{
		private const string SECTION_NAME = "actionHandler.config";

		private XmlDocument configDom = null ;

		protected ActionHandlerConfigurator configurator;

		protected Forwards globalForwards;

		public ActionHandler()
		{
            configurator = (ActionHandlerConfigurator)System.Web.Configuration.WebConfigurationManager.GetSection(SECTION_NAME);

			//configurator = (ActionHandlerConfigurator)ConfigurationSettings.GetConfig( SECTION_NAME );

		}

		/// <summary>
		/// 
		/// </summary>
		private void LoadConfiguration( HttpContext context ) 
		{
			if( configDom==null ) 
			{
				StringWriter msg = new StringWriter();

				try 
				{
					configDom = new XmlDocument();
					string filePath = context.Server.MapPath(configurator.File); 
					configDom.Load( filePath );


					/////////////////////////
					// LOAD GLOBAL FORWARDS
					/////////////////////////
					
					XmlNode globalForwardsNode = 
						configDom.DocumentElement.SelectSingleNode("global-forwards");

					this.globalForwards = new Forwards( globalForwardsNode.ChildNodes );
				}
				catch( Exception ex ) 
				{
					msg.Write( "Load configuration error!" );
					context.Trace.Write( "", msg.ToString(), ex );
					throw new Exception( msg.ToString(), ex );
				}

			}

		}


		/// <summary>
		/// 
		/// </summary>
		private void LoadAction( HttpContext context ) 
		{
			StringWriter msg = new StringWriter();

			Regex regex = new Regex( configurator.Pattern );
		
			Match m = regex.Match( context.Request.Path );


			if( !m.Success ) 
			{
				msg.Write( "Action name not valid!" );
				context.Trace.Write( msg.ToString() );
				throw new Exception( msg.ToString() );
			}
			
			string actionName = m.Groups[1].Value;

			StringWriter xpathQry = new StringWriter();
			xpathQry.Write( "action-mappings/action[@path='{0}']", actionName );

			XmlNode actionNode = configDom.DocumentElement.SelectSingleNode( xpathQry.ToString() );
			
			if( actionNode==null ) 
			{
				msg.Write( "Action {0} not found!", actionName );
				context.Trace.Write( msg.ToString() );
				throw new Exception( msg.ToString() );
			}

			string type = actionNode.Attributes["type"].Value;

			string[] typeProps = type.Split(',');
			
			if( typeProps.Length !=2 ) 
			{
				msg.Write( "invalid type attribute of action {0} format class,assembly", actionName );
				context.Trace.Write( msg.ToString() );
				throw new Exception( msg.ToString() );
			}

			Object instance ;

			try 
			{
				ObjectHandle handle = Activator.CreateInstance( typeProps[1], typeProps[0] ); 
				instance = handle.Unwrap();
			}
			catch( Exception ex ) 
			{
				msg.Write( "Create instance of assembly <b>{0}</b> and type <b>{1}</b> failed!", typeProps[1], typeProps[0] );
				context.Trace.Write( "", msg.ToString(), ex );
				throw new Exception( msg.ToString(), ex );
			}
			

			IAction actionObject = null;
			
			if( instance is IAction) 
			{
				actionObject = (IAction)instance;
			}
			else 
			{
				msg.Write( "Action {0} not implements IAction interface!", actionName );
				context.Trace.Write( msg.ToString() );
				throw new Exception( msg.ToString() );
			}


			Forward result;

			try 
			{
				ActionContext actionContext = new ActionContext( globalForwards, actionNode);

				result = actionObject.Execute( actionContext, context );


			}
			catch( Exception ex ) 
			{
				msg.Write( "Excecute action {0} error!", actionName );
				context.Trace.Write( "", msg.ToString(), ex );
				throw new Exception( msg.ToString(), ex );
			}

			if( result!=null ) 
			{
				StringBuilder path = new StringBuilder();
					
				if( result.Path.StartsWith("/") ) 
				{
					path.Append( context.Request.ApplicationPath );
				}

				path.Append( result.Path );

				if( result.Redirect ) 
				{
					context.Response.Redirect( path.ToString(), true /* endResponse */ );
				}
				else 
				{
					context.Server.Execute( path.ToString() );
					//context.Server.Transfer( path.ToString() );
				}
			}
			
		}

		/// <summary>
		/// 
		/// </summary>
		public void ProcessRequest( HttpContext context ) 
		{
			try 
			{

				LoadConfiguration( context );			
				LoadAction( context );
			
			} 
			catch( Exception ex ) 
			{
				ManageException( context, ex );
			}
		}

		public bool IsReusable
		{
			get { return true; }
		}

		public static void ManageException(HttpContext context, Exception ex)
		{
			context.Trace.Write( "error", "ProcessRequest error", ex );
			
			HttpResponse r = context.Response;

			r.Write( "<html>" );
			r.Write( "<body>" );
			r.Write( "<p><font color='red'>NSTRUTS HANDLER ERROR</font></p><hr>" );
			r.Write( ex.Message );
			r.Write( "<hr>" );
			if( ex.InnerException!=null ) 
			{
				r.Write( "Inner Exception <br>" );
				r.Write( ex.InnerException.Message );
			}
			r.Write( "<hr>" );
			r.Write( "<a href='trace.axd' target='trace'>view trace</a>" );
			r.Write( "</body>" );
			r.Write( "</html>" );
		}
	}

}
