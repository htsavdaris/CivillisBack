using ClosedXML;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Presentation;
using System.IO;
using CivilisDB.Models;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CivilisApp.Services
{
    public class ImportPersonExcel
    {
        private const int EPONYMO_COL= 1;
        private const int ONOMA_COL = 2;
        private const int KINITO_COL = 3;
        private const int MAIL_COL = 4;
        private const int DIEFTHINI_COL = 5;
        private const int AR_COL = 6;
        private const int TK_COL = 7;
        private const int POLI_COL = 8;
        private const int PROFESSION_COL = 9;
        //private const int GENDER_COL = 10;
        //private const int GENDER_COL = 10;




        public ImportPersonExcel() { }

        public void ImportToTemp(string fileName, string connectionString, ILogger logger,long userid, string datasource)
        {

            string[] tempRow = new string[10];

            bool canReadData = true;
            int curRow = 2;
            var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var Document = new XLWorkbook(fileStream);
            while (canReadData)
            {
                var c = Document.Worksheet("").Cell(curRow, 1);
                var row = Document.Worksheet(0).Row(curRow);
                bool empty = row.IsEmpty();
                if (!empty)
                {
                    for (int i = 1; i <= PROFESSION_COL; i++)
                    {
                        tempRow[i] = row.Cell(i).Value.GetText() ?? "";
                    }
                    InsertRowToTemp(tempRow, connectionString, logger, userid, datasource);
                    curRow += 1;
                }
                else
                {
                    canReadData = false;
                }
            }
        }

        public long InsertRowToTemp(string[] tempRow, string connectionString, ILogger logger, long userid, string datasource)
        {
            tblImportedPerson tblImportedPersonRow = new tblImportedPerson();
            tblImportedPersonRow.fldpersonlastname = tempRow[EPONYMO_COL];
            tblImportedPersonRow.fldpersonfirstname = tempRow[ONOMA_COL];
            tblImportedPersonRow.fldpersonmobile = tempRow[KINITO_COL];
            tblImportedPersonRow.fldpersonemail = tempRow[MAIL_COL];
            tblImportedPersonRow.fldpersonstreet1 = tempRow[DIEFTHINI_COL];
            tblImportedPersonRow.fldpersonnumber1 = tempRow[AR_COL];
            tblImportedPersonRow.fldpersontk1 = tempRow[TK_COL];
            tblImportedPersonRow.fldcity = tempRow[POLI_COL];
            tblImportedPersonRow.fldprofession = tempRow[PROFESSION_COL];
            tblImportedPersonRow.fldcreatedat = DateTime.Now;
            tblImportedPersonRow.fldupdatedat = DateTime.Now;
            tblImportedPersonRow.fldcreatedby = userid;
            tblImportedPersonRow.fldupdatedby = userid;
            tblImportedPersonRow.flddatasource = datasource;
            tblImportedPersonDac dac = new tblImportedPersonDac(connectionString, logger);
            try
            {
                bool res = dac.Insert(tblImportedPersonRow);
                if (res)
                {
                    //tblImportedPersonRow.fldpersonid = id.Value;
                    return tblImportedPersonRow.fldpersonid;
                }
                else
                    return 0;
            }
            catch (Npgsql.NpgsqlException ex)  {
                return -1;
            }
        }

        public void ProcessTemp()
        {

        }

    }
}

