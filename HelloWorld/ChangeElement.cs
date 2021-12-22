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
    class ChangeElement : IExternalCommand
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

                if (r != null)
                {
                    using (Transaction trans = new Transaction(doc, "Change Element"))
                    {
                        trans.Start();

                        doc.Delete(r.ElementId);

                        TaskDialog tDiaglog = new TaskDialog("Delete Element");
                        tDiaglog.MainContent = "Bạn có chắc muốn xóa đối tượng này không";
                        tDiaglog.CommonButtons = TaskDialogCommonButtons.Ok | TaskDialogCommonButtons.Cancel;

                        if (tDiaglog.Show() == TaskDialogResult.Ok)
                        {
                            trans.Commit();
                            TaskDialog.Show("Delete Element", "Bạn đã xóa phần tử " + r.ElementId.ToString());

                        }
                        else
                        {
                            trans.RollBack();
                            TaskDialog.Show("Delete Element", "Phần tử " + r.ElementId.ToString() + " chưa bị xóa");
                        }
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
