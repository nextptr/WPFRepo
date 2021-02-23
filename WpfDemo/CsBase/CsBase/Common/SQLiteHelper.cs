using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsBase.Common
{
    public class TableField
    {
        public string Cid;         //0 序号
        public string Name;        //1 名字
        public string Type;        //2 数据类型
        public string NotNull;     //3 能否为null值,0不能，1能
        public string Dflt_value;  //4 缺省值
        public string pk;          //5 是否是主键，0否，1是
        public TableField()
        {
            Cid = "";
            Name = "";
            Type = "";
            NotNull = "";
            Dflt_value = "";
            pk = "";
        }
    }
    public class TableInfo:List<TableField>
    {
        public string TableName = "";
    }

    public class DataItem
    {
        public string TableName;
        public int Rowid;
        public int Unit;
        public string Remark;
        public DataItem()
        {
            Rowid = -1;
            Unit = -1;
            Remark = "";
        }
    }

    public class SQLiteHelper
    {
        protected string DbPath = "";
        protected SQLiteConnection _con=null;
        protected SQLiteCommand _cmd = null;
        protected readonly int StrCount = 256;

        public static SQLiteHelper Instance
        {
            get
            {
                if (_isntance == null)
                {
                    _isntance = new SQLiteHelper();
                }
                return _isntance;
            }
        }
        protected static SQLiteHelper _isntance = null;
        protected SQLiteHelper()
        { }

        public void CreateDB(string DirPath,string DbName)
        {
            DbPath = DirPath + "\\" + DbName;
            _con = new SQLiteConnection("data source=" + DbPath);
            _con.Open();
            _con.Close();
            _cmd = new SQLiteCommand();
            _cmd.Connection = _con;
        }
        public void DeleteDB(string DirPath, string DbName)
        {
            if (_con != null)
            {
                _con.Close();
                _con = null;
                _cmd = null;
            }
            if (File.Exists(DirPath + "\\" + DbName))
            {
                File.Delete(DirPath + "\\" + DbName);
            }
        }

        protected void assert()
        {
            if (_con == null)
            {
                return;
            }
            if (_con.State != ConnectionState.Open)
            {
                _con.Open();
            }
            if (_cmd == null)
            {
                _cmd = new SQLiteCommand();
            }
            _cmd.Connection = _con;
        }
        protected bool CreateTable(string tableName)
        {
            if (_con == null) { return false; }
            if (_con.State != ConnectionState.Open)
            {
                _con.Open();
            }
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = _con;
            //cmd.CommandText = string.Format("CREATE TABLE IF NOT EXISTS {0}", tableName) + "(id INTEGER PRIMARY KEY AUTOINCREMENT ,unit varchar(4), remark varchar(128))";
            cmd.CommandText = string.Format("CREATE TABLE IF NOT EXISTS {0}", tableName) + $"(id int ,unit int, remark NVARCHAR({StrCount}))";
            cmd.ExecuteNonQuery();
            _con.Close();
            return true;
        }
        protected void DeleteTable(string tableName)
        {
            if (_con == null) { return; }
            //如果存在同名表则返回true
            if (_con.State != ConnectionState.Open)
            {
                _con.Open();
            }
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = _con;
            cmd.CommandText = "DROP TABLE IF EXISTS " + tableName;
            cmd.ExecuteNonQuery();
            _con.Close();
        }
        protected bool IsExisTable(string tableName)
        {
            assert();
            bool ret = false;
            _cmd.CommandText = $"SELECT COUNT(*) FROM sqlite_master WHERE TYPE='table' AND name='{tableName}'";
            int count = 0;
            count = int.Parse(_cmd.ExecuteScalar().ToString());
            if (count > 0)
            {
                ret = true;
            }
            _con.Close();
            return ret;
            //assert();
            //bool ret = false;
            //_cmd.CommandText = $"SELECT tbl_name FROM sqlite_master WHERE TYPE='table'";
            //SQLiteDataReader sr = _cmd.ExecuteReader();
            //while (sr.Read())
            //{
            //    if (string.Compare(sr.GetString(0), tableName, true) == 0)
            //    {
            //        ret = true;
            //        break;
            //    }
            //}
            //sr.Close();
            //_con.Close();
            //return ret;
        }
        protected void InsertTableValue(string tableName, int unit, string remark)
        {
            assert();
            _cmd.CommandText = $"SELECT COUNT(*) FROM {tableName} WHERE unit={unit}";
            int count = int.Parse(_cmd.ExecuteScalar().ToString()) + 1;
            _cmd.CommandText = $"INSERT INTO {tableName} VALUES({count},{unit},'{remark}')";
            _cmd.ExecuteNonQuery();
            _con.Close();
        }

        public void UpdateTableValue(DataItem item)
        {
            assert();
            if (!IsExisTable(item.TableName))
            {
                CreateTable(item.TableName);
            }
            assert();
            _cmd.CommandText = $"DELETE FROM {item.TableName} WHERE unit={item.Unit}";
            _cmd.ExecuteNonQuery();
            _con.Close();
            string tmp = item.Remark;
            while (tmp.Length / StrCount > 0)
            {
                InsertTableValue(item.TableName, item.Unit, tmp.Substring(0, StrCount));
                tmp=tmp.Remove(0, StrCount);
            }
            if (tmp.Length > 0)
            {
                InsertTableValue(item.TableName, item.Unit, tmp);
            }
        }
        public void GetTableValue(ref DataItem item)
        {
            assert();
            if (!IsExisTable(item.TableName))
            {
                return;
            }
            assert();
            _cmd.CommandText = $"SELECT * FROM {item.TableName} WHERE unit={item.Unit}";
            SQLiteDataReader sr = _cmd.ExecuteReader();
            item.Remark = "";
            while (sr.Read())
            {
                item.Remark += sr[2].ToString();
            }
            sr.Close();
            _con.Close();
        }
        //public void InsertTableValue(string tableName, int unit, string remark)
        //{
        //    assert();
        //    _cmd.CommandText = $"SELECT COUNT(*) FROM {tableName} WHERE unit={unit}";
        //    int count = int.Parse(_cmd.ExecuteScalar().ToString()) + 1;
        //    _cmd.CommandText = $"INSERT INTO {tableName} VALUES({count},{unit},'{remark}')";
        //    _cmd.ExecuteNonQuery();
        //    _con.Close();
        //}
        //public void DeleteTableValue(string tableName, int unit,int rowid)
        //{
        //    assert();
        //    //删除对应条目
        //    _cmd.CommandText = $"DELETE FROM {tableName} WHERE unit={unit} AND id={rowid}";
        //    _cmd.ExecuteNonQuery();
        //    //id递增排序
        //    _cmd.CommandText = $"SELECT COUNT(*) FROM {tableName} WHERE unit={unit}";
        //    int count = int.Parse(_cmd.ExecuteScalar().ToString());

        //    _cmd.CommandText = $"SELECT id FROM {tableName} WHERE unit={unit} ORDER BY id ASC";
        //    SQLiteDataReader sr = _cmd.ExecuteReader();
        //    List<int> ls = new List<int>();
        //    while (sr.Read())
        //    {
        //        ls.Add(int.Parse(sr[0].ToString()));
        //    }
        //    sr.Close();

        //    for (int i = 0; i < count; i++)
        //    {
        //        _cmd.CommandText = $"UPDATE {tableName} SET id={i + 1} WHERE id={ls[i]}";
        //        _cmd.ExecuteNonQuery();
        //    }
        //    _con.Close();
        //}
        //public void UpdateTableValue(string tableName, DataItem item)
        //{
        //    assert();
        //    _cmd.CommandText = $"UPDATE {tableName} SET remark='{item.Remark}' WHERE unit={item.Unit} AND id={item.Rowid}";
        //    _cmd.ExecuteNonQuery();
        //    _con.Close();
        //}
        //public DataItems GetTableValue(string tableName, int unit)
        //{
        //    assert();
        //    _cmd.CommandText = $"SELECT * FROM {tableName} WHERE unit={unit}";
        //    SQLiteDataReader sr = _cmd.ExecuteReader();

        //    DataItems ls = new DataItems();
        //    while (sr.Read())
        //    {
        //        DataItem item = new DataItem();
        //        item.Rowid = int.Parse(sr[0].ToString());
        //        item.Unit = int.Parse(sr[1].ToString());
        //        item.Remark = sr[2].ToString();
        //        ls.Add(item);
        //    }
        //    sr.Close();
        //    _con.Close();
        //    return ls;
        //}
    }

    //public class SQLiteHelper
    //{
    //    //创建数据库文件
    //    public static void CreateDBFile(string fileName)
    //    {
    //        string path = System.Environment.CurrentDirectory + @"/Data/";
    //        if (!Directory.Exists(path))
    //        {
    //            Directory.CreateDirectory(path);
    //        }
    //        string databaseFileName = path + fileName;
    //        if (!File.Exists(databaseFileName))
    //        {
    //            SQLiteConnection.CreateFile(databaseFileName);
    //        }
    //    }

    //    //生成连接字符串
    //    private static string CreateConnectionString()
    //    {
    //        SQLiteConnectionStringBuilder connectionString = new SQLiteConnectionStringBuilder();
    //        connectionString.DataSource = @"data/ScriptHelper.db";

    //        string conStr = connectionString.ToString();
    //        return conStr;
    //    }

    //    /// <summary>
    //    /// 对插入到数据库中的空值进行处理
    //    /// </summary>
    //    /// <param name="value"></param>
    //    /// <returns></returns>
    //    public static object ToDbValue(object value)
    //    {
    //        if (value == null)
    //        {
    //            return DBNull.Value;
    //        }
    //        else
    //        {
    //            return value;
    //        }
    //    }

    //    /// <summary>
    //    /// 对从数据库中读取的空值进行处理
    //    /// </summary>
    //    /// <param name="value"></param>
    //    /// <returns></returns>
    //    public static object FromDbValue(object value)
    //    {
    //        if (value == DBNull.Value)
    //        {
    //            return null;
    //        }
    //        else
    //        {
    //            return value;
    //        }
    //    }

    //    /// <summary>
    //    /// 执行非查询的数据库操作
    //    /// </summary>
    //    /// <param name="sqlString">要执行的sql语句</param>
    //    /// <param name="parameters">参数列表</param>
    //    /// <returns>返回受影响的条数</returns>
    //    public static int ExecuteNonQuery(string sqlString, params SQLiteParameter[] parameters)
    //    {
    //        string connectionString = CreateConnectionString();
    //        using (SQLiteConnection conn = new SQLiteConnection(connectionString))
    //        {
    //            conn.Open();
    //            using (SQLiteCommand cmd = conn.CreateCommand())
    //            {
    //                cmd.CommandText = sqlString;
    //                foreach (SQLiteParameter parameter in parameters)
    //                {
    //                    cmd.Parameters.Add(parameter);
    //                }
    //                return cmd.ExecuteNonQuery();
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// 执行查询并返回查询结果第一行第一列
    //    /// </summary>
    //    /// <param name="sqlString">SQL语句</param>
    //    /// <param name="sqlparams">参数列表</param>
    //    /// <returns></returns>
    //    public static object ExecuteScalar(string sqlString, params SQLiteParameter[] parameters)
    //    {
    //        string connectionString = CreateConnectionString();
    //        using (SQLiteConnection conn = new SQLiteConnection(connectionString))
    //        {
    //            conn.Open();
    //            using (SQLiteCommand cmd = conn.CreateCommand())
    //            {
    //                cmd.CommandText = sqlString;
    //                foreach (SQLiteParameter parameter in parameters)
    //                {
    //                    cmd.Parameters.Add(parameter);
    //                }
    //                return cmd.ExecuteScalar();
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// 查询多条数据
    //    /// </summary>
    //    /// <param name="sqlString">SQL语句</param>
    //    /// <param name="parameters">参数列表</param>
    //    /// <returns>返回查询的数据表</returns>
    //    public static DataTable GetDataTable(string sqlString, params SQLiteParameter[] parameters)
    //    {
    //        string connectionString = CreateConnectionString();
    //        using (SQLiteConnection conn = new SQLiteConnection(connectionString))
    //        {
    //            conn.Open();
    //            using (SQLiteCommand cmd = conn.CreateCommand())
    //            {
    //                cmd.CommandText = sqlString;
    //                foreach (SQLiteParameter parameter in parameters)
    //                {
    //                    cmd.Parameters.Add(parameter);
    //                }
    //                DataSet ds = new DataSet();
    //                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
    //                adapter.Fill(ds);
    //                return ds.Tables[0];
    //            }
    //        }
    //    }
    //}
}
