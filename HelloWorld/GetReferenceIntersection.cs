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
    class GetReferenceIntersection : IExternalCommand
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

                    //Ray
                    XYZ ray = new XYZ(0, 0, 1);

                    // Project Ray
                    LocationPoint locPoint = element.Location as LocationPoint;
                    XYZ projectRay = locPoint.Point;

                    ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Roofs);
                    ReferenceIntersector refInter = new ReferenceIntersector(filter, FindReferenceTarget.Face, (View3D)doc.ActiveView);

                    ReferenceWithContext refContext = refInter.FindNearest(projectRay, ray);
                    Reference reference = refContext.GetReference();

                    XYZ intPoint = reference.GlobalPoint;

                    double distance = projectRay.DistanceTo(intPoint);

                    TaskDialog.Show("Intersection", string.Format("Điểm cắt từ cột và mái là {0}\n" +
                        "Khoảng cách giữa Location Point của cột và điểm giao là {1}",
                        intPoint, distance));
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
