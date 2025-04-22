# Advanced-Software-Engineering

## Testausführung & Code Coverage

### Tests ausführen mit Code Coverage

Im <...>\Advanced-Software-Engineering\Tests"-Ordner:    
```bash
dotnet test --collect:"XPlat Code Coverage"
reportgenerator -reports:"TestResults/<zufällig erzeugter Ordner>/coverage.cobertura.xml" -targetdir:"CoverageReport"
```
Im <...>\Advanced-Software-Engineering\WpfApp_PIC_Test"-Ordner:    
```bash
reportgenerator -reports:"../Tests/TestResults/<zufällig erzeugter Ordner>/coverage.cobertura.xml" -targetdir:"../Tests/CoverageReport"
```
=> "Advanced-Software-Engineering/TestResults"  
=> "Advanced-Software-Engineering/CoverageReport" -> "Advanced-Software-Engineering/CoverageReport/index.html" für Visualisierung der CodeCoverage

Anzeigen 
```bash
start CoverageReport/index.html
```
