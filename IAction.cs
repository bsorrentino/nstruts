using System;
using System.Web;

namespace nstruts
{
	/// <summary>
	/// 
	/// </summary>
	public interface IAction
	{

		Forward Execute( ActionContext actionContext, HttpContext context ); 
	}
}
