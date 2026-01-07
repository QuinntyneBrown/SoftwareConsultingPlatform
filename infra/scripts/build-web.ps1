Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$serviceRoot = Resolve-Path (Join-Path $PSScriptRoot '..\..\src\SoftwareConsultingPlatform.WebApp')
Push-Location $serviceRoot
try {
  Write-Host "Building Angular app in $serviceRoot"

  npm ci
  npm run build

  $distPath = Join-Path $serviceRoot 'dist\software-consulting-platform\browser'
  if (-not (Test-Path $distPath)) {
    throw "Build output not found at: $distPath"
  }

  Write-Host "Build output ready: $distPath"
}
finally {
  Pop-Location
}
