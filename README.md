# LedgerCo

Solution For Ledger Co problem statement

To build the solution use below command

```
dotnet build -o geektrust
```

To get the output you can use below commands

```
dotnet geektrust/geektrust.dll Input1.txt
dotnet geektrust/geektrust.dll Input2.txt
```

To execute test cases execute below command

```
dotnet test
```

For calculating the test coverage run below command

```
dotnet test --collect="XPlat Code Coverage"
```

To generate coverage report in html format after generating code coverage report, install Report Generator Tool from https://www.nuget.org/packages/dotnet-reportgenerator-globaltool and then run below command.

```
reportgenerator -reports:"{PathToCodeBase}\Ledger\Ledger.Test\TestResults\{GUID}\coverage.cobertura.xml" -targetdir:"{OutputDirectory}" -reporttypes:Html
```

_PathToCodeBase : Folder where code base is located._

_GUID: Folder name generated after running code coverage command._

_OutputDirectory: Folder where you need html reports and supporting files to be placed._

# Questions?

1. What should be shown when Balance requested with emi number greater than choosen emi count?
