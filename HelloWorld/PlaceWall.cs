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
    class PlaceWall : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Get UIDocument
            UIDocument UIDoc = commandData.Application.ActiveUIDocument;

            // Get Document
            Document doc = UIDoc.Document;

            //Find Family
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            Level level = collector.OfCategory(BuiltInCategory.OST_Levels)
                .WhereElementIsNotElementType().Cast<Level>().First(x => x.Name == ("Level 1"));

            // Creat a Point
            XYZ p1 = new XYZ(0,0,0);
            XYZ p2 = new XYZ(50,0,0);

            // Create a Line
            Line line = Line.CreateBound(p1, p2);


            try
            {
                using (Transaction trans = new Transaction(doc, "Place Wall"))
                {
                    trans.Start();

                    Wall.Create(doc, line, level.Id, false);

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
