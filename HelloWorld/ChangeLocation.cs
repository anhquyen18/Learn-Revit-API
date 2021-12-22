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
    class ChangeLocation : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Get UIDocument
            UIDocument UIDoc = commandData.Application.ActiveUIDocument;

            // Get Document
            Document doc = UIDoc.Document;

            try
            {
                // Get Reference
                Reference r = UIDoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);

                // Get Element
                ElementId elementId = r.ElementId;
                Element element = doc.GetElement(elementId);


                if (r != null)
                {

                    using (Transaction trans = new Transaction(doc, "Set para"))
                    {
                        trans.Start();

                        LocationPoint loc = element.Location as LocationPoint;

                        XYZ curPoint = loc.Point;
                        XYZ newPoint = new XYZ(curPoint.X+4, curPoint.Y, curPoint.Z);

                        loc.Point = newPoint;

                        trans.Commit();
                    }


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
