# Advanced-Software-Engineering

##Tests:
dotnet test --collect:"XPlat Code Coverage"
reportgenerator -reports:"TestResults/<zufällig erzeugter Ordner>/coverage.cobertura.xml" -targetdir:"CoverageReport"
        
