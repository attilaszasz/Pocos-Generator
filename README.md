
Pocos Generator
===============

When all you want are Pocos
---------------------------

If you prefer to work with a micro-orm like [Dapper](https://github.com/StackExchange/Dapper), but hate manually writing POCO classes for your tables, this is the right tool for you.  
A port of [EZPoco T4 templates](https://github.com/davidsavagejr/ezpoco) to .Net Core 2.0+ using [dotnet-script](https://github.com/filipw/dotnet-script/).  

Generates POCO classes from you database tables and views. For SQL Server, also generates a class with names of stored procedures, and a class with the names and columns of table-valued parameters.  

Getting Started
===============

Prerequisites
-------------

The only prerequisite is [dotnet-script](https://github.com/filipw/dotnet-script/).  
Follow the [install instructions](https://github.com/filipw/dotnet-script/#installing) for your platform.

Usage
-----

PocosGenerator work in an opt-in style, regarding you database tables. By default all tables are ignored, you have to manually enable the ones you want to generate a POCO for.  

In the PocosGenerator.csx file, edit the following section:

```C#
if (generator.ReadSchema()){

    // Let's remove ignore for tables and views we need
    /*
        generator.Tables["tablename"].Ignore = false;
    */

    generator.Tables["Users"].Ignore = false;

    /*
        // Tweak Schema
        generator.Tables["tablename"].Ignore = true;                        // To ignore a table
        generator.Tables["tablename"].ClassName = "newname";                // To change the class name of a table
        generator.Tables["tablename"]["columnname"].Ignore = true;          // To ignore a column
        generator.Tables["tablename"]["columnname"].PropertyName="newname"; // To change the property name of a column
        generator.Tables["tablename"]["columnname"].PropertyType="bool";    // To change the property type of a column
    */

    generator.GenerateClass();
}
```

You can execute the script from command line:

```shell
dotnet script PocosGenerator.csx -- output:MyModels.cs connectionstring:ConnectionStrings:MyDatabase config:..\Config\connectionstrings.json
```

Parameters are in form of `param:value`, any order.  

 Parameter | Default       | Purpose  
-----------|:-------------:|----------
 output    | DbModels.cs   | Name of created file.
 config    | appsettings.json | Config file to read the connection string from. Relative to script location.
 connectionstring | ConnectionStrings:DefaultConnection | The connection string to use for the database.
 namespace    | Models  | .Net Namespace of the generated classes.
 pocos    | true | Generate POCOs.
 views    | true | Also include views. If false, only tables are considered.
 schema    | null | Restrict to a specific schema (ex. "dbo.").
 classprefix    | null | Add a prefix to the generated class names (ex. "Accounting*TableName*").
 classsufix    | null | Add a sufix to the generated class names (ex. "*TableName*Reporting").
 spclass    | null | Name for the class holding the stored procedures. Not generated if null.
 tvpclass    | null | Name for the class holding the table valued parameters. Not generated if null.

You can also run it from [Visual Studio Code](https://code.visualstudio.com/). Install the [C# for Visual Studio Code](https://github.com/OmniSharp/omnisharp-vscode/blob/master/debugger.md) plugin.  
To create the launch.json file for you environment, first run this command in the folder containing the .csx files:

```shell
dotnet script init
```

When running from VS Code, set you options in the PocosGenerator.csx file, this section:

```C#
if (!Args.Any()){ 
    options.Output = "DbModels.cs";
    options.ConfigFilePath = @"..\Config\connectionstrings.json";
    options.Namespace = "Db.Models";
    options.ConnectionStringName = "ConnectionStrings:MyDatabase";
    options.SpClass = "spData";
    options.TvpClass = "tvpData";
}
```

Example usage of the generated classes with Dapper
--------------------------------------------------

```C#
        // _connection is an SqlConnection
        public async Task<IEnumerable<Role>> GetAllRoles()
        {
            return await _connection.QueryAsync<Role>(
                spData.dbo_UsersRole_GetAll,
                commandType: CommandType.StoredProcedure
                );
        }
```

---
This script was tested with SQL Server only. To make it work with other databases, edit the `PocosGenerator.GetDbConnection()` method and return a correct DbConnection instance.