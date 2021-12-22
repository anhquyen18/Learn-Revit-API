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
    class CreateNewFloor : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Get UIDocument
            UIDocument UIDoc = commandData.Application.ActiveUIDocument;

            // Get Document
            Document doc = UIDoc.Document;

            // Creat a Point
            XYZ p1 = new XYZ(-10, -10, 0);
            XYZ p2 = new XYZ(10, -10, 0);
            XYZ p3 = new XYZ(50, 0, 0);
            XYZ p4 = new XYZ(10, 10, 0);
            XYZ p5 = new XYZ(-10, 10, 0);

            // Create a Line
            Line cur1 = Line.CreateBound(p1, p2);
            Arc cur2 = Arc.Create(p2, p4, p3);
            Line cur3 = Line.CreateBound(p4, p5);
            Line cur4 = Line.CreateBound(p5, p1);

            // Create Curve array
            CurveArray cArray = new CurveArray();
            cArray.Append(cur1);
            cArray.Append(cur2);
            cArray.Append(cur3);
            cArray.Append(cur4);

            try
            {
                using (Transaction trans = new Transaction(doc, "Place Wall"))
                {
                    trans.Start();

                    doc.Create.NewFloor(cArray, true);

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
