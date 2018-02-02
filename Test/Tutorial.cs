using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using System;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using ConsoleApi;
using Autodesk.AutoCAD.EditorInput;

namespace Test
{
    public class Tutorial : IExtensionApplication
    {
        [CommandMethod("ShowUSDCourse")]
        public void Helloworld()
        {
            Editor editor = Application.DocumentManager.MdiActiveDocument.Editor;
            editor.WriteMessage("Привет из Autocad плагина");

            PromptResult pr = editor.GetString("\nProvide a date in format YYYY.MM : ");
            if (pr.Status != PromptStatus.OK)
            {
                editor.WriteMessage("No string was provided\n");
                
                return;
            }
            
            using (Polyline acPoly = new Polyline())
            {
                DateTime date;
                DateTime.TryParse(pr.StringResult, out date);
                double[] array = Getter.RequestForMonth(date);
                int counter = 0;
                foreach (double a in array)
                {
                    acPoly.AddVertexAt(counter, new Point2d(counter++ *3, a*10 - array[0]*10), 0, 0, 0);
                }

                Database acadDB = HostApplicationServices.WorkingDatabase;
                Autodesk.AutoCAD.DatabaseServices.TransactionManager acadTransMgr = acadDB.TransactionManager;
                Transaction acadTrans = acadTransMgr.StartTransaction();
                BlockTableRecord acadBTR = (BlockTableRecord)acadTrans.GetObject(acadDB.CurrentSpaceId, OpenMode.ForWrite);
                acadBTR.AppendEntity(acPoly);
                acadTrans.AddNewlyCreatedDBObject(acPoly, true);
                acadTrans.Commit();
                Document acDoc = Application.DocumentManager.MdiActiveDocument;


                acDoc.SendStringToExecute(".показать границы ", true, false, true);
            }



            
        }

        public void Initialize()
        {
            var editor = Application.DocumentManager.MdiActiveDocument.Editor;
            editor.WriteMessage("Инициализация плагина.." + Environment.NewLine);
        }

        public void Terminate()
        {

        }

    }
}