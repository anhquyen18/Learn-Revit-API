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
    class SetPara : IExternalCommand
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
                    Parameter para = element.get_Parameter(BuiltInParameter.INSTANCE_HEAD_HEIGHT_PARAM);

                    using(Transaction trans = new Transaction(doc, "Set para"))
                    {
                        trans.Start();

                        para.Set(6.5);

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
