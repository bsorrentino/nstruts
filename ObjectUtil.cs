using System;
using System.Web;
using System.Reflection;

namespace nstruts
{
	/// <summary>
	/// Class utility for manage generic object
	/// </summary>
	public class ObjectUtil
	{
		protected ObjectUtil()
		{
		}

		public static void SetObjectFromRequest(object instance, HttpRequest request)
		{
			Type type = instance.GetType();

		}

		
	}
}
