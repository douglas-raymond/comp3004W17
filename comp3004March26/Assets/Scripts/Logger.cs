using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class Logger{

	string path;
	string source;

	public Logger(string _source)
	{
		source = _source;
		path = "./log.txt";
	}

	public void Init(){
		try
		{
			FileStream fs = new FileStream(path, FileMode.Create);
			fs.Close();
		}
		catch(Exception ex){
			Console.WriteLine(ex.ToString());
		}

	}
	
	public void log(string msg)
	{
		StreamWriter file = new StreamWriter(path, true);
		file.WriteLine("INFO ["+source+"]: "+msg);
		file.Close ();
	}

	public void setSource(string _source){
		source = _source;
	}
}
