TechEdWebApiDemo
================

Sample code used in TechEd NZ 2012 WebApi Session

DEV313 - WebApi - Know It, Learn It, Love It
Code Sample - TechEd NZ - 2012
Author: Grant Archibald

Licence see Licence.txt

NOTE: This solution uses NuGet package restore
(see http://docs.nuget.org/docs/workflows/using-nuget-without-committing-packages)

Steps Involved:
1. Create Model Members.cs (Code First Entity Framework Data Model)

2. Create Folder DAL

3. Create TechEdContext.cs (Optionally set connection string in contructor)

4. Create TechEdInit.cs
 
5. Register Database.SetInitializer in Global.asax.cs

6. Compile Solution to build Model

7. Right Click - Add controller MembersController.cs

   a) Choose Models.Members and TechExContext

   NOTE: This default controller will be updated to add
	i) OData [Queryable]
	ii) AutoMapper to limit data sent to/from the consumers

Extras
------

Help Pages
1. Install-Package Microsoft.AspNet.WebApi.HelpPage -pre
2. Open http://localhost:XXXX/help 
3. See http://bit.ly/webapi-help for Video Demo

OData Support
1. Install-Package Microsoft.AspNet.WebApi.OData -pre
2. Register OData routes in WebApiConfig.cs in App_Start 
3. Add [Queryable] to GetMembers in MembersController
4. See http://bit.ly/webapi-odata for more information

Security
1. Install-Package Thinktecture.IdentityModel
2. Add SecurityException Filter e.g. SecurityResources.cs in App_Start
3. Add SecurityConfig.cs in AppStart
4. Register Security in Global.asax.cs e.g. SecurityConfig.ConfigureGlobal 
5. See http://bit.ly/webapi-security on how to secure WebApi

	NOTES:
		The default security config accepts username == password
		You will need to change to another method e.g. MembershipProvider

Extended Members Controller
1. Install-Package AutoMapper
2. Add extra project with ServiceModels
   (see TechEd.Integration)
3. Reference TechEd.Integration
4. Register AutoMapper to/from domain model e.g.

	AutoMapper.Mapper.CreateMap<Models.Member, TechEd.Integration.ServiceModels.Member>();
    AutoMapper.Mapper.CreateMap<TechEd.Integration.ServiceModels.Member, Models.Member>();

5. Change MembersController.cs to reference TechEd.Integration.ServiceModels
6. Change MembersController to map to/from domain model
7. Change Queryable to use ODataQueryOptions
8. Make use of using OData.Framework; defined in ODataExtensions.cs for extra parsing of OData queries
	