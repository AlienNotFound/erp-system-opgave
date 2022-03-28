using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TECHCOOL.UI;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemOpgave
{
    public class DataBase
    {
        public void GetAllProducts()
      {
          string connectionString = @"Server=ADAMS-BÆRBAR\MSSQLSERVER01;Database=erp_system;Trusted_Connection=True;";
          SqlConnection connection = new SqlConnection(connectionString);
          connection.Open();

          SqlDataReader dt;
          SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_productmodule", connection);
          dt = cmd.ExecuteReader();
          
          ListPage<ProductDetails> listPage = new ListPage<ProductDetails>();
          while (dt.Read())
          {
              listPage.Add(new ProductDetails(
                  Convert.ToInt32(dt["id"]),
                  dt["productname"].ToString(),
                  dt["details"].ToString(),
                  Convert.ToInt32(dt["stockunits"]),
                  Convert.ToDecimal(dt["buyprice"]),
                  Convert.ToDecimal(dt["salesprice"]),
                  dt["location"].ToString(),
                  Convert.ToDecimal(dt["salesprice"]),
                  dt["unit"].ToString(),
                  Convert.ToDouble(dt["avancepercent"]),
                  Convert.ToDouble(dt["avancekroner"])));
          }
          
          listPage.AddColumn("Varenr.", "ProductNumber");
          listPage.AddColumn("Produktnavn", "Name");
          listPage.AddColumn("Lagerantal", "StockUnits");
          listPage.AddColumn("Købspris", "BuyPrice");
          listPage.AddColumn("Salgspris", "SalesPrice");
          listPage.AddColumn("Avance i procent", "AvancePercent");
        
          //Prints the selected product name out in the console, after pressing enter. Preparation for P3 
          Console.WriteLine("Valgte: " + listPage.Select().Name);
          connection.Close();
      }
    }
}
