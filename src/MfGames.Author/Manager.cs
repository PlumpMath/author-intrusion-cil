#region Namespaces

using System;

using Castle.Windsor;

#endregion

namespace MfGames.Author
{
	public class Manager : IDisposable
	{
		#region Constructors
		
		public Manager ()
		{
			// Set up Windsor container along with the extensions.
			windsor = new WindsorContainer();
		}
		
		#endregion
		
		#region Destructors
		
		public void Dispose ()
		{
			if (windsor != null)
			{
				windsor.Dispose ();
				windsor = null;
			}
		}
		
		#endregion
		
		#region IOC
		
		private WindsorContainer windsor;
		
		#endregion
	}
}
