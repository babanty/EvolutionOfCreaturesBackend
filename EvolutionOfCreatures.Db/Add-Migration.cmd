
set /p name=enter migration name:
dotnet ef migrations add %name% --startup-project ../EvolutionOfCreatures.App
pause