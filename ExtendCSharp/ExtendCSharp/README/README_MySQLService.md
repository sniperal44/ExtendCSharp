# ExtendCSharp
Esempio utilizzo Funzioni MySQL



	//Creazione della connessione
  MySQLext conn = new MySQLext("localhost", "DB_Name", "user", "pass");

  //Esecuzione di una Query senza valore di ritorno
  if(!conn.ExecuteQuery("insert into reparto (NomeReparto) values('pippo')"))
  {
      MessageBox.Show(conn.LastException.Message);
  }


  //Esecuzione di una Query CON valore di ritorno
  //occorre prima creare una classe che permette la lettura dei dati: 
  //Visionare la classe ExtendCSharp.README.DemoClasses.MySQL_demo_Class

  String query="select * from test";
  IEnumerable<MySQL_demo_Class> results=sql.ExecuteReaderQuery<MySQL_demo_Class>(query);


  //chiusura della connessione
  conn.Close();