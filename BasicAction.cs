using System;
using System.Web;
using nstruts;

namespace nstruts.actions
{
	/// <summary>
	/// 
	/// </summary>
	public class BasicAction : nstruts.IAction
	{
		public BasicAction()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public Forward Execute( ActionContext actionContext, HttpContext context ) 
		{
			string forwardName = context.Request.Params["forward"];

			Forward result = null;

			if( forwardName!=null ) 
			{
				result = actionContext.Forwards[forwardName];
			}

			return result;
		}
	}
}
