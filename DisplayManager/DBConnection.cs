using SPAMI.Util.Logger;
using System;
using System.Data.SqlClient;

namespace DisplayManager
{
    public class DBConnection {

        public bool Ready { get; set; }
        string _server;
        string _database;
        string _tableName;
        //SqlConnection sqlConnection = new SqlConnection("user id='';" +
        //                                           "password ='';" +
        //                                           @"server = USER-HP\PVSI_DB;" +
        //                                           "Trusted_connection=yes;" +
        //                                           "database=CLEANER;" +
        //                                           "connection timeout =10;");
        public SqlConnection SqlConnection {
            get;
            private set;
        }
        public string stationId_;
        public string toolId_;
        public string name_;
        public string limit_;
        public string value_;

        public DBConnection(string server, string database, string tableName) {
            _server = server;           // USER-HP\PVSI_DB
            _database = database;       // CLEANER
            _tableName = tableName;     // dbo.DetailedReports3
            SqlConnection = new SqlConnection("user id='';" +
                                              "password ='';" +
                                              "server = " + _server + ";" +
                                              "Trusted_connection=yes;" +
                                              "database=" + _database + ";" +
                                              "connection timeout =10;");
        }

        public bool OpenConnection() {
            try {
                lock (SqlConnection) {
                    SqlConnection.Open();
                }
                Log.Line(LogLevels.Pass, "DBConnection.OpenConnection", "Database connected successfully");
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "DBConnection.OpenConnection", "Error: " + ex.Message);
                return false;
            }
            return true;
        }
        
        public bool CloseConnection() {
            try {
                lock (SqlConnection) {
                    SqlConnection.Close();
                }
                Log.Line(LogLevels.Pass, "DBConnection.OpenConnection", "Database disconnected successfully");
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "DBConnection.CloseConnection", "Error: " + ex.Message);
                return false;
            }
            return true;
        }

        public void DeleteTable() {
            SqlCommand comm_ = new SqlCommand("TRUNCATE TABLE " + _tableName, SqlConnection);        
            //SqlCommand command_ = new SqlCommand("DELETE FROM " + _tableName, SqlConnection);
            //command_.ExecuteNonQuery();
            

            comm_.ExecuteNonQuery();

        }

        public void FillData(int vialId, int statId, bool stationRej, int toolId, string toolDescription, string toolType, bool toolUsed, bool toolRej, string name, string limit, string value) {

            SqlCommand command_ = new SqlCommand("INSERT INTO " + _tableName + " (Date,VialId,StationId,StationRej,ToolId,ToolDescription,ToolType,ToolUsed,ToolRej,Name,Limit,Value) VALUES(@Date, @VialId, @StationId, @StationRej, @ToolId, @ToolDescription, @ToolType, @ToolUsed, @ToolRej, @Name, @Limit, @Value)", SqlConnection);

            DateTime date_ = DateTime.Now;
            string dateTime_ = date_.ToString("MM:dd:yyyy HH:mm:ss");
            command_.Parameters.AddWithValue("@Date", date_);
            command_.Parameters.AddWithValue("@VialId", vialId);
            command_.Parameters.AddWithValue("@StationId", statId);
            command_.Parameters.AddWithValue("@StationRej", stationRej);
            command_.Parameters.AddWithValue("@ToolId", toolId);
            command_.Parameters.AddWithValue("@ToolDescription", toolDescription);
            command_.Parameters.AddWithValue("@ToolType", toolType);
            command_.Parameters.AddWithValue("@ToolUsed", toolUsed);
            command_.Parameters.AddWithValue("@ToolRej", toolRej);
            command_.Parameters.AddWithValue("@Name", name);
            command_.Parameters.AddWithValue("@Limit", limit);
            command_.Parameters.AddWithValue("@Value", value);
            int row = command_.ExecuteNonQuery();

            Log.Line(LogLevels.Pass, "VisionSystemManager.OpenDatabase", "STATION: " + statId.ToString() + " VIA_ID: " + vialId.ToString());

        }
        
        public void ReadData() {

            SqlCommand command_ = new SqlCommand("select * FROM " + _tableName, SqlConnection);

            SqlDataReader read_ = command_.ExecuteReader();

            if (read_.HasRows) {
                while (read_.Read()) {

                    stationId_ = read_["StationId"].ToString();
                    toolId_ = read_["ToolId"].ToString();
                    name_ = read_["Name"].ToString();
                    limit_ = read_["Limit"].ToString();
                    value_ = read_["Value"].ToString();

                }
            }

        }





    }
}
