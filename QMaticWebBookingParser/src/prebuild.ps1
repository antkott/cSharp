#powershell $(ProjectDir)prebuild.ps1
try{
    Stop-Process -Name "chromedriver" -Force -ErrorAction SilentlyContinue
} catch {
    exit 0
}