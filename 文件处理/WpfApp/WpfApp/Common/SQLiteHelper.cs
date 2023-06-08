using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Windows;

namespace WpfApp.Common
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
        protected bool assert()
        {
            if (_con == null)
            {
                return false;
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
            return true;
        }


        public void CreateDB(string DirPath, string DbName)
        {
            DbPath = DirPath + "\\" + DbName;
            ConnectDB(DbPath);
            return;
        }
        public string ConnectDB(string DbFullPath)
        {
            string beforDb = DbPath;
            DbPath = DbFullPath;
            _con = new SQLiteConnection("data source=" + DbPath);
            _con.Open();
            _con.Close();
            _cmd = new SQLiteCommand();
            _cmd.Connection = _con;
            return beforDb;
        }
        public void DeleteDB(string DirPath, string DbName)
        {
            string pth = DirPath + "\\" + DbName;
            DeleteDB(pth);
        }
        public void DeleteDB(string DirPath)
        {
            if (_con != null)
            {
                _con.Close();
                _con = null;
                _cmd = null;
            }
            if (File.Exists(DirPath))
            {
                File.Delete(DirPath);
            }
        }

        //SQLiteHelper.Instance.CreateTabel("MD5Table", "GroupId TEXT", "Key TEXT", "Val TEXT", "Mark TEXT");
        public bool CreateDefaultKeyTabel(string tableName, params string[] keys)
        {
            if (!assert())
                return false;

            string tableGrid = "(id INTEGER PRIMARY KEY AUTOINCREMENT";
            for (int i = 0; i < keys.Length; i++)
            {
                tableGrid += "," + keys[i];
            }
            tableGrid += ")";

            _cmd.CommandText = string.Format("CREATE TABLE IF NOT EXISTS {0}", tableName) + tableGrid;
            _cmd.ExecuteNonQuery();
            _con.Close();
            return true;
        }
        public bool CreateKeyTabel(string tableName, string tableKey, params string[] keys)
        {
            if (!assert())
                return false;

            string tableGrid = $"({tableKey}";
            for (int i = 0; i < keys.Length; i++)
            {
                tableGrid += "," + keys[i];
            }
            tableGrid += ")";

            _cmd.CommandText = string.Format("CREATE TABLE IF NOT EXISTS {0}", tableName) + tableGrid;
            _cmd.ExecuteNonQuery();
            _con.Close();
            return true;
        }
        public bool CreateTabelNoKey(string tableName, params string[] keys)
        {
            //cmd.CommandText = string.Format("CREATE TABLE IF NOT EXISTS {0}", tableName) + "(id int,unit int,remark varchar(100))";
            if (!assert())
                return false;

            string tableGrid = "(";
            for (int i = 0; i < keys.Length; i++)
            {
                if (i == 0)
                {
                    tableGrid += keys[i];
                }
                else
                {
                    tableGrid += "," + keys[i];
                }
            }
            tableGrid += ")";

            _cmd.CommandText = string.Format("CREATE TABLE IF NOT EXISTS {0}", tableName) + tableGrid;
            _cmd.ExecuteNonQuery();
            _con.Close();
            return true;
        }
        private void SortTableId(string tableName)
        {
            if (!IsExisTableField(tableName, "id"))
                return;

            if (!assert())
                return;

            //id递增排序
            _cmd.CommandText = $"SELECT COUNT(*) FROM {tableName}";
            int count = int.Parse(_cmd.ExecuteScalar().ToString());

            _cmd.CommandText = $"SELECT id FROM {tableName} ORDER BY id ASC";
            SQLiteDataReader sr = _cmd.ExecuteReader();
            List<int> ls = new List<int>();
            while (sr.Read())
            {
                ls.Add(int.Parse(sr[0].ToString()));
            }
            sr.Close();

            for (int i = 0; i < count; i++)
            {
                if (ls[i] != i + 1)
                {
                    _cmd.CommandText = $"UPDATE {tableName} SET id={i + 1} WHERE id={ls[i]}";
                    _cmd.ExecuteNonQuery();
                }
            }
        }
        public bool IsExisTable(string tableName)
        {
            if (!assert())
                return false;
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
            if (!assert())
                return;

            _cmd.CommandText = "DROP TABLE IF EXISTS " + tableName;
            _cmd.ExecuteNonQuery();
            _con.Close();
        }
        public void AlterTabelName(string Nam, string toNam)
        {
            if (!assert())
                return;
            _cmd.CommandText = $"AlTER TABLE {Nam} RENAME TO {toNam}";
            _cmd.ExecuteNonQuery();
        }
        public void InsertTableValue<T>(string tableName, T val)
        {
            if (!assert())
                return;
            Type tp = val.GetType();
            var arr = tp.GetProperties();
            string tableGrid = $"INSERT INTO {tableName} (";
            string tableConetnt = $"VALUES (";
            for (int i = 0; i < arr.Length; i++)
            {
                if (i == 0)
                {
                    tableGrid += arr[i].Name;

                    if (arr[i].PropertyType.Name == "String")
                    {
                        tableConetnt += "'" + arr[i].GetValue(val) + "'";
                    }
                    else
                    {
                        tableConetnt += arr[i].GetValue(val);
                    }
                }
                else
                {
                    tableGrid += ", " + arr[i].Name;
                    if (arr[i].PropertyType.Name == "String")
                    {
                        tableConetnt += ", '" + arr[i].GetValue(val) + "'";
                    }
                    else
                    {
                        tableConetnt += "," + arr[i].GetValue(val);
                    }
                }
            }
            tableGrid += ") ";
            tableConetnt += " );";

            _cmd.CommandText = tableGrid + tableConetnt;
            _cmd.ExecuteNonQuery();
            SortTableId(tableName);
            _con.Close();
        }
        public void InsertTableValue(string tableName, string key, List<KeyValuePair<string, string>> ls)
        {
            bool flag = IsExistTableValue(tableName, key);

            if (!assert())
                return;

            if (flag)
            {
                string upDat = "";
                for (int i = 0; i < ls.Count; i++)
                {
                    if (i == 0)
                    {
                        upDat += $"{ls[i].Key} = '{ls[i].Value}'";
                    }
                    else
                    {
                        upDat += $" , {ls[i].Key} = '{ls[i].Value}'";
                    }
                }
                _cmd.CommandText = $"UPDATE {tableName} SET {upDat} WHERE {key}";
            }
            else
            {
                string tableGrid = $"INSERT INTO {tableName} (";
                string tableConetnt = $"VALUES (";
                for (int i = 0; i < ls.Count; i++)
                {
                    if (i == 0)
                    {
                        tableGrid += ls[i].Key;
                        tableConetnt += "'" + ls[i].Value + "'";
                    }
                    else
                    {
                        tableGrid += ", " + ls[i].Key;
                        tableConetnt += ", '" + ls[i].Value + "'";
                    }
                }
                tableGrid += ") ";
                tableConetnt += " );";
                _cmd.CommandText = tableGrid + tableConetnt;
            }

            _cmd.ExecuteNonQuery();
            SortTableId(tableName);
            _con.Close();
        }
        public void AddTableValue(string tableName, List<KeyValuePair<string, string>> ls)
        {
            if (!assert())
                return;

            string tableGrid = $"INSERT INTO {tableName} (";
            string tableConetnt = $"VALUES (";
            for (int i = 0; i < ls.Count; i++)
            {
                if (i == 0)
                {
                    tableGrid += ls[i].Key;
                    tableConetnt += "'" + ls[i].Value + "'";
                }
                else
                {
                    tableGrid += ", " + ls[i].Key;
                    tableConetnt += ", '" + ls[i].Value + "'";
                }
            }
            tableGrid += ") ";
            tableConetnt += " );";
            _cmd.CommandText = tableGrid + tableConetnt;

            _cmd.ExecuteNonQuery();
            SortTableId(tableName);
            _con.Close();
        }
        public bool IsExistTableValue(string tableName, string key)
        {
            bool flag = false;
            if (!assert())
                return flag;

            _cmd.CommandText = $"SELECT * FROM {tableName} WHERE {key}";
            SQLiteDataReader sr = _cmd.ExecuteReader();

            flag = sr.Read();
            sr.Close();
            _con.Close();
            return flag;
        }
        public void DeleteTableVAlue(string tableName, string condition)
        {
            assert();
            _cmd.CommandText = $"DELETE FROM {tableName} WHERE {condition};";
            _cmd.ExecuteNonQuery();
            SortTableId(tableName);
            _con.Close();
        }
        public void DeleteTableAllValues(string tableName)
        {
            assert();
            _cmd.CommandText = $"DELETE FROM {tableName};";
            _cmd.ExecuteNonQuery();
            //_cmd.CommandText = $"update sqlite_sequence SET seq=0 where name = '{tableName}';";
            //_cmd.ExecuteNonQuery();
            _con.Close();
        }
        public T GeTableItemValue<T>(string tableName, string whereStr)
        {
            if (!assert())
                return default(T);

            T tmp = System.Activator.CreateInstance<T>(); //创建范型类对象
            var tp = tmp.GetType();
            var tpProps = tp.GetProperties(); //获取范型类对象的字段

            _cmd.CommandText = $"SELECT * FROM {tableName} WHERE {whereStr}";
            SQLiteDataReader sr = _cmd.ExecuteReader();

            while (sr.Read())
            {
                for (int i = 0; i < sr.FieldCount; i++)
                {
                    //sr.GetName(i) 获取第i行的键名称
                    //sr[i]         第i行的值
                    var nm = sr.GetName(i);
                    for (int j = 0; j < tpProps.Length; j++)
                    {
                        if (tpProps[j].Name == nm)
                        {
                            tpProps[j].SetValue(tmp, sr[i]);
                            break;
                        }
                    }
                }
            }
            sr.Close();
            _con.Close();
            return default(T);
        }
        public List<T> GeTableItemValues<T>(string tableName)
        {
            if (!assert())
                return null;

            List<T> tmpLs = new List<T>();
            _cmd.CommandText = $"SELECT * FROM {tableName}";
            SQLiteDataReader sr = _cmd.ExecuteReader();

            while (sr.Read())
            {
                T tmp = System.Activator.CreateInstance<T>(); //创建范型类对象
                var tp = tmp.GetType();
                var tpProps = tp.GetProperties(); //获取范型类对象的字段

                for (int i = 0; i < sr.FieldCount; i++)
                {
                    //sr.GetName(i) 获取第i行的键名称
                    //sr[i]         第i行的值
                    var nm = sr.GetName(i);
                    for (int j = 0; j < tpProps.Length; j++)
                    {
                        if (tpProps[j].Name == nm)
                        {
                            tpProps[j].SetValue(tmp, sr[i]);
                            break;
                        }
                    }
                }
                tmpLs.Add(tmp);
            }
            sr.Close();
            _con.Close();
            return tmpLs;
        }
        public List<string[]> GeTableItems(string tableName, string WhereStr = "", string OrderBy = "")
        {
            if (!assert())
                return null;

            if (WhereStr == "")
            {
                _cmd.CommandText = $"SELECT * FROM {tableName}";
            }
            else
            {
                _cmd.CommandText = $"SELECT * FROM {tableName} WHERE {WhereStr}";
            }

            if (OrderBy != "")
            {
                _cmd.CommandText += $" ORDER BY {OrderBy} ASC";
            }


            SQLiteDataReader sr = _cmd.ExecuteReader();

            bool headFlag = true;
            List<string[]> ls = new List<string[]>();
            while (sr.Read())
            {
                string[] arr = new string[sr.FieldCount];
                string[] head = new string[sr.FieldCount];
                for (int i = 0; i < sr.FieldCount; i++)
                {
                    if (headFlag)
                    {
                        head[i] = sr.GetName(i);
                    }
                    arr[i] = sr[i].ToString();
                }
                if (headFlag)
                {
                    ls.Add(head);
                    headFlag = false;
                }
                ls.Add(arr);
            }
            sr.Close();
            _con.Close();
            return ls;
        }
        public Dictionary<string, string> GeTableItemValue(string tableName, string whereStr)
        {
            if (!assert())
                return null;

            _cmd.CommandText = $"SELECT * FROM {tableName} WHERE {whereStr}";
            SQLiteDataReader sr = _cmd.ExecuteReader();

            Dictionary<string, string> dic = new Dictionary<string, string>();
            while (sr.Read())
            {
                for (int i = 0; i < sr.FieldCount; i++)
                {
                    var nm = sr.GetName(i);
                    dic[nm] = sr[i].ToString();
                }
            }
            sr.Close();
            _con.Close();
            return dic;
        }
        public List<string> GeTableColumValue(string tableName, params string[] keys)
        {
            if (!assert())
                return null;

            string key = "";
            for (int i = 0; i < keys.Length; i++)
            {
                if (i == 0)
                {
                    key += keys[i];
                }
                else
                {
                    key += "," + keys[i];
                }
            }

            _cmd.CommandText = $"SELECT {key} FROM {tableName}";
            SQLiteDataReader sr = _cmd.ExecuteReader();

            List<string> ls = new List<string>();
            while (sr.Read())
            {
                string tmp = "";
                for (int i = 0; i < sr.FieldCount; i++)
                {
                    if (i == 0)
                    {
                        tmp += sr[i].ToString();
                    }
                    else
                    {
                        tmp += "_" + sr[i].ToString();
                    }
                }
                ls.Add(tmp);
            }
            sr.Close();
            _con.Close();
            return ls;
        }
        public void UpdateTableValue(string tableName, string update, string condition)
        {
            assert();
            _cmd.CommandText = $"UPDATE {tableName} SET {update} WHERE {condition}";
            //_cmd.CommandText = $"UPDATE {tableName} SET remark='{remark}' WHERE unit={unit} AND id={rowid}";
            _cmd.ExecuteNonQuery();
            SortTableId(tableName);
            _con.Close();
        }
        public void UpdateTableValueForeach(string tableName, List<KeyValuePair<string, string>> commands)
        {
            assert();
            foreach (var item in commands)
            {
                _cmd.CommandText = $"UPDATE {tableName} SET {item.Value} WHERE {item.Key}";
                _cmd.ExecuteNonQuery();
            }
            SortTableId(tableName);
            _con.Close();
        }

        #region 连续操作数据库
        private static object excLock = new object();
        public bool ReadyExecute()
        {
            return assert();
        }
        public void ExecuteFinish()
        {
            _con.Close();
        }

        public void Ex_AddTableValue(string tableName, List<KeyValuePair<string, string>> ls)
        {
            string tableGrid = $"INSERT INTO {tableName} (";
            string tableConetnt = $"VALUES (";
            for (int i = 0; i < ls.Count; i++)
            {
                if (i == 0)
                {
                    tableGrid += ls[i].Key;
                    tableConetnt += "'" + ls[i].Value + "'";
                }
                else
                {
                    tableGrid += ", " + ls[i].Key;
                    tableConetnt += ", '" + ls[i].Value + "'";
                }
            }
            tableGrid += ") ";
            tableConetnt += " );";
            _cmd.CommandText = tableGrid + tableConetnt;
            _cmd.ExecuteNonQuery();
        }

        #endregion


        /// <summary>
        /// 百万级数据操作 事务操作 添加数据
        /// </summary>
        public void AppendHugeTableData(string dbPath, string tableName, List<string> coloumList, List<object[]> rowItemList)
        {
            string insertCmd = "";
            string rowHeader = "";

            string coloumHeader = "insert into " + tableName + "(";

            for (int i = 0; i < coloumList.Count; i++)
            {
                coloumHeader += coloumList[i];

                if (i < coloumList.Count - 1)
                {
                    coloumHeader += ",";
                }
            }

            coloumHeader += ") values( ";

            using (SQLiteConnection sqliteConn = new SQLiteConnection("data source=" + dbPath))
            {
                sqliteConn.Open();

                using (SQLiteCommand command = new SQLiteCommand(sqliteConn))
                {
                    DbTransaction trans = sqliteConn.BeginTransaction();

                    for (int i = 0; i < rowItemList.Count; i++)
                    {
                        rowHeader = "'";

                        for (int j = 0; j < rowItemList[i].Length; j++)
                        {
                            rowHeader += rowItemList[i][j];

                            if (j < rowItemList[i].Length - 1)
                            {
                                rowHeader += "','";
                            }
                            else
                            {
                                rowHeader += "'";
                            }
                        }

                        rowHeader += ")";

                        insertCmd = coloumHeader + rowHeader;

                        command.CommandText = insertCmd;
                        command.ExecuteNonQuery();
                    }

                    trans.Commit();
                }

                if (sqliteConn.State == System.Data.ConnectionState.Open)
                    sqliteConn.Close();
            }
        }

        public void AlterTableField(string tableName, TableField field)
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
