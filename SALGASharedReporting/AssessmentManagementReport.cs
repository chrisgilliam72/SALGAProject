using SALGADBLib;
using SALGASharedReporting.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SALGASharedReporting
{
    public class AssessmentManagementReport
    {
        public static async Task<CompletenessReportViewModel> GetCompletenessReport(IAssessmentRepository assessmentRepository, IDemographicsRepository demographicsRepository, int auditYear)
        {
            var reportVM = new CompletenessReportViewModel();

            var municipalitiesGrps = (await demographicsRepository.GetMunicipalities()).GroupBy(x=>x.Province).ToList();
            var assessmentTrackings = await assessmentRepository.GetAssessmentTrackings(auditYear);

            foreach (var municipalityGrp in municipalitiesGrps)
            {
                var completeAssessments = assessmentTrackings.Where(x => x.Municipality.Province == municipalityGrp.Key && x.IsSubmitted == true).ToList();
                var incompleteAssessments = assessmentTrackings.Where(x => x.Municipality.Province == municipalityGrp.Key && x.IsSubmitted == false).ToList();

                var provinceRow = new ProvinceCompleteRow()
                {
                    Province = municipalityGrp.Key.Name,
                    NoMunicipalities = municipalityGrp.Count(),
                    CompleteMunicipalities = completeAssessments.Count(),
                    MunicipalityCompleteList = completeAssessments.Select(x=>new ProvincialMunicipalityCompletion { ID=x.Municipality.pkID, Name=x.Municipality.Name, Status= MunicipalityCompletionStatus.Complete}).ToList(),
                    MunicipalityPartiallyCompleList= incompleteAssessments.Select(x=> new ProvincialMunicipalityCompletion { ID=x.Municipality.pkID, Name=x.Municipality.Name, Status=MunicipalityCompletionStatus.Incomplete}).ToList()                   
                };

                foreach (var municipality in municipalityGrp)
                {
                    if (completeAssessments.FirstOrDefault(x => x.Municipality.pkID == municipality.pkID) == null &&
                       incompleteAssessments.FirstOrDefault(x => x.Municipality.pkID == municipality.pkID) == null)
                    {
                        var rowItem = new ProvincialMunicipalityCompletion { ID = municipality.pkID, Name = municipality.Name, Status = MunicipalityCompletionStatus.NotStarted };
                        provinceRow.MunicipalityNotStartedList.Add(rowItem);
                    }
                }
    

                reportVM.ProvinceRows.Add(provinceRow);
            }

            return reportVM;
        }
    }
}
