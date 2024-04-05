using System;
using System.Data;
using System.Data.SqlClient;

namespace Object
{
    public class DataBaseProcess
    {
        //string strConnect = "Data Source=tcp:tranlenam.database.windows.net,1433;" +
        //"DataBase=ChatOnline;User ID=sqlserveronline;Password=@NamTranLe123;";
        string strConnect = "Data Source=LAPTOP-NUAT9P64;" +
        "DataBase=ChatOnline;User ID=tranlenam;Password=123;";
        SqlConnection sqlConnect = null;
 //Phương thức mở kết nối
 void OpenConnect()
 {
     sqlConnect = new SqlConnection(strConnect);
     if (sqlConnect.State != ConnectionState.Open)
         sqlConnect.Open();
 }
 //Phương thức đóng kết nối
 void CloseConnect()
 {
     if (sqlConnect.State != ConnectionState.Closed)
     {
         sqlConnect.Close();
         sqlConnect.Dispose();
     }
 }
 //Phương thức thực thi câu lệnh Select trả về một DataTable
 public DataTable DataReader(string sqlSelct)
 {
     DataTable tblData = new DataTable();
     OpenConnect();
     SqlDataAdapter sqlData = new SqlDataAdapter(sqlSelct, sqlConnect);
     sqlData.Fill(tblData);
     CloseConnect();
     return tblData;
 }
 //Phương thức thực hiện câu lệnh dạng insert,update,delete
 public void DataChange(string sql)
 {
     OpenConnect();
     SqlCommand sqlcomma = new SqlCommand();
     sqlcomma.Connection = sqlConnect;
     sqlcomma.CommandText = sql;
     sqlcomma.ExecuteNonQuery();
     CloseConnect();
 }
    }

}
