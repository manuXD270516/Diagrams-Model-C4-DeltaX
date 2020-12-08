using Structurizr;
using Structurizr.Api;
using Structurizr.Documentation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Diagrams_Model_C4_DeltaX
{
    class Program
    {

        // Implementacion temporal (En un solo archivo)
        private const long WORKSPACE_ID = 54399;
        private const string API_KEY = "26d5dc78-e852-4644-a6df-8ab014c2239a";
        private const string API_SECRET = "3700e29e-0f25-41be-bde3-45fb4458d1ac";

        // TAGS
        private const string EXISTS_SYSTEM_TAG = "existsSystem";
        private const string MOBILE_APP_TAG = "mobileApp";
        private const string SINGLE_PAGE_APPLICATION_TAG = "spa";
        private const string WEB_APPLICATION_TAG = "webApplication"; // analizar el uso
        private const string DATABASE_TAG = "database"; // analizar el uso



        static void Main(string[] args)
        {

            // Crear el nuevo espacio de trabajo
            Workspace workspace = new Workspace("Arquitectura de Software - Plataforma de Gestion y Monitoreo de Transporte de Carga", "Diseño de la arquitectura de software de proyectos de transformacion digital");
            Model model = workspace.Model;

            model.Enterprise = new Enterprise("Transport Manangement System");
            // Agregar los elementos que contendra el sistema
            
            Person userCompany = model.AddPerson(Location.External, "Empresa", "Empresa que adquiere los servicios de la plataforma de gestion y seguimiento de transporte de carga");
            Person userDriver = model.AddPerson(Location.External, "Chofer", "Persona encargada que transporta la carga de los clientes administrando el flujo de la operacion");
            Person userOperator = model.AddPerson(Location.External, "Operador", "Persona encargada de adminstrar y monitorear las operaciones");
            Person userClient = model.AddPerson(Location.External, "Cliente", "Persona interesada en realizar el seguimiento de su carga");
            //

            SoftwareSystem tmsSoftwareSystem = model.AddSoftwareSystem(Location.Internal, "TMS Generico", "Plataforma de Gestion y Seguimiento del transporte y logistica de Carga a Nivel Nacional");

            SoftwareSystem odooErpSoftwareSystem = model.AddSoftwareSystem(Location.Internal, "Odoo Sh", "ERP: Sistema de Planificacion de Recursos Empresariales");
            SoftwareSystem mailServerSoftwareSystem = model.AddSoftwareSystem(Location.Internal, "Servidor de Correos", "Servidor de Correos Interno Microsoft Exchange");
            SoftwareSystem fcmSoftwareSystem = model.AddSoftwareSystem(Location.Internal, "Firebase Cloud Messaging ", "Sistema de mensajeria en tiempo real multiplaforma ofrecido por Google");
            SoftwareSystem azureBlobStorageSoftwareSystem = model.AddSoftwareSystem(Location.Internal, "Azure Blob Storage", "Servidor de Almacenamiento de archivos en la nube de Microsft Azure");
            SoftwareSystem googlMapsPlatoformSoftwareSystem = model.AddSoftwareSystem(Location.Internal, "Google Maps Plataform", "Plataforma web que ofrece un conjunto de APIs (Aplication Interface Programming) para la gestion de mapas, rutas, direcciones, etc. Soportado por Google");
            odooErpSoftwareSystem.AddTags(EXISTS_SYSTEM_TAG);
            mailServerSoftwareSystem.AddTags(EXISTS_SYSTEM_TAG);
            fcmSoftwareSystem.AddTags(EXISTS_SYSTEM_TAG);
            azureBlobStorageSoftwareSystem.AddTags(EXISTS_SYSTEM_TAG);
            googlMapsPlatoformSoftwareSystem.AddTags(EXISTS_SYSTEM_TAG);

            tmsSoftwareSystem.Uses(odooErpSoftwareSystem, "Integra la informacion contable de las operaciones de carga");
            tmsSoftwareSystem.Uses(fcmSoftwareSystem, "Administra notificaciones y mensajes en tiempo real");
            tmsSoftwareSystem.Uses(azureBlobStorageSoftwareSystem, "Administra los archivos generados por el sistema");
            tmsSoftwareSystem.Uses(googlMapsPlatoformSoftwareSystem, "Integra los mapas en las aplicaciones");
            tmsSoftwareSystem.Uses(mailServerSoftwareSystem, "Envia correos electronicos usando");

            fcmSoftwareSystem.Delivers(userDriver, "Envia notificaciones push a la aplicación movil");
            mailServerSoftwareSystem.Delivers(userClient, "Envia Correo electronico a");

            userCompany.Uses(tmsSoftwareSystem, "Adquiere los servicios de la plataforma, administra y registra sus propias operaciones, choferes, clientes");
            userDriver.Uses(tmsSoftwareSystem, "Oferta sus servicios y administra el flujo de la operacion");
            userOperator.Uses(tmsSoftwareSystem, "Adminstrar y monitorear las operaciones, oportunidades de carga, operaciones, rutas,  clientes y operadores");
            userClient.Uses(tmsSoftwareSystem, "Administra sus operaciones y cotizaciones de nuevos servicios");


            // Contenendores
            Container mobileApp = tmsSoftwareSystem.AddContainer("Mobile App", "Provee un conjunto de funcionalidades a los choferes como: postulaciones, notificaciones de posibilidades carga,  gestion de operaciones, entre otros", "Flutter");
            mobileApp.AddTags(MOBILE_APP_TAG);

            Container spaOperations = tmsSoftwareSystem.AddContainer("Single-Page Application (Portal Operaciones)", "Provee las funcionalidades de gestion y monitoreo operaciones, oportunidades, rutas de entre otras atraves del navegador web", "Javascript y Angular 9");
            spaOperations.AddTags(SINGLE_PAGE_APPLICATION_TAG);

            Container spaClients = tmsSoftwareSystem.AddContainer("Single-Page Application (Portal Clientes)", "Provee las funcionalidades de seguimiento de operaciones y cotizaciones atraves del navegador web", "Javascript y Angular 9");
            spaClients.AddTags(SINGLE_PAGE_APPLICATION_TAG);

            Container webApplication = tmsSoftwareSystem.AddContainer("Web Application", "Entrega el contenido estático y las Single-Page Application de los Portales de Operaciones y Clientes", "NodeJs y Express Framework");

            Container apiTmsServicesApplication = tmsSoftwareSystem.AddContainer("API TMS Application", "Provee las funcionalidades de gestion de informacion para los portales via JSON/HTTPS API", "NodeJs y Express Framework");

            Container apiAppServicesApplication = tmsSoftwareSystem.AddContainer("API Mobile Application", "Provee las funcionalidades de gestion de informacion para la aplicacion movil via JSON/HTTPS API", "NodeJs y Express Framework");

            Container database = tmsSoftwareSystem.AddContainer("Database", "Almacena y Registra la informacion de la gestion de operaciones y procesos asociados como: Oportunidades, Postulaciones, Autenticacion de Usuarios, etc.", "Microsoft Azure SQL Database");
            database.AddTags(DATABASE_TAG);

            userDriver.Uses(mobileApp, "Visualiza las oportunidades, operaciones, beneficios entre usando", "Flutter");
            userOperator.Uses(webApplication, "Visita globaltms.la usando", "HTTPS");
            userCompany.Uses(webApplication, "Visita globaltms.la usando", "HTTPS");
            userClient.Uses(webApplication, "Visita globaltms.la usando", "HTTPS");

            userCompany.Uses(spaOperations, "Administrar sus propias oportunidades, operaciones, rutas, clientes, choferes, entre otros. Usando", "HTTPS");
            userOperator.Uses(spaOperations, "Visualiza las oportunidades, operaciones, rutas, clientes, choferes, entre otros. Usando", "HTTPS");
            userClient.Uses(spaClients, "Visualiza las operaciones y cotizaciones de nuevos servicios usando", "HTTPS");

            webApplication.Uses(spaClients, "Entrega al navegador web del cliente");
            webApplication.Uses(spaOperations, "Entrega al navegador web del operador");

            spaClients.Uses(googlMapsPlatoformSoftwareSystem, "Integra Mapa de Google en la Aplicacion Web", "HTTPS/XML");
            spaOperations.Uses(googlMapsPlatoformSoftwareSystem, "Integra Mapa de Google en la Aplicacion Web", "HTTPS/XML");
            mobileApp.Uses(googlMapsPlatoformSoftwareSystem, "Integra Mapa de Google en la Aplicacion Movil", "HTTPS/XML");

            apiAppServicesApplication.Uses(database, "Lee y Escribe en", "Tedious MSSQL");
            apiTmsServicesApplication.Uses(database, "Lee y Escribe en", "Tedious MSSQL");

            apiAppServicesApplication.Uses(azureBlobStorageSoftwareSystem, "Administrar archivos en la nube usando", "HTTPS");
            apiTmsServicesApplication.Uses(azureBlobStorageSoftwareSystem, "Administrar archivos en la nube usando", "HTTPS");

            apiTmsServicesApplication.Uses(mailServerSoftwareSystem, "Envia correos electronicos usando", "SMTP");
            apiTmsServicesApplication.Uses(odooErpSoftwareSystem, "Realiza solicitudes a la API", "HTTP/XML-RPC/JSON-RPC");
            apiTmsServicesApplication.Uses(fcmSoftwareSystem, "Emision de mensajes del servidor usando", "HTTPS/JSON");
            fcmSoftwareSystem.Uses(mobileApp, "Realiza difusion de los mensajes proveidos por el servidor usando", "HTTPS/JSON");

            // EVENTUAL (HASTA AGREGAR LOS COMPONENTES), modificar al momento de agregar los componentes
            //spaClients.Uses(apiTmsServicesApplication, "Realiza solicitudes a la API", "HTTPS/JSON");
            //spaOperations.Uses(apiTmsServicesApplication, "Realiza solicitudes a la API", "HTTPS/JSON");
            //mobileApp.Uses(apiAppServicesApplication, "Realiza solicitudes a la API", "HTTPS/JSON");







            // Componentes (API APP SERVICES)
            string expressApiController = "Express API Controller";
            // Controllers
            Component userControllerMobile = apiAppServicesApplication.AddComponent("User Controller", "Permite a los usuarios autenticarse para acceder a la aplicacion movil", "Express API Controller");

            Component carrierControllerMobile = apiAppServicesApplication.AddComponent("Carrier Controller", "Permite gestionar los datos relacionados a su perfil", "Express API Controller");
            Component unitTransportControllerMobile = apiAppServicesApplication.AddComponent("UnitTransport Controller", "Permite gestionar los datos relacionados a su unidad de transporte", "Express API Controller");
            Component loadingOrderControllerMobile = apiAppServicesApplication.AddComponent("LoadingOrder Controller", "Permite gestionar las operaciones vinculadas como tambien el flujo de cada una de ellas", "Express API Controller");

            // Services
            Component servicePassportAuthenticationMobile = apiAppServicesApplication.AddComponent("Module Passport Authentication", "Provee autenticacion segura del lado del servidor", "Node Module");
            Component serviceAzureBlobStorageMobile = apiAppServicesApplication.AddComponent("Service Azure Blob Storage", "Provee los metodos para administrar archivos a la nube de Azure", "Node Module");

            // Business
            Component userBusinessMobile = apiAppServicesApplication.AddComponent("User Business", "Provee funcionalidades de administracion de usuarios", "Javascript Class");
            Component carrierBusinessMobile = apiAppServicesApplication.AddComponent("Carrier Business", "Provee funcionalidades de gestion de informacion de los choferes", "Javascript Class");
            Component unitTransportBusinessMobile = apiAppServicesApplication.AddComponent("UnitTransport Business", "Provee funcionalidades de gestion de informacion de las unidades de transporte", "Javascript Class");
            Component loadingOrderBusinessMobile = apiAppServicesApplication.AddComponent("LoadingOrder Business", "Provee funcionalidades de gestion del flujo de datos de las ordenes de carga", "Javascript Class");

            // Conexion a Base de datos
            Component moduleSequelizeOrmAppServicesMobile = apiAppServicesApplication.AddComponent("Module Sequelize ORM", "Provee una capa de abstraccion para la conexiona a la base de datos", "Node Module");


            // Relations
            apiAppServicesApplication.Components.Where(comp => expressApiController.Equals(comp.Technology))
                .ToList()
                .ForEach(comp => mobileApp.Uses(comp, "Realiza solicitudes a la API", "HTTPS/JSON"));
            userControllerMobile.Uses(servicePassportAuthenticationMobile, "Usa");
            carrierControllerMobile.Uses(serviceAzureBlobStorageMobile, "Usa");
            unitTransportControllerMobile.Uses(serviceAzureBlobStorageMobile, "Usa");
            loadingOrderControllerMobile.Uses(serviceAzureBlobStorageMobile, "Usa");

            serviceAzureBlobStorageMobile.Uses(azureBlobStorageSoftwareSystem, "Usa", "HTTPS");

            userControllerMobile.Uses(userBusinessMobile, "Usa");
            carrierControllerMobile.Uses(carrierBusinessMobile, "Usa");
            unitTransportControllerMobile.Uses(unitTransportBusinessMobile, "Usa");
            loadingOrderControllerMobile.Uses(loadingOrderBusinessMobile, "Usa");

            userBusinessMobile.Uses(moduleSequelizeOrmAppServicesMobile, "Usa");
            carrierBusinessMobile.Uses(moduleSequelizeOrmAppServicesMobile, "Usa");
            unitTransportBusinessMobile.Uses(moduleSequelizeOrmAppServicesMobile, "Usa");
            loadingOrderBusinessMobile.Uses(moduleSequelizeOrmAppServicesMobile, "Usa");

            moduleSequelizeOrmAppServicesMobile.Uses(database, "Lee y Escribe en", "MSSQL Tedious");



            // Componentes (API TMS SERVICES)

            // Controllers
            Component userControllerTms = apiTmsServicesApplication.AddComponent("User Controller", "Permite a los usuarios autenticarse para acceder a los portales web", "Express API Controller");
            Component carrierControllerTms = apiTmsServicesApplication.AddComponent("Carrier Controller", "Permite gestionar los datos relacionados al perfil del chofer", "Express API Controller");
            Component unitTransportControllerTms = apiTmsServicesApplication.AddComponent("UnitTransport Controller", "Permite gestionar los datos relacionados a su unidad de transporte", "Express API Controller");
            Component loadingOrderControllerTms = apiTmsServicesApplication.AddComponent("LoadingOrder Controller", "Permite administrar las ordenes de carga", "Express API Controller");
            Component opportunityControllerTms = apiTmsServicesApplication.AddComponent("Opportunity Controller", "Permite administrar las oportunidades de carga", "Express API Controller");
            Component operationControllerTms = apiTmsServicesApplication.AddComponent("Operation Controller", "Permite administrar las operaciones y su flujo", "Express API Controller");
            Component clientControllerTms = apiTmsServicesApplication.AddComponent("Client Controller", "Permite gestionar los datos de clientes", "Express API Controller");

            // Services
            Component servicePassportAuthenticationTms = apiTmsServicesApplication.AddComponent("Module Passport Authentication", "Provee seguridad en autenticacion de usuario del lado del servidor", "Node Module");
            Component serviceAzureBlobStorageTms = apiTmsServicesApplication.AddComponent("Service Azure Blob Storage", "Provee los metodos para administrar archivos a la nube de Azure", "Node Module");
            Component serviceFirebaseCloudMessagingTms = apiTmsServicesApplication.AddComponent("Service Firebase Cloud Messaging", "Provee funcionalidades para emitir/recepcionar notificaciones en tiempo real", "API Firebase");
            Component serviceOdooApiTms = apiTmsServicesApplication.AddComponent("Service Odoo API", "Provee funcionalidades de integracion con la base de datos de Odoo", "Node Module");
            Component serviceMailerTms = apiTmsServicesApplication.AddComponent("Service NodeMailer", "Provee funcionalidades para enviar correos electronicos", "Node Module");

            // Business
            Component userBusinessTms = apiTmsServicesApplication.AddComponent("User Business", "Provee funcionalidades de administracion de usuarios", "Javascript Class");
            Component carrierBusinessTms = apiTmsServicesApplication.AddComponent("Carrier Business", "Provee funcionalidades de gestion de informacion de los choferes", "Javascript Class");
            Component unitTransportBusinessTms = apiTmsServicesApplication.AddComponent("UnitTransport Business", "Provee funcionalidades de gestion de informacion de las unidades de transporte", "Javascript Class");
            Component opportunityBusinessTms = apiTmsServicesApplication.AddComponent("Opportunity Business", "Provee funcionalidades de gestion del reclutamiento de choferes para las operaciones", "Javascript Class");
            Component operationBusinessTms = apiTmsServicesApplication.AddComponent("Operation Business", "Provee funcionalidades de gestion y seguimiento de operaciones", "Javascript Class");
            Component clientBusinessTms = apiTmsServicesApplication.AddComponent("Client Business", "Provee funcionalidades de gestion de los clientes", "Javascript Class");
            Component loadingOrderBusinessTms = apiTmsServicesApplication.AddComponent("LoadingOrder Business", "Provee funcionalidades de gestion del flujo de datos de las ordenes de carga", "Javascript Class");

            // Conexion a Base de datos
            Component moduleSequelizeOrmAppServicesTms = apiTmsServicesApplication.AddComponent("Module Sequelize ORM", "Provee una capa de abstraccion para la conexiona a la base de datos", "Node Module");


            // Relations
            apiTmsServicesApplication.Components.Where(comp => expressApiController.Equals(comp.Technology))
                .ToList()
                .ForEach(comp => spaOperations.Uses(comp, "Realiza solicitudes a la API", "HTTPS/JSON"));


            List<string> excludeControllersForClients = new List<string>
            {
                "Carrier Controller", "UnitTransport Controller","Opportunity Controller"
            };
            apiTmsServicesApplication.Components.Where(comp => expressApiController.Equals(comp.Technology))
                .ToList()
                .ForEach(comp =>
                {
                    if (!excludeControllersForClients.Contains(comp.Name))
                    {
                        spaClients.Uses(comp, "Realiza solicitudes a la API", "HTTPS/JSON");
                    }
                });

            userControllerTms.Uses(servicePassportAuthenticationTms, "Usa");

            carrierControllerTms.Uses(serviceFirebaseCloudMessagingTms, "Usa");

            unitTransportControllerTms.Uses(serviceFirebaseCloudMessagingTms, "Usa");

            loadingOrderControllerTms.Uses(serviceAzureBlobStorageTms, "Usa");
            loadingOrderControllerTms.Uses(serviceFirebaseCloudMessagingTms, "Usa");
            loadingOrderControllerTms.Uses(serviceOdooApiTms, "Usa");

            operationControllerTms.Uses(serviceFirebaseCloudMessagingTms, "Usa");
            operationControllerTms.Uses(serviceOdooApiTms, "Usa");

            opportunityControllerTms.Uses(serviceFirebaseCloudMessagingTms, "Usa");

            clientControllerTms.Uses(serviceMailerTms, "Usa");


            serviceAzureBlobStorageTms.Uses(azureBlobStorageSoftwareSystem, "Usa", "HTTPS");
            serviceMailerTms.Uses(mailServerSoftwareSystem, "Envia correos electronicos usando", "SMTP");
            serviceOdooApiTms.Uses(odooErpSoftwareSystem, "Usa", "HTTP/XML-RPC/JSON-RPC");
            serviceFirebaseCloudMessagingTms.Uses(fcmSoftwareSystem, "Usa", "HTTPS");
            serviceAzureBlobStorageTms.Uses(azureBlobStorageSoftwareSystem, "Usa", "HTTPS");


            userControllerTms.Uses(userBusinessTms, "Usa");
            carrierControllerTms.Uses(carrierBusinessTms, "Usa");
            unitTransportControllerTms.Uses(unitTransportBusinessTms, "Usa");
            loadingOrderControllerTms.Uses(loadingOrderBusinessTms, "Usa");
            operationControllerTms.Uses(operationBusinessTms, "Usa");
            opportunityControllerTms.Uses(opportunityBusinessTms, "Usa");
            clientControllerTms.Uses(clientBusinessTms, "Usa");


            userBusinessTms.Uses(moduleSequelizeOrmAppServicesTms, "Usa");
            carrierBusinessTms.Uses(moduleSequelizeOrmAppServicesTms, "Usa");
            unitTransportBusinessTms.Uses(moduleSequelizeOrmAppServicesTms, "Usa");
            loadingOrderBusinessTms.Uses(moduleSequelizeOrmAppServicesTms, "Usa");
            opportunityBusinessTms.Uses(moduleSequelizeOrmAppServicesTms, "Usa");
            operationBusinessTms.Uses(moduleSequelizeOrmAppServicesTms, "Usa");
            clientBusinessTms.Uses(moduleSequelizeOrmAppServicesTms, "Usa");

            moduleSequelizeOrmAppServicesTms.Uses(database, "Lee y Escribe en", "MSSQL Tedious");



            model.AddImplicitRelationships();


            // Definir la visualizacion de los diagramas
            ViewSet views = workspace.Views;
            SystemContextView systemContextView = views.CreateSystemContextView(tmsSoftwareSystem, "Contexto de Sistema", "Diagrama de Contexto de Sistema (Nivel 1)");
            systemContextView.EnterpriseBoundaryVisible = false;
            systemContextView.AddNearestNeighbours(tmsSoftwareSystem);
            systemContextView.PaperSize = PaperSize.A5_Landscape;

            /*contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();*/

            ContainerView containerView = views.CreateContainerView(tmsSoftwareSystem, "Contenedores de Sistema", "Diagrama de contenedores de Sistema (Nivel 2)");
            containerView.Add(userOperator);
            containerView.Add(userClient);
            containerView.Add(userCompany);
            containerView.Add(userDriver);
            containerView.AddAllContainers();
            containerView.AddAllSoftwareSystems();
            containerView.Remove(tmsSoftwareSystem);

            // Componentes for API Mobile Application
            ComponentView componentMobileView = views.CreateComponentView(apiAppServicesApplication, "Componentes de Sistema - API Mobile", "Diagrama de Componentes de Sistema (Nivel 3)");
            componentMobileView.Add(mobileApp);
            componentMobileView.Add(azureBlobStorageSoftwareSystem);
            componentMobileView.Add(database);
            componentMobileView.AddAllComponents();
            componentMobileView.PaperSize = PaperSize.A5_Landscape;

            // Componentes for API TMS Application
            ComponentView componentTMSView = views.CreateComponentView(apiTmsServicesApplication, "Componentes de Sistema - API TMS", "Diagrama de Componentes de Sistema (Nivel 3)");
            componentTMSView.Add(spaClients);
            componentTMSView.Add(spaOperations);
            componentTMSView.Add(azureBlobStorageSoftwareSystem);
            componentTMSView.Add(mailServerSoftwareSystem);
            componentTMSView.Add(odooErpSoftwareSystem);
            componentTMSView.Add(fcmSoftwareSystem);
            componentTMSView.Add(database);
            componentTMSView.AddAllComponents();
            componentTMSView.PaperSize = PaperSize.A5_Landscape;

            // Agregar algo de documentacion
            StructurizrDocumentationTemplate template = new StructurizrDocumentationTemplate(workspace);
            template.AddContextSection(tmsSoftwareSystem, Format.Markdown, "Definiendo el contexto del sistema de software\n![](embed:SystemContext)");

            // agregando estilos personalizados
            Styles styles = views.Configuration.Styles;
            styles.Add(new ElementStyle(Tags.SoftwareSystem) { Background = "#1168bd", Color = "#ffffff" });
            styles.Add(new ElementStyle(Tags.Person) { Background = "#08427b", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle(Tags.Container) { Background = "#438dd5", Color = "#ffffff" });
            styles.Add(new ElementStyle(Tags.Component) { Background = "#85bbf0", Color = "#000000" });
            styles.Add(new ElementStyle(EXISTS_SYSTEM_TAG) { Background = "#999999", Color = "#ffffff" });
            styles.Add(new ElementStyle(MOBILE_APP_TAG) { Shape = Shape.MobileDeviceLandscape });
            styles.Add(new ElementStyle(SINGLE_PAGE_APPLICATION_TAG) { Shape = Shape.WebBrowser });
            styles.Add(new ElementStyle(DATABASE_TAG) { Shape = Shape.Cylinder });

            // cargar el documento actual (formato JSON)
            updateWorkspaceToStructurizr(workspace);



            Console.WriteLine("Generated Diagram Sucessfully!");
        }

        private static void updateWorkspaceToStructurizr(Workspace workspace)
        {
            StructurizrClient structurizrClient = new StructurizrClient(API_KEY, API_SECRET);
            structurizrClient.PutWorkspace(WORKSPACE_ID, workspace);
        }
    }
}
