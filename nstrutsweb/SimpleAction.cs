using System;
using System.Web;
using nstruts;

namespace nstruts.web
{
	/// <summary>
	/// 
	/// </summary>
	public class SimpleAction : nstruts.IAction
	{
		public SimpleAction()
		{
			// 
			// TODO: Add constructor logic here
			//
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ctx"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public Forward Execute( ActionContext ctx, HttpContext context )
		{
			return ctx.Forwards["success"];

		}
	}
}
