using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace C971_Mobile_App
{
    public interface ISQLiteDb
    {
        SQLiteAsyncConnection GetConnection();
    }
}
