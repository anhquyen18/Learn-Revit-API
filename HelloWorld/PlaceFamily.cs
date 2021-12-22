using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace HelloWorld
{
    [TransactionAttribute(TransactionMode.Manual)]
    class PlaceFamily : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Get UIDocument
            UIDocument UIDoc = commandData.Application.ActiveUIDocument;

            // Get Document
            Document doc = UIDoc.Document;

            //Find Family
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            FamilySymbol symbol = collector.OfClass(typeof(FamilySymbol))
                .WhereElementIsElementType().Cast<FamilySymbol>().First(x => x.Name == ("M_Muntin Pattern_2x2"));

           //FamilySymbol symbol = null;

           /* foreach (FamilySymbol sym in symbols)
            {
                if (sym.Name == "M_Muntin Pattern_2x2")
                {
                    symbol = sym as FamilySymbol;
                    break;
                }
            }*/

            try
            {
                using (Transaction trans = new Transaction(doc, "Place Family"))
                {
                    trans.Start();
                    if (symbol.IsActive == false)
                    {
                        symbol.Activate();
                    }
                    doc.Create.NewFamilyInstance(new XYZ(50, 50, 50), symbol,
                        Autodesk.Revit.DB.Structure.StructuralType.NonStructural);

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
