using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows;

namespace DatabaseHelper.Common
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
    public class TableInfo : List<TableField>
    {
        public string TableName = "";
    }
    public class SQLiteHelper
    {
        protected string DbPath = "";
        protected SQLiteConnection _con = null;
        protected SQLiteCommand _cmd = null;

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

        public void CreateDB(string DirPath, string DbName)
        {
            DbPath = DirPath + "\\" + DbName;
            _con = new SQLiteConnection("data source=" + DbPath);
            _con.Open();
            _con.Close();
            _cmd = new SQLiteCommand();
            _cmd.Connection = _con;
            return;
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

        public bool CreateTabel(TableInfo tableinfo)
        {
            assert();
            string sql = "";
            sql = string.Format("CREATE TABLE IF NOT EXISTS {0}", tableinfo.TableName);
            sql += "(";
            foreach (TableField dif in tableinfo)
            {
                sql += dif.Name + " ";
                sql += dif.Type + ",";
            }
            sql += ")";
            _cmd.CommandText = sql;
            _cmd.ExecuteNonQuery();
            _con.Close();
            return true;
        }
        public bool CreateTabel(string tableName)
        {
            if (_con == null) { return false; }
            if (_con.State != ConnectionState.Open)
            {
                _con.Open();
            }
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = _con;
            cmd.CommandText = string.Format("CREATE TABLE IF NOT EXISTS {0}", tableName) + "(id INTEGER PRIMARY KEY AUTOINCREMENT,unit int,remark varchar(256))";
            cmd.ExecuteNonQuery();
            _con.Close();
            return true;
        }
        public bool CreateTabelNoKey(string tableName)
        {
            if (_con == null) { return false; }
            if (_con.State != ConnectionState.Open)
            {
                _con.Open();
            }
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = _con;
            cmd.CommandText = string.Format("CREATE TABLE IF NOT EXISTS {0}", tableName) + "(id int,unit int,remark varchar(100))";
            cmd.ExecuteNonQuery();
            _con.Close();
            return true;
        }

        public bool IsExisTable(string tableName)
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
            //string tmp = "";
            //while (sr.Read())
            //{
            //    tmp = sr.GetString(0);
            //    if (string.Compare(sr.GetString(0), tableName, true) == 0)
            //    {
            //        ret = true;
            //        //break;
            //    }
            //}
            //sr.Close();
            //_con.Close();
            //return ret;
        }
        public void DeleteTabel(string tableName)
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
        public void AlterTabelName(string Nam, string toNam)
        {
            assert();
            _cmd.CommandText = $"AlTER TABLE {Nam} RENAME TO {toNam}";
            _cmd.ExecuteNonQuery();
        }

        public void AtomInsertTableValue(string tableName, params object[]datas)
        {
            assert();
            string insertString = $"INSERT INTO {tableName} VALUES (";
            for (int i = 0; i < datas.Length; i++)
            {
                if (i == 0)
                {
                    insertString += datas[i].ToString() + ',';
                }
                else
                {
                    insertString += "'" + datas[i].ToString() + "'" + ',';
                }
            }
            _cmd.CommandText = insertString.TrimEnd(',') + ')';
            _cmd.ExecuteNonQuery();
            _con.Close();
        }
        public void AtomInsertTableValue(string tableName,List<object> datas)
        {
            assert();
            string insertString = $"INSERT INTO {tableName} VALUES (";
            for (int i = 0; i < datas.Count; i++)
            {
                if (i == 0)
                {
                    insertString += datas[i].ToString() + ',';
                }
                else
                {
                    insertString += "'" + datas[i].ToString() + "'" + ',';
                }
            }
            _cmd.CommandText = insertString.TrimEnd(',') + ')';
            _cmd.ExecuteNonQuery();
            _con.Close();
        }

        //连续写入数据库
        private string dataTableName = "";
        private bool isReadyInsert = false;
        public bool ReadyInsertTable(string tableName)
        {
            try
            {
                if (_con == null)
                {
                    MessageBox.Show("数据库不存在!");
                    isReadyInsert = false;
                    return isReadyInsert;
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

                if (!IsExisTable(tableName))
                {
                    MessageBox.Show($"不存在{tableName}数据表!");
                    isReadyInsert = false;
                }
                else
                {
                    _con.Open();
                    isReadyInsert = true;
                    dataTableName = tableName;
                }
                return isReadyInsert;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        public void ExecuteInsertTableValue(List<object> datas)
        {
            if (isReadyInsert)
            {
                string insertString = $"INSERT INTO {dataTableName} VALUES (";
                for (int i = 0; i < datas.Count; i++)
                {
                    if (i == 0)
                    {
                        insertString += datas[i].ToString() + ',';
                    }
                    else
                    {
                        insertString += "'" + datas[i].ToString() + "'" + ',';
                    }
                }
                _cmd.CommandText = insertString.TrimEnd(',') + ')';
                _cmd.ExecuteNonQuery();
            }
        }
        public void FinishInsertTableValue()
        {
            _con.Close();
            isReadyInsert = false;
        }

        public void DeleteTableVAlue(string tableName, int id)
        {
            assert();
            _cmd.CommandText = $"DELETE FROM {tableName} WHERE id={id}";
            _cmd.ExecuteNonQuery();
            _con.Close();
        }
        public List<string> GeTableItemValue(string tableName, int unit)
        {
            assert();
            _cmd.CommandText = $"SELECT * FROM {tableName} WHERE unit={unit}";
            SQLiteDataReader sr = _cmd.ExecuteReader();

            List<string> ls = new List<string>();
            while (sr.Read())
            {
                string tmp = "<";
                tmp += sr[0].ToString() + ",";
                tmp += sr[1].ToString() + ",";
                tmp += sr[2].ToString() + ">";
                ls.Add(tmp);
            }
            sr.Close();
            _con.Close();
            return ls;
        }
        //public void UpdateTableValue(string tableName, int unit, int rowid, string remark)
        //{
        //    assert();
        //    _cmd.CommandText = $"UPDATE {tableName} SET remark='{remark}' WHERE unit={unit} AND id={rowid}";
        //    _cmd.ExecuteNonQuery();
        //    _con.Close();
        //}

        public void UpdateTableValue(string tableName, object repField,object repValue ,object repKey,object repId)
        {
            assert();
            _cmd.CommandText = $"UPDATE {tableName} SET {repField}='{repValue}' WHERE {repKey}={repId}";
            _cmd.ExecuteNonQuery();
            _con.Close();
        }

        public void InsertTableValueNokey(string tableName, int unit, string remark)
        {
            assert();
            _cmd.CommandText = $"SELECT COUNT(*) FROM {tableName} WHERE unit={unit}";
            int count = int.Parse(_cmd.ExecuteScalar().ToString()) + 1;
            _cmd.CommandText = $"INSERT INTO {tableName} VALUES({count},{unit},'{remark}')";
            _cmd.ExecuteNonQuery();
            _con.Close();
        }
        public void DeleteTableValueNokey(string tableName, int unit, int rowid)
        {
            assert();
            //删除对应条目
            _cmd.CommandText = $"DELETE FROM {tableName} WHERE unit={unit} AND id={rowid}";
            _cmd.ExecuteNonQuery();
            //id递增排序
            _cmd.CommandText = $"SELECT COUNT(*) FROM {tableName} WHERE unit={unit}";
            int count = int.Parse(_cmd.ExecuteScalar().ToString());

            _cmd.CommandText = $"SELECT id FROM {tableName} WHERE unit={unit} ORDER BY id ASC";
            SQLiteDataReader sr = _cmd.ExecuteReader();
            List<int> ls = new List<int>();
            while (sr.Read())
            {
                ls.Add(int.Parse(sr[0].ToString()));
            }
            sr.Close();

            for (int i = 0; i < count; i++)
            {
                _cmd.CommandText = $"UPDATE {tableName} SET id={i + 1} WHERE id={ls[i]}";
                _cmd.ExecuteNonQuery();
            }
            _con.Close();
        }

        public void AddTableField(string tableName, TableField field)
        {
            assert();
            _cmd.CommandText = $"ALTER TABLE {tableName} ADD COLUMN {field.Name} {field.Type}";
            _cmd.ExecuteNonQuery();
        }
        public void AlterFieldName(string tableName, string Nam, string toNam)
        {
            assert();
            _cmd.CommandText = "SELECT name,sql FROM sqlie_master WHERE TYPE='table' ORDER BY name";
            SQLiteDataReader sr = _cmd.ExecuteReader();
            string _sql = "";
            while (sr.Read())
            {
                if (string.Compare(sr.GetString(0), tableName, true) == 0)
                {//选中要修改的表，取出sql语句
                    _sql = sr.GetString(1);
                    break;
                }
            }
            sr.Close();
            //改旧表名为带"_old"
            string _old = tableName + "_old";
            _cmd.CommandText = $"ALTER TABLE {tableName} RENAME TO {_old}";
            _cmd.ExecuteNonQuery();
            //将旧sql中的(旧字段名Nam)替换成(新的字段名toNam),建立新表
            _sql = _sql.Replace(Nam, toNam);
            _cmd.CommandText = _sql;
            _cmd.ExecuteNonQuery();
            //拷贝数据
            using (SQLiteTransaction tr = _con.BeginTransaction())
            {
                _cmd.CommandText = $"INSERT INTO {tableName} SELECT * FROM {_old}";
                _cmd.ExecuteNonQuery();
                _cmd.CommandText = $"DROP TABLE {_old}";
                _cmd.ExecuteNonQuery();
                tr.Commit();
            }
            _con.Close();
        }
        public void AlterFieldName2(string tableName, string Nam, string toNam)
        {
            //改写schema里建表时的sql语句
            //sqlite每次打开时，都是依据建表时的sql语句动态建立column字段信息
            assert();
            //获取原sql语句
            _cmd.CommandText = $"SELECT sql FROM sqlite_master WHERE TYPE='table' AND name='{tableName}'";
            SQLiteDataReader sr = _cmd.ExecuteReader();
            sr.Read();
            string _sql = sr.GetString(0);
            sr.Close();
            _sql = $"UPDATE sqlite_master SET sql='{_sql.Replace(Nam, toNam)}' WHERE name='{tableName}'";
            //设置writable_scema为true,准备改写schema
            _cmd.CommandText = "pragma writable_schema=1";
            _cmd.ExecuteNonQuery();
            _cmd.CommandText = _sql;
            _cmd.ExecuteNonQuery();
            //设置writable_scema为false
            _cmd.CommandText = "pragma writable_scheme=0";
            _cmd.ExecuteNonQuery();
        }
        public bool IsExisTableField(string tableName, string fieldName)
        {
            List<TableInfo> ls = GetTableInfos();
            foreach (TableInfo tab in ls)
            {
                if (tab.TableName.Equals(tableName))
                {
                    foreach (TableField fid in tab)
                    {
                        if (string.Compare(fid.Name, fieldName, true) == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public List<string> GetSqlstr()
        {
            assert();
            List<string> ret = new List<string>();
            _cmd.CommandText = "SELECT sql FROM sqlite_master WHERE TYPE='table'";
            SQLiteDataReader sr = _cmd.ExecuteReader();
            while (sr.Read())
            {
                string tmp = sr[0].ToString();
                ret.Add(tmp);
            }
            sr.Close();
            _con.Close();
            return ret;
        }
        public TableInfo GetTableInfo(string tableName)  //查询单表中字段的属性
        {
            if (_con == null) { return null; }
            if (_con.State != ConnectionState.Open)
            {
                _con.Open();
            }
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = _con;
            cmd.CommandText = string.Format("PRAGMA table_info('{0}')", tableName);
            SQLiteDataReader reader = cmd.ExecuteReader();
            TableInfo retInfo = new TableInfo();
            while (reader.Read())
            {
                TableField tmp = new TableField();
                tmp.Cid = $"{reader[0]} ";         //表字段id
                tmp.Name = $"{reader[1]} ";        //表字段名
                tmp.Type = $"{reader[2]} ";        //表字段类型
                tmp.NotNull = $"{reader[3]} ";     //表字段是否可空
                tmp.Dflt_value = $"{reader[4]} ";  //表字段默认值
                tmp.pk = $"{reader[5]} ";          //表字段是否为键值
                retInfo.Add(tmp);
            }
            reader.Close();
            _con.Close();
            return retInfo;
        }
        /// <summary>
        /// sqlite内置表sqlite_master，使用B-tree管理数据库所有表的信息
        ///
        /// CREATE TABLE sqlite_master(
        /// type text,
        /// name text,
        /// tbl_name text,
        /// rootpage integer,
        /// sql text
        /// );
        /// sql语句用例"SELECT name FROM sqlite_master WHERE TYPE='table'"
        /// </summary>
        /// <returns></returns>
        public List<TableInfo> GetTableInfos()
        {
            if (_con == null) { return null; }
            if (_con.State != ConnectionState.Open) { _con.Open(); }
            List<TableInfo> retList = new List<TableInfo>();
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = _con;
            cmd.CommandText = "SELECT name FROM sqlite_master WHERE TYPE='table'";
            SQLiteDataReader sr = cmd.ExecuteReader();
            List<string> tables = new List<string>();
            while (sr.Read())
            {
                tables.Add(sr.GetString(0));
            }
            sr.Close();
            foreach (string tab in tables)
            {
                cmd.CommandText = $"PRAGMA TABLE_INFO({tab})";
                sr = cmd.ExecuteReader();
                TableInfo info = new TableInfo();
                info.TableName = tab;
                while (sr.Read())
                {
                    TableField tmp = new TableField();
                    tmp.Cid = $"{sr[0]}";
                    tmp.Name = $"{sr[1]}";
                    tmp.Type = $"{sr[2]}";
                    tmp.NotNull = $"{sr[3]}";
                    tmp.Dflt_value = $"{sr[4]}";
                    tmp.pk = $"{sr[5]} ";
                    info.Add(tmp);
                }
                retList.Add(info);
                sr.Close();
            }
            _con.Close();
            return retList;
        }
    }
}
