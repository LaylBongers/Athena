using System;

namespace Athena
{
	/// <summary>
	///     Represents information to reference to a service within serialized data.
	/// </summary>
	public class ServiceReference
	{
		/// <summary>
		///     Gets or sets the name of the service being referenced. For human readability of files only.
		/// </summary>
		public string FriendlyName { get; set; }

		/// <summary>
		///     Gets or sets the guid of the service being referenced. Used for lookup.
		/// </summary>
		public Guid Guid { get; set; }
	}
}