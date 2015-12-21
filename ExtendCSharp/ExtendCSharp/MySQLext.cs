using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendCSharp
{
    class MySQLext
    {
        public MySqlConnection c;
        public Exception LastException { get; private set; }

        public MySQLext(String IpServer, String DBname, String User, String Pass)
        {
            Connect(IpServer, DBname, User, Pass);
        }

        public bool Connect(String IpServer, String DBname, String User, String Pass)
        {
            if (c != null)
                c.TryClose();

            c = new MySqlConnection("Server=" + IpServer + ";Database=" + DBname + ";Uid=" + User + ";Pwd=" + Pass + ";");

            try
            {
                c.Open();
            }
            catch (Exception ex)
            {
                LastException = ex;
                return false;
            }
            return true;
        }
        public bool Close()
        {
            Exception e = c.TryClose();
            if (e == null)
                return true;
            else
            {
                LastException = e;
                return false;
            }
        }

        public bool ExecuteQuery(String Query)
        {
            MySqlDataReader dr = null;
            try
            {
                MySqlCommand comm = new MySqlCommand(Query, c);
                dr = comm.ExecuteReader();
                dr.TryClose();
                return true;
            }
            catch (Exception e)
            {
                dr.TryClose();
                LastException= e;
                return false;
            }
        }
        public MySqlDataReader ExecuteReaderQuery(String Query)
        {
            try
            {
                MySqlCommand comm = new MySqlCommand(Query, c);
                return comm.ExecuteReader();
            }
            catch { return null; }
        }

        public bool FillDataGrid(String query, DataGridView d)
        {
            try
            {
                MySqlDataAdapter adr = new MySqlDataAdapter(query, c);
                adr.SelectCommand.CommandType = CommandType.Text;
                DataTable dt = new DataTable();
                adr.Fill(dt);
                d.DataSource = dt;
            }
            catch (Exception ex)
            {
                LastException= ex;
                return false;
            }
            return true;
        }
    }
}
