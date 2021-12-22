using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace HelloWorld
{
    [TransactionAttribute(TransactionMode.Manual)]
    class CreateViewPlan : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get UIDocument
            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            //Get Document
            Document doc = uidoc.Document;

            //Get Level 1
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            Level level = collector.OfCategory(BuiltInCategory.OST_Levels)
                .WhereElementIsNotElementType().Cast<Level>().First(x => x.Name == ("Level 1"));

            //Get Family
            ViewFamilyType viewFamily = new FilteredElementCollector(doc).
                OfClass(typeof(ViewFamilyType)).
                Cast<ViewFamilyType>().First(x => x.ViewFamily == ViewFamily.FloorPlan);

            try
            {
                using (Transaction trans = new Transaction(doc, "Create View Plan"))
                {
                    trans.Start();

                    ViewPlan viewPlan = ViewPlan.Create(doc, viewFamily.Id, level.Id);
                    viewPlan.Name = "My floor form API";

                    trans.Commit();
                }
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }

        }
    }
}
