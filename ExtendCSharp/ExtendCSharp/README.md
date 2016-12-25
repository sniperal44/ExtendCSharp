# ExtendCSharp
Libreria contenente le estensioni delle classi C#

Esempio utilizzo Funzioni MySQL




  MySQLext conn = new MySQLext("localhost", "asd", "root", "");

  if(!conn.ExecuteQuery("insert into reparto (NomeReparto) values('pippo')"))
  {
      MessageBox.Show(conn.LastException.Message);
  }




  List<Type> ListaTipiSelect1 = new Type[] { typeof(int), typeof(String), typeof(DateTime) }.ToList<Type>();

  List<object[]> res= conn.ExecuteReaderQuery("select ID,Nome,`Data di Nascita` from dipendente" ,ListaTipiSelect1);

  foreach(object[] o in res)
  {
      textBoxPlus1.Text += o[0]._Cast<int>()+" - " + o[1]._Cast<String>()+" - " + o[2]._Cast<DateTime>().ToString();
     textBoxPlus1.Text += "\r\n";
  }




  textBoxPlus1.Text += "\r\n\r\n\r\n\r\n\r\n";





  Dictionary<String, Type> t = new Dictionary<string, Type>();
  t.Add("ID", typeof(int));
  t.Add("Nome", typeof(String));
  t.Add("Data di Nascita", typeof(DateTime));

  List<Dictionary<String, object>> ress = conn.ExecuteReaderQuery("select ID,Nome,`Data di Nascita` from dipendente", t);

  bool prima = true;
  foreach (Dictionary<String,object> o in ress)
  {
      if (prima)
      {
          foreach (KeyValuePair<String, object> oo in o)
          {
              textBoxPlus1.Text += oo.Key + " - ";
          }
          textBoxPlus1.Text += "\r\n";
          prima = false;
      }
      textBoxPlus1.Text += o["ID"]._Cast<int>() + " - " + o["Nome"]._Cast<String>() + " - " + o["Data di Nascita"]._Cast<DateTime>().ToString();
      textBoxPlus1.Text += "\r\n";
  }



  conn.Close();