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
    [TransactionAttribute(TransactionMode.ReadOnly)]
    class GetGeometry : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get UIDocument
            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            //Get Document
            Document doc = uidoc.Document;


            try
            {
                //Get Reference of Element
                Reference r = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);

                if (r != null)
                {
                    //Get Element
                    ElementId elementId = r.ElementId;
                    Element element = doc.GetElement(elementId);

                    //Get Geometry
                    Options opt = new Options();
                    opt.DetailLevel = ViewDetailLevel.Fine;

                    GeometryElement geoElement = element.get_Geometry(opt);

                    foreach (GeometryObject obj in geoElement)
                    {
                        Solid solid = obj as Solid;

                        int faces = 0;
                        double area = 0.0;

                        foreach (Face f in solid.Faces)
                        {
                            area += f.Area;
                            faces++;
                        }

                        TaskDialog.Show("Geometry", string.Format("Wall đã chọn có số mặt là {0}\n" +
                            "Diện tích các mặt là {1}",
                            faces, UnitUtils.ConvertFromInternalUnits(area, DisplayUnitType.DUT_SQUARE_METERS)));
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
