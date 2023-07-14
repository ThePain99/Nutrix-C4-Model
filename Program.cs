using Structurizr;
using Structurizr.Api;

namespace nutrix_c4_model
{
    class Program
    {
        static void Main(string[] args)
        {
            Generator();
        }

        static void Generator()
        {
            const long workspaceId = 70969;
            const string apiKey = "3ccdea99-3bca-47f7-82a1-02955f9d40ad";
            const string apiSecret = "3fb4d3d3-8490-4804-a33d-1e1479c4d19d";

            StructurizrClient structurizrClient = new StructurizrClient(apiKey, apiSecret);
            Workspace workspace = new Workspace("PRY20231054", "Software de Predicción de Rendimiento Académico");
            Model model = workspace.Model;

            SoftwareSystem predictionSystem = model.AddSoftwareSystem("PRY20231054", "Software de Predicción de Rendimiento Académico");

            Person supervisor = model.AddPerson("Supervisor", "Supervisor registrado en la plataforma");
            Person administrator = model.AddPerson("Administrador", "Administrador encargado de la plataforma");

            supervisor.Uses(predictionSystem, "Usa");
            administrator.Uses(predictionSystem, "Usa");

            ViewSet viewSet = workspace.Views;

            // 1. Diagrama de Contexto
            SystemContextView contextView = viewSet.CreateSystemContextView(predictionSystem, "Contexto", "Diagrama de contexto");
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            // Tags
            predictionSystem.AddTags("SistemaPrediccion");
            supervisor.AddTags("Supervisor");
            administrator.AddTags("Administrator");

            Styles styles = viewSet.Configuration.Styles;
            styles.Add(new ElementStyle("Supervisor") { Background = "#0a60ff", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("Administrator") { Background = "#08427b", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("SistemaPrediccion") { Background = "#008f39", Color = "#ffffff", Shape = Shape.RoundedBox });

            // 2. Diagrama de Contenedores
            Container webApplication = predictionSystem.AddContainer("Web App", "Permite a los supervisores utilizar la plataforma para las predicciones", "Angular Web");
            Container apiApplication = predictionSystem.AddContainer("Api Application", "Permite al web application consumir el servicio para acceder a datos en la BD", "Spring Boot port 8080");
            Container database = predictionSystem.AddContainer("Database", "", "MySQL");

            supervisor.Uses(webApplication, "Consulta");
            administrator.Uses(webApplication, "Consulta");

            webApplication.Uses(apiApplication, "API Request", "JSON/HTTPS");

            apiApplication.Uses(database, "", "JDBC");

            // Tags
            webApplication.AddTags("WebApp");
            apiApplication.AddTags("ApiApplication");
            database.AddTags("Database");


            styles.Add(new ElementStyle("WebApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            styles.Add(new ElementStyle("ApiApplication") { Shape = Shape.RoundedBox, Background = "#0000ff", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle("Database") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });

            ContainerView containerView = viewSet.CreateContainerView(predictionSystem, "Contenedor", "Diagrama de contenedores");
            containerView.AddAllElements();

            // 3. Diagrama de componentes patient

            ComponentView componentView = viewSet.CreateComponentView(apiApplication, "Components Prediction", "Component Diagram");

            Component AdministratorController = apiApplication.AddComponent("Administrator Controller", "REST API endpoints de Administrator", "Spring Boot REST Controller");
            Component predictionController = apiApplication.AddComponent("Prediction Controller", "REST API endpoints de predicción", "Spring Boot REST Controller");
            Component supervisorsController = apiApplication.AddComponent("Supervisor Controller", "REST API endpoints de supervisors", "Spring Boot REST Controller");
         
            Component AdministratorRepository = apiApplication.AddComponent("Administrator Repository", "Información de los Administrator", "Spring Component");
            Component predictionRepository = apiApplication.AddComponent("Prediction Repository", "Información de la predicción", "Spring Component");
            Component supervisorsRepository = apiApplication.AddComponent("Supervisor Repository", "Información de Supervisors", "Spring Component");

            // Tags
            AdministratorController.AddTags("AdministratorController");
            predictionController.AddTags("PredictionController");
            supervisorsController.AddTags("SupervisorsController");
     
            AdministratorRepository.AddTags("AdministratorRepository");
            predictionRepository.AddTags("PredictionRepository");
            supervisorsRepository.AddTags("SupervisorsRepository");

            styles.Add(new ElementStyle("AdministratorController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("PredictionController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("SupervisorsController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            styles.Add(new ElementStyle("AdministratorRepository") { Shape = Shape.Component, Background = "#00BFFF", Icon = "" });
            styles.Add(new ElementStyle("PredictionRepository") { Shape = Shape.Component, Background = "#00BFFF", Icon = "" });
            styles.Add(new ElementStyle("SupervisorsRepository") { Shape = Shape.Component, Background = "#00BFFF", Icon = "" });

            webApplication.Uses(AdministratorController, "", "JSON/HTTPS");
            webApplication.Uses(predictionController, "", "JSON/HTTPS");
            webApplication.Uses(supervisorsController, "", "JSON/HTTPS");

            AdministratorController.Uses(AdministratorRepository, "Invoca métodos de Administrator", "");
            predictionController.Uses(predictionRepository, "Invoca métodos de predicción", "");
            supervisorsController.Uses(supervisorsRepository, "Invoca métodos de supervisors", "");

            supervisorsRepository.Uses(database, "", "JDBC");
            predictionRepository.Uses(database, "", "JDBC");
            AdministratorRepository.Uses(database, "", "JDBC");

            componentView.Add(webApplication);
            componentView.Add(database);

            componentView.Add(AdministratorController);
            componentView.Add(predictionController);
            componentView.Add(supervisorsController);
            componentView.Add(supervisorsRepository);
            componentView.Add(predictionRepository);
            componentView.Add(AdministratorRepository);
            
        
            // Configuraciones de la vista
            contextView.PaperSize = PaperSize.A5_Landscape;
            containerView.PaperSize = PaperSize.A5_Landscape;
            componentView.PaperSize = PaperSize.A4_Landscape;
            /*componentViewNutritionist.PaperSize = PaperSize.A4_Landscape;
            componentViewAppointment.PaperSize = PaperSize.A4_Landscape;
            componentViewPublications.PaperSize = PaperSize.A4_Landscape;*/

            // Actualizar Workspace
            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);
        }
    }
}