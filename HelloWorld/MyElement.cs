using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;


namespace HelloWorld
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class MyElement : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //TaskDialog.Show("Anh Quyền đẹp trai vcl", "Hello World");
            //return Autodesk.Revit.UI.Result.Succeeded;

            // Get UIDocument
            UIDocument UIDoc = commandData.Application.ActiveUIDocument;

            // Get Document
            Document doc = UIDoc.Document;

            // 

            try
            {
                // Get Reference
                Reference r = UIDoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);

                // Get Element
                ElementId elementId = r.ElementId;
                Element element = doc.GetElement(elementId);

                // Get information of element
                ElementId elementIdType = element.GetTypeId();
                ElementType elementType = doc.GetElement(elementIdType) as ElementType;

                if (r != null)
                {
                    TaskDialog.Show("Element Information", "Category: " + element.Category.Name + Environment.NewLine
                        + "Name of Element: " + element.Name + Environment.NewLine
                        + "Name of Family: " + elementType.FamilyName + Environment.NewLine
                        + "Name of Family Type - Symbol" + elementType.Name);
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
