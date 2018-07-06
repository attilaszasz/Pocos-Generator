#! "netcoreapp2.1"
#load "PocosGenerator.Core.csx"

//when running from VS Code, set your options here
if (!Args.Any()){ 
    options.Output = "DbModels.cs";
    options.ConfigFilePath = @"..\Config\connectionstrings.json";
    options.Namespace = "Db.Models";
    options.ConnectionStringName = "ConnectionStrings:Data";
	options.SpClass = "spData";
	options.TvpClass = "tvpData";
}

var generator = new PocosGenerator(options);

WriteLine($"Connecting to database: {zap_password(generator.ConnectionString)}");

if (generator.ReadSchema()){

	// Let's remove ignore for tables and views we need
    /*
		generator.Tables["tablename"].Ignore = false;
	*/
	
	/*
		// Tweak Schema
		generator.Tables["tablename"].Ignore = true;						// To ignore a table
		generator.Tables["tablename"].ClassName = "newname";				// To change the class name of a table
		generator.Tables["tablename"]["columnname"].Ignore = true;			// To ignore a column
		generator.Tables["tablename"]["columnname"].PropertyName="newname";	// To change the property name of a column
		generator.Tables["tablename"]["columnname"].PropertyType="bool";	// To change the property type of a column
	*/

	generator.GenerateClass();
}

WriteLine($"Attempting to write generated content to {options.Output}");
System.IO.File.WriteAllText(options.Output, generator.Content);
WriteLine("Finished.");