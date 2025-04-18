# Advanced-Software-Engineering

## Testausführung & Code Coverage

### Tests ausführen mit Code Coverage

```bash
dotnet test --collect:"XPlat Code Coverage"
reportgenerator -reports:"TestResults/<zufällig erzeugter Ordner>/coverage.cobertura.xml" -targetdir:"CoverageReport"
```
=> "Advanced-Software-Engineering/TestResults"  
=> "Advanced-Software-Engineering/CoverageReport" -> "Advanced-Software-Engineering/CoverageReport/index.html" für Visualisierung der CodeCoverage

