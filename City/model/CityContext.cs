#pragma warning disable CS1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: Microsoft.Xrm.Sdk.Client.ProxyTypesAssemblyAttribute()]

namespace City.Dataverse
{
	
	
	/// <summary>
	/// Represents a source of entities bound to a Dataverse service. It tracks and manages changes made to the retrieved entities.
	/// </summary>
	public partial class CityContext : Microsoft.Xrm.Sdk.Client.OrganizationServiceContext
	{
		
		/// <summary>
		/// Constructor.
		/// </summary>
		public CityContext(Microsoft.Xrm.Sdk.IOrganizationService service) : 
				base(service)
		{
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="City.Dataverse.Account"/> entities.
		/// </summary>
		public System.Linq.IQueryable<City.Dataverse.Account> AccountSet
		{
			get
			{
				return this.CreateQuery<City.Dataverse.Account>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="City.Dataverse.Contact"/> entities.
		/// </summary>
		public System.Linq.IQueryable<City.Dataverse.Contact> ContactSet
		{
			get
			{
				return this.CreateQuery<City.Dataverse.Contact>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="City.Dataverse.Incident"/> entities.
		/// </summary>
		public System.Linq.IQueryable<City.Dataverse.Incident> IncidentSet
		{
			get
			{
				return this.CreateQuery<City.Dataverse.Incident>();
			}
		}
	}
}
#pragma warning restore CS1591
