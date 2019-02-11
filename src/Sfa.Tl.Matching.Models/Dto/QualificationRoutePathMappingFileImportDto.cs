using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class QualificationRoutePathMappingFileImportDto : FileImportDto
    {
        public override Stream FileDataStream { get; set; }
        public override int? NumberOfHeaderRows => 3;

        [Column(Order = 0)] public string LarsId { get; set; }
        [Column(Order = 1)] public string Title { get; set; }
        [Column(Order = 2)] public string ShortTitle { get; set; }
        [Column(Order = 3)] public string AnimalCareandManagement { get; set; }
        [Column(Order = 4)] public string AgricultureLandManagementandProduction { get; set; }
        [Column(Order = 5)] public string ManagementandAdministration { get; set; }
        [Column(Order = 6)] public string HumanResources { get; set; }
        [Column(Order = 7)] public string CareServices { get; set; }
        [Column(Order = 8)] public string Hospitality { get; set; }
        [Column(Order = 9)] public string Catering { get; set; }
        [Column(Order = 10)] public string DesignSurveyingandPlanning { get; set; }
        [Column(Order = 11)] public string OnSiteConstruction { get; set; }
        [Column(Order = 12)] public string BuildingServicesEngineering { get; set; }
        [Column(Order = 13)] public string CraftandDesign { get; set; }
        [Column(Order = 14)] public string MediaBroadcastandProduction { get; set; }
        [Column(Order = 15)] public string CulturalHeritageandVisitorAttractions { get; set; }
        [Column(Order = 16)] public string DigitalSupportandServices { get; set; }
        [Column(Order = 17)] public string DigitalProductionDevelopmentandDesign { get; set; }
        [Column(Order = 18)] public string DevelopmentandDigitalBusinessServices { get; set; }
        [Column(Order = 19)] public string Education { get; set; }
        [Column(Order = 20)] public string EngineeringDesignDevelopmentandControl { get; set; }
        [Column(Order = 21)] public string ManufacturingandProcess { get; set; }
        [Column(Order = 22)] public string MaintenanceInstallationandRepair { get; set; }
        [Column(Order = 23)] public string HairBeautyandAesthetics { get; set; }
        [Column(Order = 24)] public string Health { get; set; }
        [Column(Order = 25)] public string HealthcareScience { get; set; }
        [Column(Order = 26)] public string Science { get; set; }
        [Column(Order = 27)] public string CommunityExerciseFitnessandHealth { get; set; }
        [Column(Order = 28)] public string Legal { get; set; }
        [Column(Order = 29)] public string Financial { get; set; }
        [Column(Order = 30)] public string Accounting { get; set; }
        [Column(Order = 31)] public string ProtectiveServices { get; set; }
        [Column(Order = 32)] public string CustomerService { get; set; }
        [Column(Order = 33)] public string Marketing { get; set; }
        [Column(Order = 34)] public string Procurement { get; set; }
        [Column(Order = 35)] public string Retail { get; set; }
        [Column(Order = 36)] public string Transport { get; set; }
        [Column(Order = 37)] public string Logistics { get; set; }
        [Column(Order = 38)] public string Source { get; set; }
    }
}