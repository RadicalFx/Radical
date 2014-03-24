using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Xml.Schema;
using System.ServiceModel;
using System.ServiceModel.Activation;
using WsdlDescription = System.Web.Services.Description.ServiceDescription;
using System.ServiceModel.Configuration;

namespace Topics.Radical.ServiceModel.Behaviors
{
	/// <summary>
	/// Defines a behavior that can produce a wsdl document
	/// compatible with the Soap 1.1 specification, Soap 1.1
	/// specs does not includes "wsdl:import" directive.
	/// </summary>
	public class InlineWsdlEndpointBehavior : IWsdlExportExtension, IEndpointBehavior
	{
		private void AddImportedSchemas( XmlSchema schema, XmlSchemaSet schemaSet, List<XmlSchema> importList )
		{
			foreach( XmlSchemaImport import in schema.Includes )
			{
				var realSchemas = schemaSet.Schemas( import.Namespace );
				foreach( XmlSchema item in realSchemas )
				{
					if( !importList.Contains( item ) )
					{
						importList.Add( item );
						this.AddImportedSchemas( item, schemaSet, importList );
					}
				}
			}
		}

		private void RemoveXsdImports( XmlSchema schema )
		{
			for( int i = 0; i < schema.Includes.Count; i++ )
			{
				if( schema.Includes[ i ] is XmlSchemaImport )
				{
					schema.Includes.RemoveAt( i-- );
				}
			}
		}

		/// <summary>
		/// Writes custom Web Services Description Language (WSDL) elements into the generated WSDL for a contract.
		/// </summary>
		/// <param name="exporter">The <see cref="T:System.ServiceModel.Description.WsdlExporter"/> that exports the contract information.</param>
		/// <param name="context">Provides mappings from exported WSDL elements to the contract description.</param>
		public void ExportContract( WsdlExporter exporter, WsdlContractConversionContext context )
		{

		}

		/// <summary>
		/// Writes custom Web Services Description Language (WSDL) elements into the generated WSDL for an endpoint.
		/// </summary>
		/// <param name="exporter">The <see cref="T:System.ServiceModel.Description.WsdlExporter"/> that exports the endpoint information.</param>
		/// <param name="context">Provides mappings from exported WSDL elements to the endpoint description.</param>
		public void ExportEndpoint( WsdlExporter exporter, WsdlEndpointConversionContext context )
		{
			var schemaSet = exporter.GeneratedXmlSchemas;

			foreach( WsdlDescription wsdl in exporter.GeneratedWsdlDocuments )
			{
				var importList = new List<XmlSchema>();

				foreach( XmlSchema schema in wsdl.Types.Schemas )
				{
					this.AddImportedSchemas( schema, schemaSet, importList );
				}

				wsdl.Types.Schemas.Clear();

				foreach( var schema in importList )
				{
					this.RemoveXsdImports( schema );
					wsdl.Types.Schemas.Add( schema );
				}
			}
		}

		/// <summary>
		/// Implement to pass data at runtime to bindings to support custom behavior.
		/// </summary>
		/// <param name="endpoint">The endpoint to modify.</param>
		/// <param name="bindingParameters">The objects that binding elements require to support the behavior.</param>
		public void AddBindingParameters( ServiceEndpoint endpoint, BindingParameterCollection bindingParameters )
		{
		}

		/// <summary>
		/// Implements a modification or extension of the client across an endpoint.
		/// </summary>
		/// <param name="endpoint">The endpoint that is to be customized.</param>
		/// <param name="clientRuntime">The client runtime to be customized.</param>
		public void ApplyClientBehavior( ServiceEndpoint endpoint, ClientRuntime clientRuntime )
		{
		}

		/// <summary>
		/// Applies the dispatch behavior.
		/// </summary>
		/// <param name="endpoint">The endpoint.</param>
		/// <param name="dispatcher">The dispatcher.</param>
		public void ApplyDispatchBehavior( ServiceEndpoint endpoint, EndpointDispatcher dispatcher )
		{
		}

		/// <summary>
		/// Implement to confirm that the endpoint meets some intended criteria.
		/// </summary>
		/// <param name="endpoint">The endpoint to validate.</param>
		public void Validate( ServiceEndpoint endpoint )
		{
		}
	}
}