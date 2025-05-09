# Advanced-Software-Engineering
Dieses Repository enthält das Projekt, auf dem der Programmentwurf für die Vorlesung "Advanced Software Engineering" beruht. Es handelt sich hierbei um einen einfachen PIC-Simulator, der den angemeldeten Funktionsumfang enthält. Die Funktionsweise des Codes wird hier nicht weiter erklärt, alle wichtigen Ausführungen finden sich in der abgegebenen PDF. Das Repository soll nur als Referenz dienen.

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
